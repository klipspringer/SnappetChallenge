using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SnappetChallenge.Mappers;
using SnappetChallenge.Mappers.Interfaces;
using SnappetChallenge.Models;
using SnappetChallenge.Queries;
using SnappetChallenge.Queries.Handlers;
using SnappetChallenge.Queries.Interfaces;
using SnappetChallenge.Queries.Responses;
using SnappetChallenge.Repository;
using SnappetChallenge.Repository.Config;
using SnappetChallenge.Repository.DataLoader;
using SnappetChallenge.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnappetChallenge
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
            services.Configure<RepositoryOptions>(Configuration.GetSection(RepositoryOptions.KeyName));

            // Repository DI
            services.AddScoped<IFileDataLoader, JsonFileDataLoader>();
            services.AddScoped<IRepository, JsonRepository>();

            // Queries DI
            services.AddScoped<IQueryHandler<GetEducatorTeachingOverviewQuery, Task<IEnumerable<EducatorTeachingOverviewResponse>>>, 
                GetEducatorTeachingOverviewQueryHandler>();
            services.AddScoped<IQueryHandler<GetEducatorSubjectOverviewQuery, Task<IEnumerable<EducatorSubjectOverviewResponse>>>,
                GetEducatorSubjectOverviewQueryHandler>();

            // Mappers DI
            services.AddScoped<IMapper<EducatorTeachingOverviewResponse, SubjectOverviewDto>, SubjectOverviewMapper>();
            services.AddScoped<IMapper<EducatorSubjectOverviewResponse, SubjectStudentOverviewDto>, SubjectStudentOverviewMapper>();
            
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddLazyCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
