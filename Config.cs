using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryService.Models;
using LibraryService.Repositories;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
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
            .AddSingleton<IRepository<Book>>(new InMemoryBookRepository("data.book.json"))
            .AddSingleton<IRepository<Genre>>(new InMemoryGenreRepository("data.genre.json"))
            .AddSingleton<IRepository<BookGenre>>(new InMemoryBookGenreRepository("data.book_genre.json"))
            .AddSingleton<IRepository<Member>>(new InMemoryMemberRepository("data.member.json"))
            .AddSingleton<IRepository<BookInstance>>(new InMemoryBookInstanceRepository("data.book_status.json"))
            .AddEndpointsApiExplorer()
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<Program>()
            .AddFluentValidationClientsideAdapters()
            .AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.EnableAnnotations();
                
                const string title = "LibraryService";

                swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = title,
                    Description = "A library management system",
                    Version = "v1",
                });

                var filePath = Path.Combine(AppContext.BaseDirectory, $"{title}.xml");
                swaggerGenOptions.IncludeXmlComments(filePath);
            })
            .AddFluentValidationRulesToSwagger()
            .AddControllers()
            .AddNewtonsoftJson(jsonOptions => jsonOptions.SerializerSettings.Converters.Add(new StringEnumConverter()));
        
        // must be called after "AddNewtonsoftJson" according to https://stackoverflow.com/a/55541764
        builder.Services.AddSwaggerGenNewtonsoftSupport();
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