using WebMVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ MVC với các controller và views
builder.Services.AddControllersWithViews();

// Cấu hình HttpClient cho các dịch vụ
builder.Services.AddHttpClient<IProgrammeService, ProgrammeService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
});

builder.Services.AddHttpClient<IContactService, ContactService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
});

// Thêm dịch vụ CORS nếu cần
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

// Cấu hình pipeline cho các yêu cầu HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Cấu hình CORS nếu cần
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

// Cấu hình các route cho MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Programme}/{action=Index}/{id?}");

app.Run();