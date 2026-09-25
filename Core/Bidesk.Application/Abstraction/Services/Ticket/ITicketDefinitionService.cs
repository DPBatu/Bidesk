using Bidesk.Application.Features.Queries.TicketDefinition.GetTicketLookups;
using Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketStatuses;
using Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketPriorities;
using Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketGroups;
using Bidesk.Application.Features.Queries.TicketDefinition.GetAllTicketCategories;
using Bidesk.Application.Features.Queries.TicketDefinition.GetBideskParameters;
using Bidesk.Application.Features.Queries.TicketDefinition.GetBideskPersonnel;
using Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketStatus;
using Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketStatus;
using Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketPriority;
using Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketPriority;
using Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketGroup;
using Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketGroup;
using Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketCategory;
using Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketCategory;
using Bidesk.Application.Features.Commands.TicketDefinition.SaveTicketSubCategory;
using Bidesk.Application.Features.Commands.TicketDefinition.DeleteTicketSubCategory;
using Bidesk.Application.Features.Commands.TicketDefinition.SaveBideskParameter;

namespace Bidesk.Application.Abstraction.Services.Ticket
{
    /// <summary>Durum, oncelik, grup/teknisyen, kategori ve parametre tanimlari.</summary>
    public interface ITicketDefinitionService
    {
        Task<GetTicketLookupsQRes> GetTicketLookups(GetTicketLookupsQReq request, CancellationToken cancellationToken);
        Task<GetAllTicketStatusesQRes> GetAllTicketStatuses(GetAllTicketStatusesQReq request, CancellationToken cancellationToken);
        Task<GetAllTicketPrioritiesQRes> GetAllTicketPriorities(GetAllTicketPrioritiesQReq request, CancellationToken cancellationToken);
        Task<GetAllTicketGroupsQRes> GetAllTicketGroups(GetAllTicketGroupsQReq request, CancellationToken cancellationToken);
        Task<GetAllTicketCategoriesQRes> GetAllTicketCategories(GetAllTicketCategoriesQReq request, CancellationToken cancellationToken);
        Task<GetBideskParametersQRes> GetBideskParameters(GetBideskParametersQReq request, CancellationToken cancellationToken);
        Task<GetBideskPersonnelQRes> GetBideskPersonnel(GetBideskPersonnelQReq request, CancellationToken cancellationToken);
        Task<SaveTicketStatusCmdRes> SaveTicketStatus(SaveTicketStatusCmdReq request, CancellationToken cancellationToken);
        Task<DeleteTicketStatusCmdRes> DeleteTicketStatus(DeleteTicketStatusCmdReq request, CancellationToken cancellationToken);
        Task<SaveTicketPriorityCmdRes> SaveTicketPriority(SaveTicketPriorityCmdReq request, CancellationToken cancellationToken);
        Task<DeleteTicketPriorityCmdRes> DeleteTicketPriority(DeleteTicketPriorityCmdReq request, CancellationToken cancellationToken);
        Task<SaveTicketGroupCmdRes> SaveTicketGroup(SaveTicketGroupCmdReq request, CancellationToken cancellationToken);
        Task<DeleteTicketGroupCmdRes> DeleteTicketGroup(DeleteTicketGroupCmdReq request, CancellationToken cancellationToken);
        Task<SaveTicketCategoryCmdRes> SaveTicketCategory(SaveTicketCategoryCmdReq request, CancellationToken cancellationToken);
        Task<DeleteTicketCategoryCmdRes> DeleteTicketCategory(DeleteTicketCategoryCmdReq request, CancellationToken cancellationToken);
        Task<SaveTicketSubCategoryCmdRes> SaveTicketSubCategory(SaveTicketSubCategoryCmdReq request, CancellationToken cancellationToken);
        Task<DeleteTicketSubCategoryCmdRes> DeleteTicketSubCategory(DeleteTicketSubCategoryCmdReq request, CancellationToken cancellationToken);
        Task<SaveBideskParameterCmdRes> SaveBideskParameter(SaveBideskParameterCmdReq request, CancellationToken cancellationToken);
    }
}
