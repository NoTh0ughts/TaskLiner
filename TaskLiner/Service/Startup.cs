using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TaskLiner.DB.Entity;
using TaskLiner.DB.Repos;
using TaskLiner.DB.UnitOfWork;

namespace TaskLiner.Service
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

            services.AddControllers()
                .AddNewtonsoftJson(x =>  x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

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
                .AddCustomRepository<WorkerContract, GenericRepository<WorkerContract>>()
                .AddCustomRepository<TaskComment, GenericRepository<TaskComment>>()
                .AddCustomRepository<ProjectAccess, GenericRepository<ProjectAccess>>();

            services.AddMediatR(this.GetType().Assembly);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    #if DEBUG 
                    options.RequireHttpsMetadata = false;
                    #else
                    options.RequireHttpsMetadata = true;
                    #endif
                    
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.IDUSER,
                        
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query?["access_token"];
                            if (!accessToken.HasValue) return System.Threading.Tasks.Task.CompletedTask;

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }
                            return System.Threading.Tasks.Task.CompletedTask;
                        }
                    };
                });

            services.AddSignalR();
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

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication(); 
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
