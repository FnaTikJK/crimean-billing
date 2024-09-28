using System.Reflection;
using API.DAL;
using API.Infrastructure;
using API.Infrastructure.Config;
using API.Infrastructure.Middlewares;
using API.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddDbContext<DataContext>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonConfig.DateOnlyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new JsonConfig.DateTimeConverter());
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.RegisterMiddlewares();

app.Run();