using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace OpticalCharacterRecognition.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            IronOcr.Installation.LicenseKey = "IRONOCR.AFIDKURNIAWAN.26866-547E8FE2DB-CXNB7Y-FCNRXJNPYATL-2TX5NXN2C25F-YU6NAKAIWFIM-KJLVSQCNLEJ4-2P7HPUK5LDQV-IU6CEO-TF64ZJVSJ4KAEA-DEPLOYMENT.TRIAL-DWADH5.TRIAL.EXPIRES.24.JUN.2021";
        }

        public IConfiguration Configuration { get; }

        public const string CorsPolicy = "AllowAny";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy(CorsPolicy, pol => pol
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .Build());
            });
            
            services.AddControllers();

            services.AddSwaggerGen(opt => opt
                .SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Services.Api",
                    Version = "v1"
                }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(opt => opt
                .SwaggerEndpoint("/swagger/v1/swagger.json", "Services.Api v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(opt => opt
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .Build());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
