using Microsoft.EntityFrameworkCore;
using Data;
using Servicio;
using Servicio.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<TableroContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ITableroServicio, TableroServicio>();
builder.Services.AddScoped<IColumnaServicio, ColumnaServicio>();
builder.Services.AddScoped<ITareaServicio, TareaServicio>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tablero}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
