using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Produto.Data;
using Produto.Repositorios;
using Produto.Repositorios.Interfaces;

namespace Produto
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddEntityFrameworkSqlServer()
                .AddDbContext<ProdutoDBContext>(
                    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase"))
                );

            builder.Services.AddScoped<IProdutoRepositorioSQLServer, ProdutoRepositorioSQLServer>();

            builder.Services.AddScoped<IProdutoContextMDB, ProdutoMongoDBContext>();
            builder.Services.AddScoped<IProdutoRepositorioMongoDB, ProdutoRepositorioMongoDB>();

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