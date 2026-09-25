using Bidesk.Application.Abstraction.Services;
using Bidesk.Application.Abstraction.Services.Log;
using Bidesk.Application.Exceptions;
using Bidesk.Domain.AuditLog;
using Bidesk.Domain.CustomAttributes.Log;
using Bidesk.Domain.Entities;
using Bidesk.Domain.Entities.Public;
using Bidesk.Domain.Entities.Ticket;
using Bidesk.Persistence.Context.SchemaExtension;
using Bidesk.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bidesk.Persistence.Context
{
    public class BaseContext : IdentityDbContext<AppUserEnt, AppRoleEnt, long>
    {
        private readonly IServiceProvider _serviceProvider;
        public long CompanyId
        {
            get
            {
                ContextService? contextService = _serviceProvider.GetService(typeof(IContextService)) as ContextService;
                if (contextService == null) throw new BaseException("Context bilgisi alınamadı!", "Context Servisine Erişilemedi!");
                return contextService.GetCurrentCompanyId();
            }
        }

        public BaseContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public DbSet<AppRoleEnt> AspNetRoles { get; set; }
        public DbSet<AppUserEnt> AspNetUsers { get; set; }
        public DbSet<BusinessPartnerEnt> BusinessPartners { get; set; }
        public DbSet<CompanyEnt> Companies { get; set; }
        public DbSet<DefectCodeEnt> DefectCodes { get; set; }
        public DbSet<DepartmentEnt> Departments { get; set; }
        public DbSet<EndpointEnt> Endpoints { get; set; }
        public DbSet<MaterialEnt> Materials { get; set; }
        public DbSet<MenuEnt> Menu { get; set; }
        public DbSet<PositionEnt> Positions { get; set; }
        public DbSet<ProductionLineEnt> ProductionLines { get; set; }
        public DbSet<ProductionOperationEnt> ProductionOperations { get; set; }
        public DbSet<RoleRelationEnt> RoleRelations { get; set; }
        public DbSet<RouteEnt> Routes { get; set; }
        public DbSet<UserDepartmentEnt> UserDepartmentLogs { get; set; }
        public DbSet<UserPositionEnt> UserPositionLogs { get; set; }
        public DbSet<WorkCenterEnt> WorkCenters { get; set; }
        //Public (paylaşılan Portal tabloları — migration dışı)
        public DbSet<NotificationEnt> Notifications { get; set; }

        /// <summary>
        /// Merkezi aksiyon tablosunun (public.Actions) salt-okunur aynasi. Ozellik adi
        /// "PortalActions"; tablo adi ToTable ile "Actions" olarak veriliyor
        /// (bkz. PublicSchemaExtension) — Kaizen'in kendi KaizenActions tablosuyla
        /// karismasin diye.
        /// </summary>
        public DbSet<ActionEnt> PortalActions { get; set; }

        /// <summary>
        /// Portal'ın tek satırlık genel ayar kaydının aynası. Kaizen'in KENDİ parametre
        /// tablosuyla karıştırılmasın diye özellik adı "PortalParameters".
        /// </summary>
        public DbSet<PortalParameterEnt> PortalParameters { get; set; }
        public DbSet<NotificationUserEnt> NotificationUsers { get; set; }
        public DbSet<ImpersonationActionLogEnt> ImpersonationActionLogs { get; set; }

        //Bidesk
        public DbSet<AuditLogEnt> AuditLogs { get; set; }
        public DbSet<TicketEnt> Tickets { get; set; }
        public DbSet<TicketStatusEnt> TicketStatuses { get; set; }
        public DbSet<TicketPriorityEnt> TicketPriorities { get; set; }
        public DbSet<TicketGroupEnt> TicketGroups { get; set; }
        public DbSet<TechnicianGroupEnt> TechnicianGroups { get; set; }
        public DbSet<TicketCategoryEnt> TicketCategories { get; set; }
        public DbSet<TicketSubCategoryEnt> TicketSubCategories { get; set; }
        public DbSet<TicketNotifyUserEnt> TicketNotifyUsers { get; set; }
        public DbSet<TicketConversationEnt> TicketConversations { get; set; }
        public DbSet<TicketAttachmentEnt> TicketAttachments { get; set; }
        public DbSet<BideskParameterEnt> BideskParameters { get; set; }

        private string SchemaName = "Bidesk";
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddPublicSchemaExtension();

            modelBuilder.Entity<AuditLogEnt>().ToTable(nameof(AuditLogs), SchemaName);
            modelBuilder.Entity<TicketStatusEnt>().ToTable(nameof(TicketStatuses), SchemaName);
            modelBuilder.Entity<TicketPriorityEnt>().ToTable(nameof(TicketPriorities), SchemaName);
            modelBuilder.Entity<TicketGroupEnt>().ToTable(nameof(TicketGroups), SchemaName);
            modelBuilder.Entity<TicketCategoryEnt>().ToTable(nameof(TicketCategories), SchemaName);
            modelBuilder.Entity<BideskParameterEnt>().ToTable(nameof(BideskParameters), SchemaName);

            modelBuilder.Entity<TicketSubCategoryEnt>(entity =>
            {
                entity.ToTable(nameof(TicketSubCategories), SchemaName);
                entity.HasOne(e => e.Category).WithMany(c => c.SubCategories)
                      .HasForeignKey(e => e.CategoryID).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TechnicianGroupEnt>(entity =>
            {
                entity.ToTable(nameof(TechnicianGroups), SchemaName);
                entity.HasIndex(e => new { e.CompanyID, e.GroupID, e.UserID })
                      .IsUnique()
                      .HasFilter("\"IsCanceled\" = false")
                      .HasDatabaseName("IX_TechnicianGroup_Group_User");
                entity.HasOne(e => e.Group).WithMany(g => g.Technicians)
                      .HasForeignKey(e => e.GroupID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.User).WithMany()
                      .HasForeignKey(e => e.UserID).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TicketEnt>(entity =>
            {
                entity.ToTable(nameof(Tickets), SchemaName);
                entity.HasIndex(e => new { e.CompanyID, e.IsCanceled, e.StatusID });
                entity.HasIndex(e => new { e.CompanyID, e.IsCanceled, e.RequesterUserID });
                entity.HasIndex(e => new { e.CompanyID, e.IsCanceled, e.TechnicianUserID });
                entity.HasIndex(e => new { e.CompanyID, e.IsCanceled, e.GroupID });

                entity.HasOne(e => e.RequesterUser).WithMany()
                      .HasForeignKey(e => e.RequesterUserID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.TechnicianUser).WithMany()
                      .HasForeignKey(e => e.TechnicianUserID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Status).WithMany()
                      .HasForeignKey(e => e.StatusID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Priority).WithMany()
                      .HasForeignKey(e => e.PriorityID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Group).WithMany()
                      .HasForeignKey(e => e.GroupID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Category).WithMany()
                      .HasForeignKey(e => e.CategoryID).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.SubCategory).WithMany()
                      .HasForeignKey(e => e.SubCategoryID).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TicketNotifyUserEnt>(entity =>
            {
                entity.ToTable(nameof(TicketNotifyUsers), SchemaName);
                entity.HasOne(e => e.Ticket).WithMany(t => t.NotifyUsers)
                      .HasForeignKey(e => e.TicketID).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.User).WithMany()
                      .HasForeignKey(e => e.UserID).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TicketConversationEnt>(entity =>
            {
                entity.ToTable(nameof(TicketConversations), SchemaName);
                entity.HasIndex(e => new { e.CompanyID, e.IsCanceled, e.TicketID });
                entity.HasOne(e => e.Ticket).WithMany(t => t.Conversations)
                      .HasForeignKey(e => e.TicketID).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TicketAttachmentEnt>(entity =>
            {
                entity.ToTable(nameof(TicketAttachments), SchemaName);
                entity.HasIndex(e => new { e.CompanyID, e.IsCanceled, e.TicketID });
                entity.HasOne(e => e.Ticket).WithMany(t => t.Attachments)
                      .HasForeignKey(e => e.TicketID).OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);

            //Identity ara tablolari BURADA Ignore EDILMEZ. AddPublicSchemaExtension bunlari zaten
            //public semasina esleyip ExcludeFromMigrations ile migration disinda tutuyor; ayrica
            //Ignore edilmeleri o eslemeyi geri aliyor ve UserManager.GetRolesAsync calisamiyordu
            //("Cannot create a DbSet for 'IdentityUserRole<long>'"), yani admin olmayan her
            //kullanicinin yetki kontrolu patliyordu. EMS ve Suggestion API'lerinde de bu blok yok.
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DateTime date = DateTime.Now;
            var datas = ChangeTracker.Entries<BaseEntity>();

            long companyId = 0;
            long userId = 0;

            ContextService? contextService = _serviceProvider.GetService(typeof(IContextService)) as ContextService;
            if (contextService == null) throw new BaseException("Context bilgisi alınamadı!", "Context Servisine Erişilemedi!");

            companyId = contextService.GetCurrentCompanyId();
            userId = contextService.GetCurrentUserId();

            foreach (var data in datas)
            {
                if (data.State == EntityState.Modified)
                {
                    await LogChangesAsync(date, userId);
                    if (!data.Entity.IsCanceled)
                    {
                        data.Entity.UpdatedDate = date;
                        data.Entity.UpdatedUserId = userId;
                    }
                    else
                    {
                        data.Entity.CanceledDate = date;
                        data.Entity.CanceledUserId = userId;
                    }
                }
                else if (data.State == EntityState.Added)
                {
                    data.Entity.CreatedDate = date;
                    data.Entity.CreatedUserId = userId;
                    data.Entity.CompanyID = companyId;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private async Task LogChangesAsync(DateTime _logDate, long _logUser)
        {
            var changes = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified);

            List<BaseAuditLog> audits = new List<BaseAuditLog>();

            foreach (var entry in changes)
            {
                //var entityType = entry.Entity.GetType();
                //var loggableAttribute = (LoggableAttribute)Attribute.GetCustomAttribute(entityType, typeof(LoggableAttribute));
                var entityType = entry.Entity.GetType().BaseType ?? entry.Entity.GetType();
                var loggableAttribute = entityType.GetCustomAttribute<LoggableAttribute>();

                if (loggableAttribute != null && loggableAttribute.IsLoggable)
                {
                    foreach (var prop in entry.Properties)
                    {
                        if (prop.OriginalValue?.ToString() != entry.CurrentValues[prop.Metadata.Name]?.ToString())
                        {
                            string dbContextName = GetType().Name;

                            var auditLog = new BaseAuditLog
                            {
                                TableName = entry.Metadata.GetTableName(),
                                FieldName = prop.Metadata.Name,
                                TableID = Convert.ToInt64(entry.CurrentValues["ID"]?.ToString()),
                                OldValue = entry.State == EntityState.Modified ? prop.OriginalValue?.ToString() : null,
                                NewValue = entry.CurrentValues[prop.Metadata.Name]?.ToString(),
                                CreatedDate = _logDate,
                                CreatedUserId = _logUser,
                            };
                            audits.Add(auditLog);
                        }

                    }
                    ILogServiceFactory logServiceFactory = _serviceProvider.GetService<ILogServiceFactory>();
                    ILogService logService = logServiceFactory.GetLogService(this);

                    if (audits.Any())
                    {
                        await logService.LogChangesAsync(this, audits);
                    }
                }


            }
        }
    }
}
