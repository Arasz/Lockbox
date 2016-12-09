﻿using Lockbox.Client.Extensions;
using Lockbox.Examples.WebApp.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lockbox.Examples.WebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddLockbox(encryptionKey: "48q2Xc9FBEUExnrpYSMx",
                    apiUrl: "http://localhost:5000",
                    apiKey: "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJTdWIiOiJyb290IiwiRXhwIjo2Njc3MTY5NjQyNzI1NDYxNTB9.9WcQKEXiFYjS3LwQv4BF6vCUVQUcwNS5N0TJslCfYq_1dOQKGS2wFW6AJlkVTJMFB6xJRgoE6GGvr5x2dlEA0w",
                    boxName: "default",
                    entryKey: "appsettings");
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var databaseSettings = new DatabaseSettings();
            var emailSettings = new EmailSettings();
            Configuration.GetSection("database").Bind(databaseSettings);
            Configuration.GetSection("email").Bind(emailSettings);
            services.AddSingleton(databaseSettings);
            services.AddSingleton(emailSettings);
            services.AddOptions();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMvcWithDefaultRoute();
        }
    }
}