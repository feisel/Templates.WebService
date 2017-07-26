﻿using System;
using Axoom.Extensions.Configuration.Yaml;
using Axoom.Extensions.Logging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Axoom.MyService
{
    /// <summary>
    /// Startup class used by ASP.NET Core.
    /// </summary>
    public class Startup
    {
        [UsedImplicitly]
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Called by ASP.NET Core to set up an environment.
        /// </summary>
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddYamlFile("appsettings.yml", optional: false, reloadOnChange: true)
                .AddYamlFile($"appsettings.{env.EnvironmentName}.yml", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Called by ASP.NET Core to register services.
        /// </summary>
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddRestApi()
                .AddLogging()
                .AddOptions()
                //.Configure<MyOptions>(Configuration.GetSection("MyOptions"))
                //.AddTransient<IMyService, MyService>()
                //.AddSingleton<Worker>()
                ;
        }

        /// <summary>
        /// Called by ASP.NET Core to configure services after they have been registered.
        /// </summary>
        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IServiceProvider provider)
        {
            loggerFactory
                .AddAxoomLogging(Configuration.GetSection("Logging"))
                .CreateLogger<Startup>()
                .LogInformation("Starting My Service");

            app.UseRestApi();
        }
    }
}