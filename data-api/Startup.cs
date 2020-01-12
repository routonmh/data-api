using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAPI.Constants;
using DataAPI.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace DataAPI
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
            // Cross Origin Resource Sharing - Allow requests from other domains within browsers.
            // Configure CORS Policy
            services.AddCors(o => o.AddPolicy("OpenCorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod() // GET, PUT, POST, DELETE
                    .AllowAnyHeader();
            }));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddOptions();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Data API", Version = "v1"
            }); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            // Add configured Cors Policy
            app.UseCors("OpenCorsPolicy");

            // Setup API Documentation
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Data API");
                c.RoutePrefix = "apidocs"; // View at /apidocs
            });

            if (env.IsDevelopment())
            {
                // Error page
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // HTTPS Header Middleware
                app.UseHsts();
            }

            // Selective Middleware
            // Require Jwt Authentication
            app.UseWhen(x => x.Request.Path.Value.Contains("/api/"), appBuilder =>
            {
                appBuilder.UseMiddleware<RequireLocalAuthentication>();

                appBuilder.Use(async (context, next) =>
                {
                    string userAccountIdString = context.Request
                        .Headers[Headers.ACCOUNT_ID_HEADER_NAME];

                    logger.LogInformation("Request from: {0}", userAccountIdString);

                    await next();
                });
            });

            // Attach API controllers
            app.UseMvc();
        }
    }
}