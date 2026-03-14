using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Permission = Domain.Entities.Permission;

namespace Infrastructure.Persistence.DataContext;

internal static class DbInitializer
{
    public static void SeedData(this ModelBuilder modelBuilder)
    {
        //Seed Permission Data
        IEnumerable<Permission> permissions = Enum.GetValues<Authentication.Permission>()
            .Select(p => new Permission
            {
                Id = (int)p,
                Name = p.ToString()
            });

        modelBuilder.Entity<Permission>().HasData(permissions);

        //Seed Role Data
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Registered" },
            new Role { Id = 2, Name = "Administrator" }
            );


        //Seed RolePermission Data
        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission { RoleId = 1, PermissionId = (int)Authentication.Permission.ReadUser },
            new RolePermission { RoleId = 2, PermissionId = (int)Authentication.Permission.ReadUser },
            new RolePermission { RoleId = 2, PermissionId = (int)Authentication.Permission.UpdateUser }
            );


    }
}
