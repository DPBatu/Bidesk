using Bidesk.Application.Constants;
using Bidesk.Domain.Entities.Public;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bidesk.Persistence.Context.SchemaExtension
{
    public static class PublicSchemaExtension
    {
        public static string SchemaName = "public";
        public static void AddPublicSchemaExtension(this ModelBuilder builder)
        {
            //Public Schema Entitylerine erişmek için context'e ekleme yapıldı.
            //Migration dışında tutulduğundan emin olmak için ToTable metodunda ExcludeFromMigrations kullanıldı.
            //Migration işlemi olmadığı için ilişkilerin manuel olarak belirtilmesi gerekiyor.

            builder.Entity<IdentityRoleClaim<long>>().ToTable("AspNetRoleClaims", SchemaName, t => t.ExcludeFromMigrations());
            builder.Entity<IdentityUserClaim<long>>().ToTable("AspNetUserClaims", SchemaName, t => t.ExcludeFromMigrations());
            builder.Entity<IdentityUserLogin<long>>().ToTable("AspNetUserLogins", SchemaName, t => t.ExcludeFromMigrations());
            builder.Entity<IdentityUserRole<long>>().ToTable("AspNetUserRoles", SchemaName, t => t.ExcludeFromMigrations());
            builder.Entity<IdentityUserToken<long>>().ToTable("AspNetUserTokens", SchemaName, t => t.ExcludeFromMigrations());

            builder.Entity<AppRoleEnt>()
                .HasMany(r => r.Routes)
                .WithMany(e => e.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "AppRoleEntRouteEnt",
                    j => j
                        .HasOne<RouteEnt>()
                        .WithMany()
                        .HasForeignKey("RoutesID")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<AppRoleEnt>()
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.ToTable("AppRoleEntRouteEnt", "public", t => t.ExcludeFromMigrations());
                        j.HasKey("RolesId", "RoutesID");
                    });

            builder.Entity<AppRoleEnt>(roles =>
            {
                roles.HasKey(r => r.Id).HasName("PK_AspNetRoles");

                roles.ToTable(nameof(BaseContext.AspNetRoles), SchemaName, t => t.ExcludeFromMigrations());
            });

            builder.Entity<AppRoleEnt>()
                .HasMany(r => r.Endpoints)
                .WithMany(e => e.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "AppRoleEntEndpointEnt",
                    j => j
                        .HasOne<EndpointEnt>()
                        .WithMany()
                        .HasForeignKey("EndpointsID")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<AppRoleEnt>()
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.ToTable("AppRoleEntEndpointEnt", "public", t => t.ExcludeFromMigrations());
                        j.HasKey("EndpointsID", "RolesId");
                    });


            //builder.Entity<AppRoleEnt>(roles =>
            //{
            //    roles.HasKey(r => r.Id).HasName("PK_AspNetRoles");

            //    roles.HasMany(r => r.Endpoints)
            //    .WithMany(e => e.Roles);

            //    roles.HasMany(r => r.Routes)
            //    .WithMany(e => e.Roles);

            //    roles.ToTable(nameof(BaseContext.AspNetRoles), SchemaName, t => t.ExcludeFromMigrations());
            //});

            builder.Entity<AppUserEnt>(
                user =>
                {
                    user.HasKey(u => u.Id).HasName("PK_AspNetRoles");

                    user.HasOne(u => u.Department)
                    .WithMany(d => d.Employees);

                    user.HasMany(u => u.DepartmentLogs)
                    .WithOne(dl => dl.User);

                    user.HasOne(u => u.Position)
                     .WithMany(p => p.Employees);

                    user.HasMany(u => u.PositionLogs)
                    .WithOne(pl => pl.User);

                    user.HasMany(u => u.ResponsibleDepartment)
                    .WithOne(d => d.Responsible);

                    user.ToTable(nameof(BaseContext.AspNetUsers), SchemaName, t => t.ExcludeFromMigrations());
                }
            );

            builder.Entity<EndpointEnt>(e =>
            {
                e.HasMany(e => e.Roles)
                .WithMany(r => r.Endpoints);

                e.HasOne(e => e.Menu)
                .WithMany(m => m.Endpoints);

                e.ToTable(nameof(BaseContext.Endpoints), SchemaName, t => t.ExcludeFromMigrations());
            });


            builder.Entity<RouteEnt>(r =>
            {
                r.HasMany(r => r.Roles)
                .WithMany(e => e.Routes);

                r.ToTable(nameof(BaseContext.Routes), SchemaName, t => t.ExcludeFromMigrations());
            });

            builder.Entity<BusinessPartnerEnt>().ToTable(nameof(BaseContext.BusinessPartners), SchemaName, t => t.ExcludeFromMigrations());
            builder.Entity<CompanyEnt>().ToTable(nameof(BaseContext.Companies), SchemaName, t => t.ExcludeFromMigrations());
            builder.Entity<DefectCodeEnt>().ToTable(nameof(BaseContext.DefectCodes), SchemaName, t => t.ExcludeFromMigrations());
            builder.Entity<DepartmentEnt>().ToTable(nameof(BaseContext.Departments), SchemaName, t => t.ExcludeFromMigrations());
            builder.Entity<MaterialEnt>().ToTable(nameof(BaseContext.Materials), SchemaName, t => t.ExcludeFromMigrations());
            builder.Entity<MenuEnt>().ToTable(nameof(BaseContext.Menu), SchemaName, t => t.ExcludeFromMigrations());

            builder.Entity<PositionEnt>(p =>
            {
                p.HasOne(p => p.CanceledUser);

                p.ToTable(nameof(BaseContext.Positions), SchemaName, t => t.ExcludeFromMigrations());
            });

            builder.Entity<ProductionLineEnt>().ToTable(nameof(BaseContext.ProductionLines), SchemaName, t => t.ExcludeFromMigrations());
            builder.Entity<ProductionOperationEnt>().ToTable(nameof(BaseContext.ProductionOperations), SchemaName, t => t.ExcludeFromMigrations());
            builder.Entity<RoleRelationEnt>().ToTable(nameof(BaseContext.RoleRelations), SchemaName, t => t.ExcludeFromMigrations());

            builder.Entity<UserDepartmentEnt>(u =>
            {
                u.HasOne(u => u.CanceledUser);

                u.ToTable(nameof(BaseContext.UserDepartmentLogs), SchemaName, t => t.ExcludeFromMigrations());
            });
            builder.Entity<UserPositionEnt>(ul =>
            {
                ul.HasOne(ul => ul.CanceledUser);

                ul.ToTable(nameof(BaseContext.UserPositionLogs), SchemaName, t => t.ExcludeFromMigrations());
            });

            builder.Entity<WorkCenterEnt>().ToTable(nameof(BaseContext.WorkCenters), SchemaName, t => t.ExcludeFromMigrations());

            builder.Entity<NotificationEnt>(n =>
            {
                n.ToTable(nameof(BaseContext.Notifications), SchemaName, t => t.ExcludeFromMigrations());
            });

            builder.Entity<NotificationUserEnt>(nu =>
            {
                nu.ToTable(nameof(BaseContext.NotificationUsers), SchemaName, t => t.ExcludeFromMigrations());
            });

            // Merkezi aksiyon tablosu Portal'a aittir; Kaizen buradan yalnizca OKUR.
            // Yazma her zaman api/portal/Action uclarindan gecer.
            builder.Entity<ActionEnt>(a =>
            {
                // Tablo adi ELLE veriliyor: DbSet ozelligi PortalActions ama Portal'daki
                // fiziksel tablo "Actions". nameof(...) burada yanlis adi uretirdi.
                a.ToTable("Actions", SchemaName, t => t.ExcludeFromMigrations());

                // BIDESK aksiyon kaydi acmiyor; tablo yalnizca Portal ile ayni modeli paylasmak icin esleniyor.

                // Sorumlu ve atayan kullanici — bu FK'ler veritabaninda GERCEKTEN var
                // (public.Actions -> public.AspNetUsers, ikisi de Portal'in semasinda),
                // burada yalnizca EF modeline tanitiliyor.
                a.HasOne(x => x.ResponsibleUser).WithMany().HasForeignKey(x => x.ResponsibleUserID)
                    .OnDelete(DeleteBehavior.NoAction);
                a.HasOne(x => x.AssignerUser).WithMany().HasForeignKey(x => x.AssignerUserID)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Taklit (impersonation) islem kaydi Portal'a aittir; Kaizen buraya yalnizca yazar.
            // Portal genel ayarlari; Kaizen yalnizca OKUR.
            builder.Entity<PortalParameterEnt>(pp =>
            {
                pp.ToTable("PortalParameters", SchemaName, t => t.ExcludeFromMigrations());
            });

            builder.Entity<ImpersonationActionLogEnt>(ia =>
            {
                ia.ToTable(nameof(BaseContext.ImpersonationActionLogs), SchemaName, t => t.ExcludeFromMigrations());
            });
        }
    }
}
