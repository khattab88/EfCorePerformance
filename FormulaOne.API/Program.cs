
using FormulaOne.API.Config;
using FormulaOne.Data;
using FormulaOne.Data.Repositories;
using FormulaOne.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FormulaOne.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            var dbContextConfig = new DbContextConfig();
            builder.Configuration.GetSection("DbContextConfig").Bind(dbContextConfig);

            builder.Services.AddDbContext<AppDbContext>(builder =>
            {
                builder.UseSqlServer(connectionString, options =>
                {
                    options.CommandTimeout(dbContextConfig.CommandTimeout);
                });

                // enabled only on dev environment
                builder.EnableDetailedErrors(dbContextConfig.EnableDetailedErrors);
                builder.EnableSensitiveDataLogging(dbContextConfig.EnableSensitiveDataLogging);
            });


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}