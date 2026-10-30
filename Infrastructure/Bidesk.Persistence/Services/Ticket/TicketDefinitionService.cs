using Bidesk.Application.Abstraction.Services.Ticket;
using Bidesk.Application.Abstraction.Services.User;
using Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketCategory;
using Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketGroup;
using Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketPriority;
using Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketStatus;
using Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketSubCategory;
using Bidesk.Application.Features.Commands.TicketDefinition.SaveBideskParameter;
using Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketCategory;
using Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketGroup;
using Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketPriority;
using Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketStatus;
using Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketSubCategory;
using Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketCategories;
using Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketGroups;
using Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketPriorities;
using Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketStatuses;
using Bidesk.Application.Features.Queries.TicketDefinition.GetBideskParameters;
using Bidesk.Application.Features.Queries.TicketDefinition.GetBideskPersonnel;
using Bidesk.Application.Features.Queries.TicketDefinition.GetTicketLookups;
using Bidesk.Application.Repositories.BideskParameter;
using Bidesk.Application.Repositories.TechnicianGroup;
using Bidesk.Application.Repositories.Ticket;
using Bidesk.Application.Repositories.TicketCategory;
using Bidesk.Application.Repositories.TicketGroup;
using Bidesk.Application.Repositories.TicketPriority;
using Bidesk.Application.Repositories.TicketStatus;
using Bidesk.Application.Repositories.TicketSubCategory;
using Bidesk.Application.ViewModels.Ticket;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Bidesk.Persistence.Services.Ticket
{
    public class TicketDefinitionService : ITicketDefinitionService
    {
        public const string AutoCloseDaysParam = "AutoCloseDays";

        private readonly IUserService _userService;
        private readonly ITicketReadRepository _ticketReadRepository;
        private readonly ITicketStatusReadRepository _statusReadRepository;
        private readonly ITicketStatusWriteRepository _statusWriteRepository;
        private readonly ITicketPriorityReadRepository _priorityReadRepository;
        private readonly ITicketPriorityWriteRepository _priorityWriteRepository;
        private readonly ITicketGroupReadRepository _groupReadRepository;
        private readonly ITicketGroupWriteRepository _groupWriteRepository;
        private readonly ITechnicianGroupReadRepository _technicianGroupReadRepository;
        private readonly ITechnicianGroupWriteRepository _technicianGroupWriteRepository;
        private readonly ITicketCategoryReadRepository _categoryReadRepository;
        private readonly ITicketCategoryWriteRepository _categoryWriteRepository;
        private readonly ITicketSubCategoryReadRepository _subCategoryReadRepository;
        private readonly ITicketSubCategoryWriteRepository _subCategoryWriteRepository;
        private readonly IBideskParameterReadRepository _parameterReadRepository;
        private readonly IBideskParameterWriteRepository _parameterWriteRepository;

        public TicketDefinitionService(IUserService userService, ITicketReadRepository ticketReadRepository,
            ITicketStatusReadRepository statusReadRepository, ITicketStatusWriteRepository statusWriteRepository,
            ITicketPriorityReadRepository priorityReadRepository, ITicketPriorityWriteRepository priorityWriteRepository,
            ITicketGroupReadRepository groupReadRepository, ITicketGroupWriteRepository groupWriteRepository,
            ITechnicianGroupReadRepository technicianGroupReadRepository, ITechnicianGroupWriteRepository technicianGroupWriteRepository,
            ITicketCategoryReadRepository categoryReadRepository, ITicketCategoryWriteRepository categoryWriteRepository,
            ITicketSubCategoryReadRepository subCategoryReadRepository, ITicketSubCategoryWriteRepository subCategoryWriteRepository,
            IBideskParameterReadRepository parameterReadRepository, IBideskParameterWriteRepository parameterWriteRepository)
        {
            _userService = userService;
            _ticketReadRepository = ticketReadRepository;
            _statusReadRepository = statusReadRepository;
            _statusWriteRepository = statusWriteRepository;
            _priorityReadRepository = priorityReadRepository;
            _priorityWriteRepository = priorityWriteRepository;
            _groupReadRepository = groupReadRepository;
            _groupWriteRepository = groupWriteRepository;
            _technicianGroupReadRepository = technicianGroupReadRepository;
            _technicianGroupWriteRepository = technicianGroupWriteRepository;
            _categoryReadRepository = categoryReadRepository;
            _categoryWriteRepository = categoryWriteRepository;
            _subCategoryReadRepository = subCategoryReadRepository;
            _subCategoryWriteRepository = subCategoryWriteRepository;
            _parameterReadRepository = parameterReadRepository;
            _parameterWriteRepository = parameterWriteRepository;
        }

        #region Queries

        public async Task<GetTicketLookupsQRes> GetTicketLookups(GetTicketLookupsQReq request, CancellationToken cancellationToken)
        {
            try
            {
                long userId = _userService.GetCurrentUserID();
                var currentUser = await _userService.GetCurrentUserAsync();
                bool isTechnician = (currentUser?.IsAdmin ?? false)
                    || await _technicianGroupReadRepository.GetWhere(t => t.UserID == userId, tracking: false).AnyAsync(cancellationToken);

                var lookups = new TicketLookupsVM
                {
                    Statuses = await QueryStatuses(onlyActive: true).ToListAsync(cancellationToken),
                    Priorities = await QueryPriorities(onlyActive: true).ToListAsync(cancellationToken),
                    Groups = await LoadGroupsAsync(onlyActive: true, cancellationToken),
                    Categories = await LoadCategoriesAsync(onlyActive: true, cancellationToken),
                    IsTechnician = isTechnician
                };

                return ResponseFactory.Success<GetTicketLookupsQRes>(configure: r => r.Lookups = lookups);
            }
            catch (Exception ex) { return ResponseFactory.Error<GetTicketLookupsQRes>(ex); }
        }

        public async Task<GetAllTicketStatusesQRes> GetAllTicketStatuses(GetAllTicketStatusesQReq request, CancellationToken cancellationToken)
        {
            try
            {
                var list = await QueryStatuses(onlyActive: false).ToListAsync(cancellationToken);
                return ResponseFactory.Success<GetAllTicketStatusesQRes>(configure: r => r.Statuses = list);
            }
            catch (Exception ex) { return ResponseFactory.Error<GetAllTicketStatusesQRes>(ex); }
        }

        public async Task<GetAllTicketPrioritiesQRes> GetAllTicketPriorities(GetAllTicketPrioritiesQReq request, CancellationToken cancellationToken)
        {
            try
            {
                var list = await QueryPriorities(onlyActive: false).ToListAsync(cancellationToken);
                return ResponseFactory.Success<GetAllTicketPrioritiesQRes>(configure: r => r.Priorities = list);
            }
            catch (Exception ex) { return ResponseFactory.Error<GetAllTicketPrioritiesQRes>(ex); }
        }

        public async Task<GetAllTicketGroupsQRes> GetAllTicketGroups(GetAllTicketGroupsQReq request, CancellationToken cancellationToken)
        {
            try
            {
                var list = await LoadGroupsAsync(onlyActive: false, cancellationToken);
                return ResponseFactory.Success<GetAllTicketGroupsQRes>(configure: r => r.Groups = list);
            }
            catch (Exception ex) { return ResponseFactory.Error<GetAllTicketGroupsQRes>(ex); }
        }

        public async Task<GetAllTicketCategoriesQRes> GetAllTicketCategories(GetAllTicketCategoriesQReq request, CancellationToken cancellationToken)
        {
            try
            {
                var list = await LoadCategoriesAsync(onlyActive: false, cancellationToken);
                return ResponseFactory.Success<GetAllTicketCategoriesQRes>(configure: r => r.Categories = list);
            }
            catch (Exception ex) { return ResponseFactory.Error<GetAllTicketCategoriesQRes>(ex); }
        }

        public async Task<GetBideskParametersQRes> GetBideskParameters(GetBideskParametersQReq request, CancellationToken cancellationToken)
        {
            try
            {
                var list = await _parameterReadRepository.GetAll(tracking: false)
                    .OrderBy(p => p.ParamName)
                    .Select(p => new BideskParameterVM { ParamName = p.ParamName, ParamValue = p.ParamValue })
                    .ToListAsync(cancellationToken);
                return ResponseFactory.Success<GetBideskParametersQRes>(configure: r => r.Parameters = list);
            }
            catch (Exception ex) { return ResponseFactory.Error<GetBideskParametersQRes>(ex); }
        }

        public async Task<GetBideskPersonnelQRes> GetBideskPersonnel(GetBideskPersonnelQReq request, CancellationToken cancellationToken)
        {
            try
            {
                var list = await _userService.GetActivePersonnelAsync();
                return ResponseFactory.Success<GetBideskPersonnelQRes>(configure: r => r.Personnel = list);
            }
            catch (Exception ex) { return ResponseFactory.Error<GetBideskPersonnelQRes>(ex); }
        }

        #endregion

        #region Status / Priority

        public async Task<SaveTicketStatusCmdRes> SaveTicketStatus(SaveTicketStatusCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                TicketStatusVM vm = request.Status;
                if (string.IsNullOrWhiteSpace(vm.Name))
                    return ResponseFactory.Fail<SaveTicketStatusCmdRes>("Durum adı zorunludur.");
                if (vm.IsDefault && !vm.IsActive)
                    return ResponseFactory.Fail<SaveTicketStatusCmdRes>("Varsayılan durum pasif olamaz.");

                string name = vm.Name.Trim();
                if (await _statusReadRepository.GetWhere(s => s.ID != vm.ID && s.Name == name).AnyAsync(cancellationToken))
                    return ResponseFactory.Fail<SaveTicketStatusCmdRes>("Bu adla bir durum zaten var.");

                TicketStatusEnt? entity = vm.ID > 0 ? await _statusReadRepository.GetByIDAsync(vm.ID) : new TicketStatusEnt();
                if (entity == null) return ResponseFactory.Fail<SaveTicketStatusCmdRes>("Durum bulunamadı.");

                if (entity.IsDefault && !vm.IsDefault)
                    return ResponseFactory.Fail<SaveTicketStatusCmdRes>("Varsayılan durum kaldırılamaz; başka bir durumu varsayılan yapın.");

                entity.Name = name;
                entity.Severity = vm.Severity;
                entity.SortOrder = vm.SortOrder;
                entity.IsDefault = vm.IsDefault;
                entity.IsResolved = vm.IsResolved || vm.IsClosed;
                entity.IsClosed = vm.IsClosed;
                entity.PausesSla = vm.PausesSla;
                entity.IsActive = vm.IsActive;

                // Tek varsayilan: yenisi isaretlenince eskisi duser.
                if (vm.IsDefault)
                {
                    var others = await _statusReadRepository.GetWhere(s => s.IsDefault && s.ID != vm.ID).ToListAsync(cancellationToken);
                    others.ForEach(o => o.IsDefault = false);
                }

                if (entity.ID == 0) await _statusWriteRepository.AddAsync(entity);
                await _statusWriteRepository.SaveAsync();
                return ResponseFactory.Success<SaveTicketStatusCmdRes>("Durum kaydedildi.", r => r.Id = entity.ID);
            }
            catch (Exception ex) { return ResponseFactory.Error<SaveTicketStatusCmdRes>(ex); }
        }

        public async Task<DeleteTicketStatusCmdRes> DeleteTicketStatus(DeleteTicketStatusCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _statusReadRepository.GetByIDAsync(request.ID);
                if (entity == null) return ResponseFactory.Fail<DeleteTicketStatusCmdRes>("Durum bulunamadı.");
                if (entity.IsDefault) return ResponseFactory.Fail<DeleteTicketStatusCmdRes>("Varsayılan durum silinemez.");
                if (await _ticketReadRepository.GetWhere(t => t.StatusID == request.ID).AnyAsync(cancellationToken))
                    return ResponseFactory.Fail<DeleteTicketStatusCmdRes>("Bu durumda talepler var; silmek yerine pasif yapın.");

                _statusWriteRepository.Remove(entity);
                await _statusWriteRepository.SaveAsync();
                return ResponseFactory.Success<DeleteTicketStatusCmdRes>("Durum silindi.");
            }
            catch (Exception ex) { return ResponseFactory.Error<DeleteTicketStatusCmdRes>(ex); }
        }

        public async Task<SaveTicketPriorityCmdRes> SaveTicketPriority(SaveTicketPriorityCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                TicketPriorityVM vm = request.Priority;
                if (string.IsNullOrWhiteSpace(vm.Name))
                    return ResponseFactory.Fail<SaveTicketPriorityCmdRes>("Öncelik adı zorunludur.");
                if (vm.ResolveHours is < 0)
                    return ResponseFactory.Fail<SaveTicketPriorityCmdRes>("Hedef süre negatif olamaz.");

                string name = vm.Name.Trim();
                if (await _priorityReadRepository.GetWhere(s => s.ID != vm.ID && s.Name == name).AnyAsync(cancellationToken))
                    return ResponseFactory.Fail<SaveTicketPriorityCmdRes>("Bu adla bir öncelik zaten var.");

                TicketPriorityEnt? entity = vm.ID > 0 ? await _priorityReadRepository.GetByIDAsync(vm.ID) : new TicketPriorityEnt();
                if (entity == null) return ResponseFactory.Fail<SaveTicketPriorityCmdRes>("Öncelik bulunamadı.");
                if (entity.IsDefault && !vm.IsDefault)
                    return ResponseFactory.Fail<SaveTicketPriorityCmdRes>("Varsayılan öncelik kaldırılamaz; başka bir önceliği varsayılan yapın.");

                entity.Name = name;
                entity.Severity = vm.Severity;
                entity.SortOrder = vm.SortOrder;
                entity.IsDefault = vm.IsDefault;
                entity.ResolveHours = vm.ResolveHours;
                entity.IsActive = vm.IsActive || vm.IsDefault;

                if (vm.IsDefault)
                {
                    var others = await _priorityReadRepository.GetWhere(s => s.IsDefault && s.ID != vm.ID).ToListAsync(cancellationToken);
                    others.ForEach(o => o.IsDefault = false);
                }

                if (entity.ID == 0) await _priorityWriteRepository.AddAsync(entity);
                await _priorityWriteRepository.SaveAsync();
                return ResponseFactory.Success<SaveTicketPriorityCmdRes>("Öncelik kaydedildi.", r => r.Id = entity.ID);
            }
            catch (Exception ex) { return ResponseFactory.Error<SaveTicketPriorityCmdRes>(ex); }
        }

        public async Task<DeleteTicketPriorityCmdRes> DeleteTicketPriority(DeleteTicketPriorityCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _priorityReadRepository.GetByIDAsync(request.ID);
                if (entity == null) return ResponseFactory.Fail<DeleteTicketPriorityCmdRes>("Öncelik bulunamadı.");
                if (entity.IsDefault) return ResponseFactory.Fail<DeleteTicketPriorityCmdRes>("Varsayılan öncelik silinemez.");
                if (await _ticketReadRepository.GetWhere(t => t.PriorityID == request.ID).AnyAsync(cancellationToken))
                    return ResponseFactory.Fail<DeleteTicketPriorityCmdRes>("Bu öncelikte talepler var; silmek yerine pasif yapın.");

                _priorityWriteRepository.Remove(entity);
                await _priorityWriteRepository.SaveAsync();
                return ResponseFactory.Success<DeleteTicketPriorityCmdRes>("Öncelik silindi.");
            }
            catch (Exception ex) { return ResponseFactory.Error<DeleteTicketPriorityCmdRes>(ex); }
        }

        #endregion

        #region Group / Technician

        public async Task<SaveTicketGroupCmdRes> SaveTicketGroup(SaveTicketGroupCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                TicketGroupVM vm = request.Group;
                if (string.IsNullOrWhiteSpace(vm.Name))
                    return ResponseFactory.Fail<SaveTicketGroupCmdRes>("Grup adı zorunludur.");

                string name = vm.Name.Trim();
                if (await _groupReadRepository.GetWhere(s => s.ID != vm.ID && s.Name == name).AnyAsync(cancellationToken))
                    return ResponseFactory.Fail<SaveTicketGroupCmdRes>("Bu adla bir grup zaten var.");

                TicketGroupEnt? entity = vm.ID > 0 ? await _groupReadRepository.GetByIDAsync(vm.ID) : new TicketGroupEnt();
                if (entity == null) return ResponseFactory.Fail<SaveTicketGroupCmdRes>("Grup bulunamadı.");

                entity.Name = name;
                entity.Email = string.IsNullOrWhiteSpace(vm.Email) ? null : vm.Email.Trim();
                entity.SortOrder = vm.SortOrder;
                entity.IsActive = vm.IsActive;

                if (entity.ID == 0)
                {
                    await _groupWriteRepository.AddAsync(entity);
                    await _groupWriteRepository.SaveAsync();
                }

                // Teknisyen listesini esitle: cikan soft-delete, yeni gelen eklenir.
                List<long> wanted = vm.Technicians.Select(t => t.UserID).Distinct().ToList();
                var current = await _technicianGroupReadRepository.GetWhere(t => t.GroupID == entity.ID).ToListAsync(cancellationToken);

                _technicianGroupWriteRepository.RemoveRange(current.Where(c => !wanted.Contains(c.UserID)).ToList());
                var toAdd = wanted.Where(u => !current.Any(c => c.UserID == u))
                    .Select(u => new TechnicianGroupEnt { GroupID = entity.ID, UserID = u })
                    .ToList();
                if (toAdd.Count > 0) await _technicianGroupWriteRepository.AddRangeAsync(toAdd);

                await _groupWriteRepository.SaveAsync();
                return ResponseFactory.Success<SaveTicketGroupCmdRes>("Grup kaydedildi.", r => r.Id = entity.ID);
            }
            catch (Exception ex) { return ResponseFactory.Error<SaveTicketGroupCmdRes>(ex); }
        }

        public async Task<DeleteTicketGroupCmdRes> DeleteTicketGroup(DeleteTicketGroupCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _groupReadRepository.GetByIDAsync(request.ID);
                if (entity == null) return ResponseFactory.Fail<DeleteTicketGroupCmdRes>("Grup bulunamadı.");
                if (await _ticketReadRepository.GetWhere(t => t.GroupID == request.ID).AnyAsync(cancellationToken))
                    return ResponseFactory.Fail<DeleteTicketGroupCmdRes>("Bu grupta talepler var; silmek yerine pasif yapın.");

                var technicians = await _technicianGroupReadRepository.GetWhere(t => t.GroupID == request.ID).ToListAsync(cancellationToken);
                _technicianGroupWriteRepository.RemoveRange(technicians);
                _groupWriteRepository.Remove(entity);
                await _groupWriteRepository.SaveAsync();
                return ResponseFactory.Success<DeleteTicketGroupCmdRes>("Grup silindi.");
            }
            catch (Exception ex) { return ResponseFactory.Error<DeleteTicketGroupCmdRes>(ex); }
        }

        #endregion

        #region Category / SubCategory

        public async Task<SaveTicketCategoryCmdRes> SaveTicketCategory(SaveTicketCategoryCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                TicketCategoryVM vm = request.Category;
                if (string.IsNullOrWhiteSpace(vm.Name))
                    return ResponseFactory.Fail<SaveTicketCategoryCmdRes>("Kategori adı zorunludur.");

                string name = vm.Name.Trim();
                if (await _categoryReadRepository.GetWhere(s => s.ID != vm.ID && s.Name == name).AnyAsync(cancellationToken))
                    return ResponseFactory.Fail<SaveTicketCategoryCmdRes>("Bu adla bir kategori zaten var.");

                TicketCategoryEnt? entity = vm.ID > 0 ? await _categoryReadRepository.GetByIDAsync(vm.ID) : new TicketCategoryEnt();
                if (entity == null) return ResponseFactory.Fail<SaveTicketCategoryCmdRes>("Kategori bulunamadı.");

                entity.Name = name;
                entity.IsActive = vm.IsActive;

                if (entity.ID == 0) await _categoryWriteRepository.AddAsync(entity);
                await _categoryWriteRepository.SaveAsync();
                return ResponseFactory.Success<SaveTicketCategoryCmdRes>("Kategori kaydedildi.", r => r.Id = entity.ID);
            }
            catch (Exception ex) { return ResponseFactory.Error<SaveTicketCategoryCmdRes>(ex); }
        }

        public async Task<DeleteTicketCategoryCmdRes> DeleteTicketCategory(DeleteTicketCategoryCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _categoryReadRepository.GetByIDAsync(request.ID);
                if (entity == null) return ResponseFactory.Fail<DeleteTicketCategoryCmdRes>("Kategori bulunamadı.");
                if (await _ticketReadRepository.GetWhere(t => t.CategoryID == request.ID).AnyAsync(cancellationToken))
                    return ResponseFactory.Fail<DeleteTicketCategoryCmdRes>("Bu kategoride talepler var; silmek yerine pasif yapın.");

                var subs = await _subCategoryReadRepository.GetWhere(s => s.CategoryID == request.ID).ToListAsync(cancellationToken);
                _subCategoryWriteRepository.RemoveRange(subs);
                _categoryWriteRepository.Remove(entity);
                await _categoryWriteRepository.SaveAsync();
                return ResponseFactory.Success<DeleteTicketCategoryCmdRes>("Kategori silindi.");
            }
            catch (Exception ex) { return ResponseFactory.Error<DeleteTicketCategoryCmdRes>(ex); }
        }

        public async Task<SaveTicketSubCategoryCmdRes> SaveTicketSubCategory(SaveTicketSubCategoryCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                TicketSubCategoryVM vm = request.SubCategory;
                if (string.IsNullOrWhiteSpace(vm.Name))
                    return ResponseFactory.Fail<SaveTicketSubCategoryCmdRes>("Alt kategori adı zorunludur.");
                if (!await _categoryReadRepository.GetWhere(c => c.ID == vm.CategoryID).AnyAsync(cancellationToken))
                    return ResponseFactory.Fail<SaveTicketSubCategoryCmdRes>("Kategori bulunamadı.");

                string name = vm.Name.Trim();
                if (await _subCategoryReadRepository.GetWhere(s => s.ID != vm.ID && s.CategoryID == vm.CategoryID && s.Name == name).AnyAsync(cancellationToken))
                    return ResponseFactory.Fail<SaveTicketSubCategoryCmdRes>("Bu kategoride aynı adla bir alt kategori zaten var.");

                TicketSubCategoryEnt? entity = vm.ID > 0 ? await _subCategoryReadRepository.GetByIDAsync(vm.ID) : new TicketSubCategoryEnt();
                if (entity == null) return ResponseFactory.Fail<SaveTicketSubCategoryCmdRes>("Alt kategori bulunamadı.");

                entity.CategoryID = vm.CategoryID;
                entity.Name = name;
                entity.IsActive = vm.IsActive;

                if (entity.ID == 0) await _subCategoryWriteRepository.AddAsync(entity);
                await _subCategoryWriteRepository.SaveAsync();
                return ResponseFactory.Success<SaveTicketSubCategoryCmdRes>("Alt kategori kaydedildi.", r => r.Id = entity.ID);
            }
            catch (Exception ex) { return ResponseFactory.Error<SaveTicketSubCategoryCmdRes>(ex); }
        }

        public async Task<DeleteTicketSubCategoryCmdRes> DeleteTicketSubCategory(DeleteTicketSubCategoryCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _subCategoryReadRepository.GetByIDAsync(request.ID);
                if (entity == null) return ResponseFactory.Fail<DeleteTicketSubCategoryCmdRes>("Alt kategori bulunamadı.");
                if (await _ticketReadRepository.GetWhere(t => t.SubCategoryID == request.ID).AnyAsync(cancellationToken))
                    return ResponseFactory.Fail<DeleteTicketSubCategoryCmdRes>("Bu alt kategoride talepler var; silmek yerine pasif yapın.");

                _subCategoryWriteRepository.Remove(entity);
                await _subCategoryWriteRepository.SaveAsync();
                return ResponseFactory.Success<DeleteTicketSubCategoryCmdRes>("Alt kategori silindi.");
            }
            catch (Exception ex) { return ResponseFactory.Error<DeleteTicketSubCategoryCmdRes>(ex); }
        }

        #endregion

        public async Task<SaveBideskParameterCmdRes> SaveBideskParameter(SaveBideskParameterCmdReq request, CancellationToken cancellationToken)
        {
            try
            {
                string name = request.Parameter.ParamName?.Trim() ?? string.Empty;
                if (name == AutoCloseDaysParam && (!int.TryParse(request.Parameter.ParamValue, out int days) || days < 0))
                    return ResponseFactory.Fail<SaveBideskParameterCmdRes>("Otomatik kapanış günü 0 veya pozitif bir sayı olmalı.");

                var entity = await _parameterReadRepository.GetSingleAsync(p => p.ParamName == name);
                if (entity == null) return ResponseFactory.Fail<SaveBideskParameterCmdRes>("Parametre bulunamadı.");

                entity.ParamValue = request.Parameter.ParamValue?.Trim();
                await _parameterWriteRepository.SaveAsync();
                return ResponseFactory.Success<SaveBideskParameterCmdRes>("Parametre kaydedildi.");
            }
            catch (Exception ex) { return ResponseFactory.Error<SaveBideskParameterCmdRes>(ex); }
        }

        #region Helpers

        private IQueryable<TicketStatusVM> QueryStatuses(bool onlyActive)
            => _statusReadRepository.GetWhere(s => !onlyActive || s.IsActive, tracking: false)
                .OrderBy(s => s.SortOrder).ThenBy(s => s.Name)
                .Select(s => new TicketStatusVM
                {
                    ID = s.ID, Name = s.Name, Severity = s.Severity, SortOrder = s.SortOrder,
                    IsDefault = s.IsDefault, IsResolved = s.IsResolved, IsClosed = s.IsClosed,
                    PausesSla = s.PausesSla, IsActive = s.IsActive
                });

        private IQueryable<TicketPriorityVM> QueryPriorities(bool onlyActive)
            => _priorityReadRepository.GetWhere(s => !onlyActive || s.IsActive, tracking: false)
                .OrderBy(s => s.SortOrder).ThenBy(s => s.Name)
                .Select(s => new TicketPriorityVM
                {
                    ID = s.ID, Name = s.Name, Severity = s.Severity, SortOrder = s.SortOrder,
                    IsDefault = s.IsDefault, ResolveHours = s.ResolveHours, IsActive = s.IsActive
                });

        private async Task<List<TicketGroupVM>> LoadGroupsAsync(bool onlyActive, CancellationToken cancellationToken)
            => await _groupReadRepository.GetWhere(g => !onlyActive || g.IsActive, tracking: false)
                .OrderBy(g => g.SortOrder).ThenBy(g => g.Name)
                .Select(g => new TicketGroupVM
                {
                    ID = g.ID, Name = g.Name, Email = g.Email, SortOrder = g.SortOrder, IsActive = g.IsActive,
                    Technicians = g.Technicians.Where(t => !t.IsCanceled)
                        .OrderBy(t => t.User.NameSurname)
                        .Select(t => new TechnicianVM { UserID = t.UserID, NameSurname = t.User.NameSurname })
                        .ToList()
                })
                .ToListAsync(cancellationToken);

        private async Task<List<TicketCategoryVM>> LoadCategoriesAsync(bool onlyActive, CancellationToken cancellationToken)
            => await _categoryReadRepository.GetWhere(c => !onlyActive || c.IsActive, tracking: false)
                .OrderBy(c => c.Name)
                .Select(c => new TicketCategoryVM
                {
                    ID = c.ID, Name = c.Name, IsActive = c.IsActive,
                    SubCategories = c.SubCategories.Where(s => !s.IsCanceled && (!onlyActive || s.IsActive))
                        .OrderBy(s => s.Name)
                        .Select(s => new TicketSubCategoryVM { ID = s.ID, CategoryID = s.CategoryID, Name = s.Name, IsActive = s.IsActive })
                        .ToList()
                })
                .ToListAsync(cancellationToken);

        #endregion
    }
}
