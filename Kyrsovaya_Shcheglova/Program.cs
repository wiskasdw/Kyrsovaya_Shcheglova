var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseFileServer();

void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews(); // ��������� ��������� ������������ � �������������
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
        // ��������� ��������� � �������������� Map()
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // �������������� �������� ��� ����� ������
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

    // ���������� ���� � ����� � ����������� �� �������������� URL
    string filePath = context.Request.Path.Value.ToLower() switch
    {
        "/admin" => "wwwroot/html/admin.html",
        "/" => "wwwroot/html/index.html",
        "/lectures" => "wwwroot/html/lectures.html",
        _ => null // ���� ���� �� �������������, ���������� null
    };

    if (filePath != null)
    {
        await context.Response.SendFileAsync(filePath);
    }
    else
    {
        context.Response.StatusCode = 404; // ������������� ������ 404, ���� ���� �� ������
        await context.Response.WriteAsync("404 - Not Found");
    }
});

app.Run();
