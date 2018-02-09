using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AiWinLinux
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 跨域
            var urls = Configuration["AppSettings:Cores"].Split(',');
            services.AddCors(options =>
                    options.AddPolicy("AllowSameDomain",
                        builder => builder.WithOrigins(urls).AllowAnyMethod().AllowAnyHeader().AllowCredentials())//.AllowAnyOrigin()
            );
            #endregion
            // Add framework services.
            services.AddOptions();
            services.Configure<AppSettingsModel>(Configuration);//.GetSection("AppSettings")
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
