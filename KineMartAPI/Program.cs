using AutoMapper;
using KineMartAPI;
using KineMartAPI.Exceptions;
using KineMartAPI.ModelEntities;
using KineMartAPI.Repositories;
using KineMartAPI.RepositoryImpls;
using KineMartAPI.ServiceImpls;
using KineMartAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

Serilog.Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

try
{
    
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    //DbContext
    builder.Services.AddDbContext<MartDbContext>(option => option.UseSqlServer(builder.Configuration
                                                                    .GetConnectionString("dbcon")));

    //Inmemory-Db
    //builder.Services.AddDbContext<MartDbContext>(options => options.UseInMemoryDatabase("localDb"));

    //Repository
    builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

    //Services
    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<ISupplierService, SupplierService>();
    builder.Services.AddScoped<IProductPropertyService, ProductPropertyService>();
    builder.Services.AddScoped<IImportService, ImportService>();
    builder.Services.AddScoped<IExportService, ExportService>();
    builder.Services.AddScoped<IProductImportService, ProductImportService>();
    builder.Services.AddScoped<IProductExportService, ProductExportService>();
    builder.Services.AddScoped<ISaleService, SaleService>();
    builder.Services.AddScoped<ILogService, LogService>();
    builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
    builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

    var tokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Secret"]!)),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
    builder.Services.AddSingleton(tokenValidationParameters);

    //Add Identity
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<MartDbContext>()
                                                                               .AddDefaultTokenProviders();

    //Add Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).
    //Add Jwt Bearer
    AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = tokenValidationParameters;
    });

    //AutoMapper
    builder.Services.AddAutoMapper(typeof(MapperConfig));

    //Serilog
    builder.Host.UseSerilog();

    //Content Negotiation
    builder.Services.AddMvc(config =>
    {
        config.RespectBrowserAcceptHeader = true;
        config.ReturnHttpNotAcceptable = true;
    });

    //Cors
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
        });
    });

    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.AddGlobalErrorHandler();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCors();
    app.MapControllers();

    app.Run();
}
finally
{
    Serilog.Log.CloseAndFlush();
}

