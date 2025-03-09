//======== oracle connect db ==============

using HotelReservations.Data;
using HotelReservations.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Services

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<DapperFactory>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new Exception("Invalid connection string");


builder.Services.AddDbContext<DataContext>(options =>
{
    // connect with oracle
    //options.UseOracle(connectionString);
    // connect with sql server
    options.UseSqlServer(connectionString);
});
builder.Services.AddScoped<IUserService, UserServiceImpl>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.AccessDeniedPath = new PathString("/Account/AccessDenied");
        option.LoginPath = new PathString("/Account/Login");
        option.ReturnUrlParameter = "ReturnUrl";
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure request pipeline or middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.Run();

//======== sql server connect db ============== 


//using HotelReservations.Data;
////using LoanManagementc103.Services;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);
//// Add Services
////builder.Services.AddSingleton<DapperFactory>(); working arive close progam

//builder.Services.AddControllersWithViews();
//builder.Services.AddScoped<DapperFactory>();
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
//    ?? throw new Exception("Invalid connectionstring");

//builder.Services.AddDbContext<DataContext>(options =>
//{    // connect with oracle
//    options.UseOracle(connectionString);
//    // connect with sql server
//    options.UseSqlServer(connectionString);
//});

////builder.Services.AddScoped<IUserService, UserServiceImpl>();
//builder.Services.AddHttpContextAccessor();
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(option =>
//    {
//        option.AccessDeniedPath = new PathString("/Account/AccessDenied");
//        option.LoginPath = new PathString("/Account/Login");
//        option.ReturnUrlParameter = "ReturnUrl";
//    });
//builder.Services.AddAuthorization();
////in service we use Add to add service

//var app = builder.Build();

//// configure request pipeline or middleware
////in middleware we use key word Use to add middleware or request pipeline

////Dont use this keyword :
////controller
////action
////area
//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();


//app.UseAuthentication();
//app.UseAuthorization();

//app.MapDefaultControllerRoute();
////app.MapControllerRoute(name:"defualt", pattern: "{controller=home}/{action=index}/{id?}");
//app.Run();