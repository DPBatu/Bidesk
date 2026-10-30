using Bidesk.Application.Abstraction.Services.Ticket;
using Bidesk.Persistence.Services.Ticket;
using Bidesk.Application.Repositories.Ticket;
using Bidesk.Persistence.Repositories.Ticket;
using Bidesk.Application.Repositories.TicketStatus;
using Bidesk.Persistence.Repositories.TicketStatus;
using Bidesk.Application.Repositories.TicketPriority;
using Bidesk.Persistence.Repositories.TicketPriority;
using Bidesk.Application.Repositories.TicketGroup;
using Bidesk.Persistence.Repositories.TicketGroup;
using Bidesk.Application.Repositories.TechnicianGroup;
using Bidesk.Persistence.Repositories.TechnicianGroup;
using Bidesk.Application.Repositories.TicketCategory;
using Bidesk.Persistence.Repositories.TicketCategory;
using Bidesk.Application.Repositories.TicketSubCategory;
using Bidesk.Persistence.Repositories.TicketSubCategory;
using Bidesk.Application.Repositories.TicketNotifyUser;
using Bidesk.Persistence.Repositories.TicketNotifyUser;
using Bidesk.Application.Repositories.TicketConversation;
using Bidesk.Persistence.Repositories.TicketConversation;
using Bidesk.Application.Repositories.TicketAttachment;
using Bidesk.Persistence.Repositories.TicketAttachment;
using Bidesk.Application.Repositories.BideskParameter;
using Bidesk.Persistence.Repositories.BideskParameter;
using Bidesk.Application.Abstraction.Services;
using Bidesk.Application.Abstraction.Services.Log;
using Bidesk.Application.Abstraction.Services.User;
using Bidesk.Application.Repositories.PortalAction;
using Bidesk.Application.Repositories.PortalParameter;
using Bidesk.Application.Repositories.Public.BusinessPartner;
using Bidesk.Application.Repositories.Public.Company;
using Bidesk.Application.Repositories.Public.DefectCode;
using Bidesk.Application.Repositories.Public.Department;
using Bidesk.Application.Repositories.Public.Endpoint;
using Bidesk.Application.Repositories.Public.ImpersonationActionLog;
using Bidesk.Application.Repositories.Public.Material;
using Bidesk.Application.Repositories.Public.Menu;
using Bidesk.Application.Repositories.Public.Notification;
using Bidesk.Application.Repositories.Public.NotificationUser;
using Bidesk.Application.Repositories.Public.Position;
using Bidesk.Application.Repositories.Public.ProductionLine;
using Bidesk.Application.Repositories.Public.ProductionOperation;
using Bidesk.Application.Repositories.Public.RoleRelation;
using Bidesk.Application.Repositories.Public.Route;
using Bidesk.Application.Repositories.Public.UserDepartment;
using Bidesk.Application.Repositories.Public.UserPosition;
using Bidesk.Application.Repositories.Public.WorkCenter;
using Bidesk.Domain.Entities.Public;
using Bidesk.Persistence.Context;
using Bidesk.Persistence.Repositories.PortalAction;
using Bidesk.Persistence.Repositories.PortalParameter;
using Bidesk.Persistence.Repositories.Public.BusinessPartner;
using Bidesk.Persistence.Repositories.Public.Company;
using Bidesk.Persistence.Repositories.Public.DefectCode;
using Bidesk.Persistence.Repositories.Public.Department;
using Bidesk.Persistence.Repositories.Public.Endpoint;
using Bidesk.Persistence.Repositories.Public.ImpersonationActionLog;
using Bidesk.Persistence.Repositories.Public.Material;
using Bidesk.Persistence.Repositories.Public.Menu;
using Bidesk.Persistence.Repositories.Public.Notification;
using Bidesk.Persistence.Repositories.Public.NotificationUser;
using Bidesk.Persistence.Repositories.Public.Position;
using Bidesk.Persistence.Repositories.Public.ProductionLine;
using Bidesk.Persistence.Repositories.Public.ProductionOperation;
using Bidesk.Persistence.Repositories.Public.RoleRelation;
using Bidesk.Persistence.Repositories.Public.Route;
using Bidesk.Persistence.Repositories.Public.UserDepartment;
using Bidesk.Persistence.Repositories.Public.UserPosition;
using Bidesk.Persistence.Repositories.Public.WorkCenter;
using Bidesk.Persistence.Services;
using Bidesk.Persistence.Services.Log;
using Bidesk.Persistence.Services.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Bidesk.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {

#if (DEBUG)
            services.AddDbContext<BaseContext, DbDevContext>();
#elif (RELEASE)
            services.AddDbContext<BaseContext, DbProdContext>();
#endif

            services.AddIdentity<AppUserEnt, AppRoleEnt>()
                .AddEntityFrameworkStores<BaseContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IBusinessPartnerReadRepository, BusinessPartnerReadRepository>();
            services.AddScoped<ICompanyReadRepository, CompanyReadRepository>();
            services.AddScoped<IDefectCodeReadRepository, DefectCodeReadRepository>();
            services.AddScoped<IDepartmentReadRepository, DepartmentReadRepository>();
            services.AddScoped<IEndpointReadRepository, EndpointReadRepository>();
            services.AddScoped<IMaterialReadRepository, MaterialReadRepository>();
            services.AddScoped<IMenuReadRepository, MenuReadRepository>();
            services.AddScoped<IPositionReadRepository, PositionReadRepository>();
            services.AddScoped<IProductionLineReadRepository, ProductionLineReadRepository>();
            services.AddScoped<IProductionOperationReadRepository, ProductionOperationReadRepository>();
            services.AddScoped<IRoleRelationReadRepository, RoleRelationReadRepository>();
            services.AddScoped<IRouteReadRepository, RouteReadRepository>();
            services.AddScoped<IUserDepartmentReadRepository, UserDepartmentReadRepository>();
            services.AddScoped<IUserPositionReadRepository, UserPositionReadRepository>();
            services.AddScoped<IWorkCenterReadRepository, WorkCenterReadRepository>();

            services.AddScoped<IContextService, ContextService>();
            services.AddScoped<IEndpointService, EndpointService>();
            services.AddScoped<IErrorLogService, ErrorLogService>();
            services.AddScoped<BideskLogService>();
            services.AddScoped<TicketNotifier>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<ITicketDefinitionService, TicketDefinitionService>();
            services.AddScoped<ITicketJobService, TicketJobService>();
            services.AddScoped<ITicketReadRepository, TicketReadRepository>();
            services.AddScoped<ITicketWriteRepository, TicketWriteRepository>();
            services.AddScoped<ITicketStatusReadRepository, TicketStatusReadRepository>();
            services.AddScoped<ITicketStatusWriteRepository, TicketStatusWriteRepository>();
            services.AddScoped<ITicketPriorityReadRepository, TicketPriorityReadRepository>();
            services.AddScoped<ITicketPriorityWriteRepository, TicketPriorityWriteRepository>();
            services.AddScoped<ITicketGroupReadRepository, TicketGroupReadRepository>();
            services.AddScoped<ITicketGroupWriteRepository, TicketGroupWriteRepository>();
            services.AddScoped<ITechnicianGroupReadRepository, TechnicianGroupReadRepository>();
            services.AddScoped<ITechnicianGroupWriteRepository, TechnicianGroupWriteRepository>();
            services.AddScoped<ITicketCategoryReadRepository, TicketCategoryReadRepository>();
            services.AddScoped<ITicketCategoryWriteRepository, TicketCategoryWriteRepository>();
            services.AddScoped<ITicketSubCategoryReadRepository, TicketSubCategoryReadRepository>();
            services.AddScoped<ITicketSubCategoryWriteRepository, TicketSubCategoryWriteRepository>();
            services.AddScoped<ITicketNotifyUserReadRepository, TicketNotifyUserReadRepository>();
            services.AddScoped<ITicketNotifyUserWriteRepository, TicketNotifyUserWriteRepository>();
            services.AddScoped<ITicketConversationReadRepository, TicketConversationReadRepository>();
            services.AddScoped<ITicketConversationWriteRepository, TicketConversationWriteRepository>();
            services.AddScoped<ITicketAttachmentReadRepository, TicketAttachmentReadRepository>();
            services.AddScoped<ITicketAttachmentWriteRepository, TicketAttachmentWriteRepository>();
            services.AddScoped<IBideskParameterReadRepository, BideskParameterReadRepository>();
            services.AddScoped<IBideskParameterWriteRepository, BideskParameterWriteRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationReadRepository, NotificationReadRepository>();
            services.AddScoped<INotificationWriteRepository, NotificationWriteRepository>();
            services.AddScoped<INotificationUserReadRepository, NotificationUserReadRepository>();
            services.AddScoped<INotificationUserWriteRepository, NotificationUserWriteRepository>();


            services.AddScoped<IUnitOfWorkService<BaseContext>, UnitOfWorkService<BaseContext>>();


            services.AddScoped<IImpersonationActionLogWriteRepository, ImpersonationActionLogWriteRepository>();
            services.AddScoped<IPortalActionReadRepository, PortalActionReadRepository>();
            services.AddScoped<IPortalActionWriteRepository, PortalActionWriteRepository>();
            services.AddScoped<IPortalParameterReadRepository, PortalParameterReadRepository>();

        }
    }
}
