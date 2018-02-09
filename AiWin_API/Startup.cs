using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AiWin_API
{
    public class Startup
    {
        //public static string ConnectionString { get; set; }
        //public static string SubCode { get; set; }
        
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
               // .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            #region 跨域
            var urls = Config.GetAppSetting("Cores").Split(',');
            services.AddCors(options =>
                    options.AddPolicy("AllowSameDomain",
                        builder => builder.WithOrigins(urls).AllowAnyMethod().AllowAnyHeader().AllowCredentials())//.AllowAnyOrigin()
            );
            #endregion
            // Add framework services.
            services.AddMvc();
            services.AddOptions();
            //ConnectionString = Configuration["AppSettings:DefaultConnection"];
            //SubCode= Configuration["AppSettings:SubCode"];
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                EnableDirectoryBrowsing = true

            };
            options.StaticFileOptions.ServeUnknownFileTypes = true;
            options.DefaultFilesOptions.DefaultFileNames = new[] { "/wwwroot/index.html" }; //put whatever default pages you like here
            app.UseFileServer(options);

            app.UseStaticFiles();
            app.UseMvc();
            app.UseCors("AllowSameDomain");
        }
    }
}
