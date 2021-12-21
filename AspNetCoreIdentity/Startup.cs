using AspNetCoreIdentity.Areas.Identity.Data;
using AspNetCoreIdentity.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentity
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AspNetCoreIdentityContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AspNetCoreIdentityContextConnection")));

            //Roles
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<AspNetCoreIdentityContext>();

            //Claims
            services.AddAuthorization(options =>
            {
                options.AddPolicy(name: "PodeExcluir", configurePolicy: policy => policy.RequireClaim("PodeExcluir"));

                options.AddPolicy(name: "PodeLer", configurePolicy: policy => policy.Requirements.Add(new PermissaoNecessaria("PodeLer")));

                options.AddPolicy(name: "PodeEscrever", configurePolicy: policy => policy.Requirements.Add(new PermissaoNecessaria("PodeEscrever")));

            });

            services.AddSingleton<IAuthorizationHandler, PermissaoNecessariaHandler>();


            services.AddControllersWithViews();
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();    
        }
    }
}
