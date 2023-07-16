using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Entities.OrderAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class StoreContext : IdentityDbContext<User,Role,int> // that means all of our identity classes are going to use an integer as their ID
    {
        public StoreContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
            .HasOne(a => a.Address)
            .WithOne()
            .HasForeignKey<UserAddress>(a => a.ID)
            .OnDelete(DeleteBehavior.Cascade);
            // ve kullanıcı adresimizin silinmesini istediğimiz için davranışı sil diyeceğiz. eğer bir kullanıcı varlığını silersek

            builder.Entity<Role>()
                .HasData(
                    new Role{Id = 1,  Name = "Member",NormalizedName = "MEMBER"},
                    new Role{Id = 2,Name = "Admin",NormalizedName = "ADMIN"}
                );
        }
    }
}