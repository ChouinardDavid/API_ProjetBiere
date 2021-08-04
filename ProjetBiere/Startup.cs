using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjetBiere.Entity;
using ProjetBiere.Repository;
using ProjetBiere.Services;
using AutoMapper;
using Newtonsoft;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace ProjetBiere
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
            services.AddDbContext<ProjetBiereContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IBiereRepository, BiereRepository>();
            services.AddScoped<IBiereService, BiereService>();
            services.AddAutoMapper(typeof(Startup));

            //TODO: Retirer le service CORS
            //AddCors() est déjà dans AddControllers()
            services.AddCors(options => options.AddDefaultPolicy(
            builder =>
            {
                //builder.WithOrigins("http://localhost:3000")
                builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                        //.WithMethods("DELETE", "GET", "POST");

        }));

            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });


            //.AddJsonOptions(options =>
            // {
            //     options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            //     //options.JsonSerializerOptions.IgnoreNullValues = true;
            // });

            //services.AddOptions<CorsOptions>("name").Configure(options => options.AddDefaultPolicy(
            //builder =>
            //
            //    builder.WithOrigins("http://localhost:3000")
            //            .AllowAnyHeader()
            //            .AllowAnyMethod();
            //}));
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                logger.LogInformation("In Development environment");
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages(); //A tester
            }

            app.UseHttpsRedirection();

            //À ce point : Any middleware that appears after the UseRouting() call will know which endpoint will run eventually
            app.UseRouting();
            app.UseCors(); //TODO: Ajouter une restriction pour permettre seulement mon site d'accéder à l'API
            //app.UseCors(options => options.WithOrigins("http://192.168.1.145/").AllowAnyMethod()); //TODO: Ajouter une restriction pour permettre seulement mon site d'accéder à l'API
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //This only maps controllers that are decorated with routing attributes - it doesn't configure any conventional routes.
                endpoints.MapControllers();
            });
        }
    }
}
