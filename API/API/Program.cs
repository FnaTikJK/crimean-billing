using System.Reflection;
using API.DAL;
using API.Infrastructure;
using API.Infrastructure.Config;
using API.Infrastructure.Middlewares;
using API.Modules;
using API.Modules.TelegramModule;
using Microsoft.AspNetCore.Authentication.Cookies;

var CorsPolicyName = "_crimean-billing";
var CorsOrigins = new string[] {
        //"http://lk.crimean-billing.work.gd" ,
        "https://lk.crimean-billing.work.gd" ,
        //"http://arm.crimean-billing.work.gd" ,
        "https://arm.crimean-billing.work.gd",
        //"http://crimean-billing.work.gd" ,
        "https://crimean-billing.work.gd" ,
        //"http://www.crimean-billing.work.gd" ,
        "https://www.crimean-billing.work.gd",
        "http://localhost:4200",
    };

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName,
                          policy =>
                          {
                              policy.WithOrigins(CorsOrigins)
                              .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                             // policy.WithOrigins(CorsOrigins)
                          });
});

builder.Services.AddDbContext<DataContext>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonConfig.DateOnlyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new JsonConfig.DateTimeConverter());
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(opt =>
    {
        opt.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = (context) =>
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = (context) =>
            {
                context.Response.StatusCode = 403;
                return Task.CompletedTask;
            },
        };
        opt.LoginPath = "/api/Accounts/Login";
    });

builder.Services.AddAutoMapper(typeof(BaseMappingProfile));

builder.Services.RegisterModules();

builder.Services.AddSwaggerGen();

var app = builder.Build();

ConfigReader.Init(app.Environment.IsDevelopment());

// if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseCors(CorsPolicyName);

// app.UseCors(builder => builder.WithOrigins("https://*.crimean-billing.work.gd")
//                                                 .SetIsOriginAllowedToAllowWildcardSubdomains()
//                                                   .AllowAnyHeader()
//                                                   .AllowAnyMethod());
app.UseCors(builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.RegisterMiddlewares();

app.ConfigureDaemons();

app.Run();