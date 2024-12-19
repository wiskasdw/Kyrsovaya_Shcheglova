var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseFileServer();

void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews(); // Добавляем поддержку контроллеров и представлений
}

void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        // Настройка маршрутов с использованием Map()
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // Дополнительные маршруты для новых файлов
        endpoints.MapControllerRoute(
            name: "admin",
            pattern: "admin",
            defaults: new { controller = "Chapters", action = "Admin" });

        endpoints.MapControllerRoute(
            name: "index",
            pattern: "",
            defaults: new { controller = "Chapters", action = "Index" });

        endpoints.MapControllerRoute(
            name: "lectures",
            pattern: "lectures",
            defaults: new { controller = "Chapters", action = "Lectures" });

        endpoints.MapControllerRoute(
            name: "resources",
            pattern: "resources",
            defaults: new { controller = "Resources", action = "Index" });

        endpoints.MapControllerRoute(
            name: "contact",
            pattern: "contact",
            defaults: new { controller = "Contact", action = "Index" });
    });
}

app.Run(async (context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";

    // Определяем путь к файлу в зависимости от запрашиваемого URL
    string filePath = context.Request.Path.Value.ToLower() switch
    {
        "/admin" => "wwwroot/html/admin.html",
        "/" => "wwwroot/html/index.html",
        "/lectures" => "wwwroot/html/lectures.html",
        _ => null // Если путь не соответствует, возвращаем null
    };

    if (filePath != null)
    {
        await context.Response.SendFileAsync(filePath);
    }
    else
    {
        context.Response.StatusCode = 404; // Устанавливаем статус 404, если файл не найден
        await context.Response.WriteAsync("404 - Not Found");
    }
});

app.Run();
