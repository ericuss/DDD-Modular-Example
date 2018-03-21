namespace DDD.Users.Data.Context
{
    using DDD.Users.Domain.Entities;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class UsersDbContext : IdentityDbContext<ApplicationUser>
    {

        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
