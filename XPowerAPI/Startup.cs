using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using XPowerAPI.Logging;
using XPowerAPI.Models;
using XPowerAPI.Models.Params;
using XPowerAPI.Repository;
using XPowerAPI.Services.Account;
using XPowerAPI.Services.Security;
using XPowerAPI.Services.Security.Account;

namespace XPowerAPI
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
            services.Configure<ForwardedHeadersOptions>(o =>
            {
                o.KnownProxies.Add(IPAddress.Parse(Configuration.GetSection("Proxies:0").Value));
            });

            string con = Configuration.GetSection("ConnectionStrings")["maria"];


            services.AddCors(co => co.AddPolicy("allow_all", b => {
                b
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            services
                .AddScoped<ILogger, DbLogger>(
                    x => new DbLogger(new MySqlConnection(con)))
                .AddScoped<IHashingService, SHA512HashingService>()
                .AddScoped<IPasswordService, PasswordService>(
                    x => new PasswordService(
                        new SHA512HashingService(),
                        ((int)Configuration
                            .GetSection("Account")
                            .GetSection("Security")
                            .GetValue(
                                typeof(int),
                                "PasswordMinLength")),
                        ((int)Configuration
                            .GetSection("Account")
                            .GetSection("Security")
                            .GetValue(
                                typeof(int),
                                "PasswordMaxLength"))))
                .AddScoped<IRepository<Customer, CustomerParams>, CustomerRepository>(
                    x => new CustomerRepository(
                            new MySqlConnection(
                                con),
                            new DbLogger(
                                new MySqlConnection(con))))
                .AddScoped<IRepository<SessionKey, SessionKeyParams>>(
                    x => new SessionKeyRepository(
                            new MySqlConnection(con),
                            new DbLogger(
                                new MySqlConnection(con))))
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<ICustomerService, CustomerService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("allow_all");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
