using System.Data.Common;
using Microsoft.OpenApi.Models;
using MoviesApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var ConnectionString = builder.Configuration.GetConnectionString(name: "DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options=>
options.UsesSqlserver(ConnectionString));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc(name: "v1", info: new OpenApiInfo
        {
            Version = "v1",
            Title = "YassminaApi",
            Description = "my fisrt api",
            TermsOfService = new Uri(uriString: "https://www.google.com"),
            Contact = new OpenApiContact
            {
                Name = "yassmina",
                Email = "Aiventu@index.com"
            },

            License = new OpenApiLicense
            {
                Name = "My License",
                Url = new Uri(uriString: "https://www.google.com")

            }
        });
        options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter you JWT key"
        });
        options.AddSecurityRequirement(securityRequirement: new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "bearer"
                    },
                    Name="bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });

    });
       

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
