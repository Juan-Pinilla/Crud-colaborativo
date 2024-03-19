using Crud_colaborativo.Data;
using Crud_colaborativo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Crud_colaborativo.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.Configure<CookiePolicyOptions>(options =>
//{
//	options.CheckConsentNeeded = context => true;
//	options.MinimumSameSitePolicy = SameSiteMode.None;
//});

builder.Services.AddScoped<IUserServiceRepository, UserServiceRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Azure_sql_connectionstring")
        ?? throw new Exception("Connection string not found"));
});

builder.Services.AddIdentity<Funcionario, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
	options.AccessDeniedPath = "/Account/AccessDenied";
	options.LoginPath = "/Auth/Login";
	options.LogoutPath = "/Auth/Logout";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministrador",
        policy => policy.RequireRole("Administrador"));
});

builder.Services.Configure<IdentityOptions>(options =>
{
	options.Password.RequireLowercase = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 4;
	options.Password.RequireNonAlphanumeric = false;
});


//builder.Services.AddDefaultIdentity<Funcionario>(options => options.SignIn.RequireConfirmedAccount = true)
//	.AddRoles<IdentityRole>()
//	.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddMvc();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var context = services.GetRequiredService<ApplicationDbContext>();
	//context.Database.Migrate();
	DbInitializer.Initialize(context);
}	

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
} else
{
	app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
