using Marquise_Web.Model.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Marquise_Web.Data;
using System.Web;

namespace Marquise_Web.Service.Service
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            return manager;
        }
    }

    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        // Constructor with ApplicationUserManager and IAuthenticationManager passed directly
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        // Factory method to create the ApplicationSignInManager
        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            var userManager = context.GetUserManager<ApplicationUserManager>();  // Get the userManager from OwinContext
            var authenticationManager = context.Authentication;  // Get the AuthenticationManager
            return new ApplicationSignInManager(userManager, authenticationManager);
        }
    }

}
