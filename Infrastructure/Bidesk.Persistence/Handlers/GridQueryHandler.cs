using Bidesk.Application.Custom_Attributes;
using Bidesk.Application.DTOs.Primeng;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Bidesk.Persistence.Handlers
{
    /// <summary>
    /// PrimeNG grid isteklerini SQL'e çevrilebilir sorgulara dönüştüren genel amaçlı işleyici.
    /// Kaynak IQueryable zaten havuza ait filtrelerden geçmiş olmalıdır (CompanyID, IsCanceled).
    /// Filtre / sıralama / sayfalama veritabanı tarafında çalışır.
    /// </summary>
    public class GridQueryHandler<TResult> where TResult : class
    {
        private const int MaxPageRows = 100;
        private const int MaxExportRows = 5000;
        private const int DefaultPageRows = 10;

        private readonly IQueryable<TResult> _source;
        private readonly GridQueryDto _gridQuery;
        private readonly Action<List<TResult>>? _enrich;
        private readonly ILogger? _logger;

        public GridQueryHandler(
            IQueryable<TResult> source,
            GridQueryDto gridQuery,
            Action<List<TResult>>? enrich = null,
            ILogger? logger = null)
        {
            _source = source;
            _gridQuery = gridQuery;
            _enrich = enrich;
            _logger = logger;
        }

        public async Task<GridReturnDto<TResult>> GetDataAsync(CancellationToken cancellationToken = default)
        {
            IQueryable<TResult> query = _source;

            query = ApplyFilters(query);
            query = ApplySearch(query);

            long totalRecords = await query.LongCountAsync(cancellationToken);

            query = ApplySort(query);
            query = ApplyPagination(query);

            List<TResult> data = await query.ToListAsync(cancellationToken);

            _enrich?.Invoke(data);

            return new GridReturnDto<TResult> { Data = data, TotalRecords = totalRecords };
        }

        private IQueryable<TResult> ApplySearch(IQueryable<TResult> query)
        {
            string? term = _gridQuery.Search?.Trim();
            if (string.IsNullOrEmpty(term)) return query;

            ParameterExpression param = Expression.Parameter(typeof(TResult), "x");
            Expression? combined = null;

            MethodInfo toLower = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!;
            MethodInfo contains = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!;
            Expression needle = Expression.Constant(term.ToLower(), typeof(string));

            foreach (PropertyInfo prop in typeof(TResult).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (prop.PropertyType != typeof(string)) continue;
                if (prop.GetCustomAttribute<EnrichedFieldAttribute>() != null) continue;
                if (prop.GetCustomAttribute<JsonIgnoreAttribute>() != null) continue;

                Expression property = Expression.Property(param, prop);
                Expression notNull = Expression.NotEqual(property, Expression.Constant(null, typeof(string)));
                Expression call = Expression.Call(Expression.Call(property, toLower), contains, needle);
                Expression predicate = Expression.AndAlso(notNull, call);

                combined = combined == null ? predicate : Expression.OrElse(combined, predicate);
            }

            // Serbest metin araması yalnız string kolonları tarar; "2651" gibi sayısal bir
            // giriş aksi halde boş döner. Sayısal terim kayıt no (ID) eşitliği olarak da aranır.
            if (long.TryParse(term, out long numericTerm))
            {
                PropertyInfo? idProp = typeof(TResult).GetProperty("ID",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                Type? underlying = idProp == null ? null : Nullable.GetUnderlyingType(idProp.PropertyType) ?? idProp.PropertyType;
                if (idProp != null &&
                    (underlying == typeof(long) ||
                     (underlying == typeof(int) && numericTerm >= int.MinValue && numericTerm <= int.MaxValue)))
                {
                    object idValue = Convert.ChangeType(numericTerm, underlying!);
                    Expression property = Expression.Property(param, idProp);
                    Expression predicate = Expression.Equal(property, Expression.Constant(idValue, idProp.PropertyType));

                    combined = combined == null ? predicate : Expression.OrElse(combined, predicate);
                }
            }

            if (combined == null) return query;

            return query.Where(Expression.Lambda<Func<TResult, bool>>(combined, param));
        }

        private IQueryable<TResult> ApplyFilters(IQueryable<TResult> query)
        {
            if (_gridQuery.LoadEvent == null || _gridQuery.LoadEvent.Filters.Count == 0) return query;

            ParameterExpression param = Expression.Parameter(typeof(TResult), "x");
            Expression? combined = null;

            foreach (FilterDto filterField in _gridQuery.LoadEvent.Filters)
            {
                if (!TryResolveField(param, filterField.PropertyName, out Expression? property))
                {
                    Warn("Grid filtresi geçersiz alana geldi, yok sayıldı: {Field}", filterField.FieldPath);
                    continue;
                }

                bool useOr = filterField.Filters.Any(f => f.Operator == "or");
                Expression? group = null;

                foreach (FilterValueDto filter in filterField.Filters)
                {
                    Expression? predicate = BuildPredicate(property!, filter);
                    if (predicate == null)
                    {
                        Warn("Grid filtresi çevrilemedi: {Field} {Mode} '{Value}'", filterField.FieldPath, filter.MatchMode, filter.Value);
                        continue;
                    }
                    group = group == null ? predicate : useOr ? Expression.OrElse(group, predicate) : Expression.AndAlso(group, predicate);
                }

                if (group == null) continue;
                combined = combined == null ? group : Expression.AndAlso(combined, group);
            }

            if (combined == null) return query;
            return query.Where(Expression.Lambda<Func<TResult, bool>>(combined, param));
        }

        private Expression? BuildPredicate(Expression property, FilterValueDto filter)
        {
            Type propType = property.Type;
            string matchMode = filter.MatchMode;

            if ((propType == typeof(bool) || propType == typeof(bool?)) && matchMode == "contains")
                matchMode = "equals";

            if (matchMode == "in")
                return BuildInPredicate(property, filter);

            if (propType == typeof(string) && matchMode is "contains" or "notContains" or "startsWith" or "endsWith")
                return BuildStringPredicate(property, matchMode, filter.Value);

            if (!TryConvertValue(filter.Value, propType, out object? converted)) return null;

            Expression value = Expression.Constant(converted, propType);

            return matchMode switch
            {
                "equals" or "is" => Expression.Equal(property, value),
                "notEquals" or "isNot" => Expression.NotEqual(property, value),
                "gt" => Expression.GreaterThan(property, value),
                "gte" => Expression.GreaterThanOrEqual(property, value),
                "lt" => Expression.LessThan(property, value),
                "lte" => Expression.LessThanOrEqual(property, value),
                "dateIs" or "dateIsNot" => BuildDateRangePredicate(property, converted, matchMode == "dateIs"),
                "dateBefore" => Expression.LessThan(property, value),
                "dateAfter" => Expression.GreaterThan(property, value),
                _ => null
            };
        }

        private static Expression BuildDateRangePredicate(Expression property, object? converted, bool inclusive)
        {
            if (converted is not DateTime day) return Expression.Constant(true);
            Type propType = property.Type;
            Expression dayStart = Expression.Constant(day, propType);
            Expression dayEnd = Expression.Constant(day.AddDays(1), propType);
            Expression inDay = Expression.AndAlso(
                Expression.GreaterThanOrEqual(property, dayStart),
                Expression.LessThan(property, dayEnd));
            return inclusive ? inDay : Expression.Not(inDay);
        }

        private static Expression BuildStringPredicate(Expression property, string matchMode, string value)
        {
            string methodName = matchMode switch
            {
                "startsWith" => nameof(string.StartsWith),
                "endsWith" => nameof(string.EndsWith),
                _ => nameof(string.Contains)
            };
            MethodInfo toLower = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!;
            MethodInfo method = typeof(string).GetMethod(methodName, new[] { typeof(string) })!;
            Expression notNull = Expression.NotEqual(property, Expression.Constant(null, typeof(string)));
            Expression call = Expression.Call(Expression.Call(property, toLower), method,
                Expression.Constant(value?.ToLower() ?? string.Empty, typeof(string)));
            return matchMode == "notContains"
                ? Expression.AndAlso(notNull, Expression.Not(call))
                : Expression.AndAlso(notNull, call);
        }

        private Expression? BuildInPredicate(Expression property, FilterValueDto filter)
        {
            string[] rawValues = filter.Values
                ?? (string.IsNullOrEmpty(filter.Value) ? Array.Empty<string>() : new[] { filter.Value });
            if (rawValues.Length == 0) return null;

            Type listType = typeof(List<>).MakeGenericType(property.Type);
            IList values = (IList)Activator.CreateInstance(listType)!;
            foreach (string raw in rawValues)
                if (TryConvertValue(raw, property.Type, out object? converted))
                    values.Add(converted);

            if (values.Count == 0) return null;

            MethodInfo contains = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                .First(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2)
                .MakeGenericMethod(property.Type);

            return Expression.Call(null, contains, Expression.Constant(values, listType), property);
        }

        private IQueryable<TResult> ApplySort(IQueryable<TResult> query)
        {
            ParameterExpression param = Expression.Parameter(typeof(TResult), "x");
            IOrderedQueryable<TResult>? ordered = null;

            if (_gridQuery.LoadEvent != null)
            {
                foreach (SortMetaDto sort in _gridQuery.LoadEvent.Sorts)
                {
                    if (!TryResolveField(param, sort.Field, out Expression? property))
                    {
                        Warn("Grid sıralaması geçersiz alana geldi: {Field}", string.Join(".", sort.Field));
                        continue;
                    }
                    string methodName = ordered == null
                        ? (sort.Order == 1 ? nameof(Queryable.OrderBy) : nameof(Queryable.OrderByDescending))
                        : (sort.Order == 1 ? nameof(Queryable.ThenBy) : nameof(Queryable.ThenByDescending));
                    ordered = InvokeOrderMethod(methodName, ordered ?? query, Expression.Lambda(property!, param), property!.Type);
                }
            }

            if (ordered != null) return ordered;

            // Deterministik sıralama: ID azalan (en yeni üstte).
            if (TryResolveField(param, new[] { "ID" }, out Expression? idProp))
                return InvokeOrderMethod(nameof(Queryable.OrderByDescending), query, Expression.Lambda(idProp!, param), idProp!.Type);

            return query;
        }

        private static IOrderedQueryable<TResult> InvokeOrderMethod(string methodName, IQueryable<TResult> source, LambdaExpression lambda, Type keyType)
        {
            return (IOrderedQueryable<TResult>)typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TResult), keyType)
                .Invoke(null, new object[] { source, lambda })!;
        }

        private IQueryable<TResult> ApplyPagination(IQueryable<TResult> query)
        {
            if (_gridQuery.SkipPagination || _gridQuery.LoadEvent == null)
                return query.Take(MaxExportRows);

            int rows = Math.Clamp(_gridQuery.LoadEvent.Rows <= 0 ? DefaultPageRows : _gridQuery.LoadEvent.Rows, 1, MaxPageRows);
            int first = Math.Max(_gridQuery.LoadEvent.First, 0);
            return query.Skip(first).Take(rows);
        }

        private static bool TryResolveField(ParameterExpression param, string[] path, out Expression? propertyAccess)
        {
            propertyAccess = null;
            if (path.Length == 0) return false;

            PropertyInfo? root = typeof(TResult).GetProperty(path[0],
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (root == null || root.GetCustomAttribute<EnrichedFieldAttribute>() != null) return false;

            Expression current = param;
            try
            {
                foreach (string segment in path)
                    current = Expression.PropertyOrField(current, segment);
            }
            catch (ArgumentException) { return false; }

            propertyAccess = current;
            return true;
        }

        private static bool TryConvertValue(string raw, Type targetType, out object? converted)
        {
            converted = null;
            Type underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;
            try
            {
                if (underlying == typeof(DateTime))
                {
                    DateTime parsed = DateTime.Parse(raw, CultureInfo.InvariantCulture,
                        DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
                    converted = DateTime.SpecifyKind(parsed, DateTimeKind.Utc);
                    return true;
                }
                TypeConverter converter = TypeDescriptor.GetConverter(underlying);
                converted = converter.ConvertFromInvariantString(raw);
                return converted != null;
            }
            catch { return false; }
        }

        private void Warn(string message, params object?[] args) => _logger?.LogWarning(message, args);
    }
}
