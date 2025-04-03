using Microsoft.EntityFrameworkCore;
using TestForMax.Models;
using TestForMax.Services;

var builder = WebApplication.CreateBuilder(args);

// Подключение к PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавление сервисов
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IApiEndpointService, ApiEndpointService>();
builder.Services.AddHttpClient();

// Добавление Razor Pages и контроллеров
builder.Services.AddRazorPages();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // Для production включить HTTPS
    // app.UseHsts();
}

// Создание и применение миграций при запуске приложения
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ошибка при миграции базы данных");
    }
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Маршрутизация для Razor Pages
app.MapRazorPages();

// Маршрутизация для API контроллеров
app.MapControllers();

app.Run();
