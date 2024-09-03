using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TokenManager.Web.Api.Handlers;
using TokenManager.Web.Api.Handlers.Stellar;
using TokenManager.Web.Api.Services;
using TokenManager.Web.Api.Services.EOS;
using TokenManager.Web.Api.Services.Stellar;

namespace TokenManager.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env.EnvironmentName;
        }

        public string Env { get; set; }
        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<EosBlockchainService>().Configure<EOSConfig>(Configuration.GetSection($"EOS:{Env}"));
            services.AddSingleton<StellarBlockchainService>().Configure<StellarConfig>(Configuration.GetSection($"Stellar:{Env}"));

            services
                //.AddHostedService<AccountHealthHandler>();
                //.AddHostedService<SettlementHandler>();
                //.AddHostedService<GenbitAccountCreateHandler>()
                //.AddHostedService<StellarAccountCreatorHandler>();
                //.AddHostedService<StellarAccountSignerHandler>();
                //.AddHostedService<GenbitComprasTreepTokenHandler>()
                .AddHostedService<TreepPayTransactionsHandler>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TokenManager API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TokenManager PI V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
