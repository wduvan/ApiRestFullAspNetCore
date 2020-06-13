using AutoMapper;
using LibraryAPI.DbContexts;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace LibraryAPI
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
            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(
                    new Microsoft.AspNetCore.Mvc.Formatters.XmlDataContractSerializerOutputFormatter());
            }).AddXmlDataContractSerializerFormatters();

            services.AddScoped<ICourseLibraryRepository, CourseLibraryRepository>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<CourseLibraryContext>(options =>
            {
                String dataBase = Environment.GetEnvironmentVariable("API_DB");
                String dataBasePassword = Environment.GetEnvironmentVariable("API_PASS_DB");
                String dataBaseUser = Environment.GetEnvironmentVariable("API_USER_DB");

                options.UseSqlServer(
                    @"Server=localhost;Database=" + dataBase + ";Trusted_Connection=True;User Id=" + dataBaseUser + ";Password=" + dataBasePassword);
            });
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
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpect fault happened. Try again later.");
                    });
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
