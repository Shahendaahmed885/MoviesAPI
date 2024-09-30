using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MoviesAPI.Models;
using MoviesAPI.services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionstring = builder.Configuration.GetConnectionString("Defaultconnection");
builder.Services.AddDbContext<ApplicationDbContext>(Options=>
  Options.UseSqlServer(connectionstring));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IGenresService, GenresService>();
builder.Services.AddTransient<ImoviesService, movieservice>();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddCors();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(name: "v1", info: new OpenApiInfo

    {
        Version = "v1",
        Title="TestApi",
        Description="My First Api",
        TermsOfService=new Uri(uriString:"https://www.google.com"),
        Contact= new OpenApiContact
        {
            Name= "shahenda",
            Email="test@domain.com"
        },
        License=new OpenApiLicense
        {
            Name = " My license",
            Url= new Uri(uriString: "https://www.google.com"),

        }

    });
    options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {

        Name="Authorization",
        Type= SecuritySchemeType.ApiKey,
        Scheme= "Bearer",
        BearerFormat="JWT",
        In= ParameterLocation.Header,
        Description="Enter your JWT key"

    });
    options.AddSecurityRequirement(securityRequirement: new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type= ReferenceType.SecurityScheme,
                    Id= "Bearer",
                },
                Name= "Bearer",
                In= ParameterLocation.Header,
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
