namespace Bidesk.Application.Custom_Attributes
{
    /// <summary>
    /// Enrich adımında bellek içinde doldurulan grid kolonunu işaretler.
    /// Bu kolonlar SQL'e çevrilemediği için filtre ve sıralama istekleri yok sayılır.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class EnrichedFieldAttribute : Attribute { }
}
