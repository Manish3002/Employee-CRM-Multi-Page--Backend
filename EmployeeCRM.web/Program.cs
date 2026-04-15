using EmployeeCRM.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddHttpClient<EmployeeApiService>();
builder.Services.AddScoped<IEmployeeApiService, EmployeeApiService>();

builder.Services.AddHttpClient<ClientApiService>();
builder.Services.AddScoped<IClientApiService, ClientApiService>();

builder.Services.AddHttpClient<TaskApiService>();
builder.Services.AddScoped<ITaskApiService, TaskApiService>();

builder.Services.AddScoped<IDashboardService, DashboardService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();