using Microsoft.EntityFrameworkCore;
using S08_Labo.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<S08_EmployesContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("BDEmployee")));
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Artistes}/{action=Index}"
        );
});

app.MapRazorPages();

app.Run();
