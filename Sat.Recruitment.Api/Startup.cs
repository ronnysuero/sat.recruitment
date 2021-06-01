using System;
using System.Net;
using Autofac;
using AutoMapper;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Sat.Recruitment.Data;
using Sat.Recruitment.Data.Interfaces;
using Sat.Recruitment.Domain.Validators;

namespace Sat.Recruitment.Api
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
            services.AddSingleton<IUserMemoryCache, UserMemoryCache>();
            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton(
                new MapperConfiguration(mc => mc.AddProfile(new Domain.AutoMapper())).CreateMapper()
            );

            services.AddProblemDetails(options =>
            {
                // Control when an exception is included
                options.IncludeExceptionDetails = (ctx, ex) => true;

                options.Map<Exception>(exception =>
                    {
                        var realError = exception;

                        while (realError.InnerException != null) realError = realError.InnerException;

                        return new ProblemDetails
                        {
                            Status = (int) HttpStatusCode.BadRequest,
                            Detail = realError.Message
                        };
                    }
                );
            });

            services.AddControllers()
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .AddFluentValidation(s =>
                    {
                        s.RegisterValidatorsFromAssemblyContaining<UserValidator>();
                        s.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    }
                );

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Add any Autofac modules or registrations.
            // This is called AFTER ConfigureServices so things you
            // register here OVERRIDE things registered in ConfigureServices.
            //
            // You must have the call to `UseServiceProviderFactory(new AutofacServiceProviderFactory())`
            // when building the host or this won't be called.
            builder.RegisterModule(new AutofacModule());
        }
    }
}