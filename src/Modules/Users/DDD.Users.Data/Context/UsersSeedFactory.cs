namespace DDD.Users.Data.Context
{
    using DDD.Infrastructure.Data;
    using DDD.Users.Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class UsersSeedFactory
    {
        private UserManager<ApplicationUser> _userManager;

        public UsersSeedFactory(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }
        public async Task Clean()
        {
            var users = this._userManager.Users;
            if (users.Any())
            {
                users.ToList().ForEach(async user => await this._userManager.DeleteAsync(user));
            }
        }

        public async Task Initialize()
        {
            await this.AddUsers();

            //await this._customerRepository.SaveChangesAsync();
        }

        private async Task AddUsers()
        {
            var users = new List<ApplicationUser>() {
                new ApplicationUser { UserName = "Bla", Email="bla@bla.com" },
            };

            users.ForEach(async user =>
            {
                var result = await _userManager.CreateAsync(user, "Blablabla5");
                if (!result.Succeeded) throw new System.ArgumentException(result.ToString());
                //if (result.Succeeded)
                //{
                //    await _userManager.AddClaimsAsync(user, new List<Claim> {
                //            new Claim("BindedId", model.BindedId.ToString()),
                //            new Claim(JwtClaimTypes.Email, model.Email),
                //        });
                //}
            });
        }
    }
}
