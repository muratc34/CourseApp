﻿using Application.Services;
using Infrastructure.Iyzıco.Settings;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SettingsKey));
        services.Configure<IyzicoSettings>(configuration.GetSection(IyzicoSettings.SettingsKey));

        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Database"));
        });

        services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
        {
            opt.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(configuration["Jwt:SecurityKey"]!)
            };
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IOrderRepository,  OrderRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddAuthorization();

        return services;
    }
}