using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskManager.Auth;
using TaskManager.Common;
using TaskManager.Database;
using TaskManager.Models;
using TaskManager.Models.Domain.ScheduledTask;

namespace TaskManager
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<TaskManagerContext>(
                options => options.UseNpgsql(Configuration.GetConnectionString("TaskManagerContext")));
            services.AddHttpContextAccessor();
            services.AddScoped(ctx => new CurrentUser(
                ctx.GetRequiredService<IHttpContextAccessor>().HttpContext!.Request.Headers["email"]));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseMiddleware<ScheduledTaskMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
