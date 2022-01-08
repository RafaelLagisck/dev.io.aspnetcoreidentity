using AspNetCoreIdentity.Config;
using AspNetCoreIdentity.Extensions;
using KissLog.AspNetCore;

namespace AspNetCoreIdentity
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile(path:"appsettings.json",optional:true,reloadOnChange:true)
                .AddJsonFile(path:$"appsettings.{hostEnvironment.EnvironmentName}.json", optional:true, reloadOnChange:true)
                .AddEnvironmentVariables();

            if(hostEnvironment.IsProduction())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityConfig(Configuration);

            services.AddAuthorizationConfig(); 

            services.ResolveDependencies();

            services.AddControllersWithViews();

            services.AddMvc().AddMvcOptions(options =>
            {
                options.Filters.Add(typeof(AuditoriaFilter));
            });
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else 
            {
                app.UseExceptionHandler("/erro/500");
                app.UseStatusCodePagesWithRedirects("/erro/{0}");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseKissLogMiddleware(options => {
                LogConfig.ConfigureKissLog(options, Configuration);
            });


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();    
        }
    }
}
