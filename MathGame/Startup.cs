using MathGameApplication.DbContexts;
using MathGameApplication.Repository;
using Microsoft.EntityFrameworkCore;

namespace MathGameApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {
                builder.WithOrigins("https://localhost:44359").AllowAnyMethod().AllowAnyHeader();
            }));

            services.AddDbContext<MathGameContext>(options =>
                   options.UseSqlServer(
                   Configuration.GetConnectionString("MathGameDB"),
                   ef => ef.MigrationsAssembly(typeof(MathGameContext).Assembly.FullName)));
            services.AddTransient<IMathGameRepository, MathGameRepository>();
            services.AddControllers();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}