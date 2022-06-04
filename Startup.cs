using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyCourse.Models.Services.Application;
using MyCourse.Models.Services.Infrastructure;

namespace MyCourse
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();            
            services.AddMvc(options => options.EnableEndpointRouting = false);
            
            //Il codice sotto riportato serve per far funzionare il costruttore presente nella classe CoursesController presente nella dir Controllers
            services.AddTransient<ICourseService, AdoNetCourseService>();
            services.AddTransient<IDatabaseAccessor, SqliteDatabaseAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                string filePath = Path.Combine(env.ContentRootPath, "bin/reload.txt");
                File.WriteAllText(filePath, DateTime.Now.ToString());


            }

            app.UseStaticFiles();

            app.UseRouting();

            //app.UseDeveloperExceptionPage();

            app.UseMvc(routeBuilder => 
            {
                /* id? sta ad indicare che è il parametro è opzionale */
                routeBuilder.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

           /* app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {                                       
                    await context.Response.WriteAsync("Hello World!");
                });
            });*/
        }
    }
}
