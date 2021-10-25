using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TaskLiner.DB.Entity;
using TaskLiner.DB.Repos;
using TaskLiner.DB.UnitOfWork;

namespace TaskLiner
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Ётот метод вызываетс€ во врем€ исполнени€, необходим дл€ добавлени€ сервисов в контейнер
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "TaskLiner", 
                    Version = "v1",
                    Description = "Program for management of projects",
                    Contact = new OpenApiContact()
                    {
                        Name = "Repo:",
                        Email = "",
                        Url = new System.Uri("https://github.com/TakeMe100Points/TaskLiner")
                    }
                });
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/auth/login");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/auth/denied");
                });


            // ƒобавление UnitOfWork дл€ контекста данных приложени€, а так же репозиториев дл€ каждой модели данных
            services.AddEntityFrameworkMySql()
                .AddDbContext<TaskLinerContext>()
                .AddUnitOfWork<TaskLinerContext>()
                .AddCustomRepository<Company, GenericRepository<Company>>()
                .AddCustomRepository<Project, GenericRepository<Project>>()
                .AddCustomRepository<Task, GenericRepository<Task>>()
                .AddCustomRepository<TaskUser, GenericRepository<TaskUser>>()
                .AddCustomRepository<TaskUserSubscriber, GenericRepository<TaskUserSubscriber>>()
                .AddCustomRepository<User, GenericRepository<User>>()
                .AddCustomRepository<WorkerContract, GenericRepository<WorkerContract>>();


        }

        //Ётот метод вызываетс€ во врем€ исполнени€, используетс€ дл€ конфигурции HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskLiner v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class ServiceExtensions
    {
        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<IRepositoryFactory, UnitOfWork<TContext>>();
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();

            return services;
        }

        public static IServiceCollection AddCustomRepository<TEntity, TRepo>(this IServiceCollection services)
        where TEntity : class
        where TRepo : class, IGenericRepository<TEntity>
        {
            services.AddScoped<IGenericRepository<TEntity>, TRepo>();

            return services;
        }
    }
}
