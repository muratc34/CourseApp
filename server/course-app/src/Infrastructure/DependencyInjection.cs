using Application.Abstractions.Caching;
using Application.Abstractions.Emails;
using Application.Abstractions.Iyzico;
using Application.Abstractions.Messaging;
using Application.Abstractions.Notifications;
using Infrastructure.Caching;
using Infrastructure.Caching.Settings;
using Infrastructure.Emails;
using Infrastructure.Emails.Settings;
using Infrastructure.Iyzıco;
using Infrastructure.Iyzıco.Settings;
using Infrastructure.Messaging;
using Infrastructure.Messaging.Consumers;
using Infrastructure.Messaging.Settings;
using Infrastructure.Notifications;
using MassTransit;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SettingsKey));
        services.Configure<IyzicoSettings>(configuration.GetSection(IyzicoSettings.SettingsKey));
        services.Configure<MailSettings>(configuration.GetSection(MailSettings.SettingsKey));
        services.Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.SettingsKey));
        services.Configure<RedisSettings>(configuration.GetSection(RedisSettings.SettingsKey));

        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Database"));
        });

        services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
        {
            opt.User.RequireUniqueEmail = true;
            opt.SignIn.RequireConfirmedEmail = true;
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

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumer<UserCreatedEventConsumer>();

            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(configuration["MessageBroker:Host"]), h =>
                {
                    h.Username(configuration["MessageBroker:Username"]);
                    h.Password(configuration["MessageBroker:Password"]);
                });

                cfg.ConfigureEndpoints(context);
            });

        });

        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = configuration["Redis:Host"];
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IOrderRepository,  OrderRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<ICacheService, CacheService>();

        services.AddTransient<IIyzicoService, IyzicoService>(); 
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IEmailNotificationService, EmailNotificationService>();
        services.AddTransient<IEventPublisher, RabbitMQEventPublisher>();

        services.AddDistributedMemoryCache();
        services.AddAuthorization();
        return services;
    }
}