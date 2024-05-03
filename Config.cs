using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryService.Models;
using LibraryService.Repositories;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Supabase;

namespace LibraryService.Configuration;

public static class Config
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        Env.Load();
        
        var url = Environment.GetEnvironmentVariable("SUPABASE_URL");
        var key = Environment.GetEnvironmentVariable("SUPABASE_KEY");

        var options = new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true,
        };

        builder.Services
            .AddSingleton(_ => new Client(url, key, options))
            .AddSingleton<IRepository<Book>>(new InMemoryBookRepositoryBase("data.book.json"))
            .AddSingleton<IRepository<Genre>>(new InMemoryGenreRepositoryBase("data.genre.json"))
            .AddSingleton<IRepository<BookGenre>>(new InMemoryBookGenreRepositoryBase("data.book_genre.json"))
            .AddSingleton<IRepository<Member>>(new InMemoryMemberRepositoryBase("data.member.json"))
            .AddEndpointsApiExplorer()
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<Program>()
            .AddFluentValidationClientsideAdapters()
            .AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                
                const string title = "LibraryService";

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = title,
                    Description = "A library management system",
                    Version = "v1",
                });

                var filePath = Path.Combine(AppContext.BaseDirectory, $"{title}.xml");
                options.IncludeXmlComments(filePath);
            })
            .AddSwaggerGenNewtonsoftSupport()
            .AddFluentValidationRulesToSwagger()
            .AddControllers()
            .AddNewtonsoftJson();
    }
    
    public static void RegisterMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger()
                .UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();
    }
}