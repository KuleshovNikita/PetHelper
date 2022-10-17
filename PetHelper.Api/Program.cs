using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PetHelper.DataAccess.Context;
using PetHelper.Startup.Extensions;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterDependencies();
builder.Services.AddCors(x => x.AddPolicy(
                            name: "AllowOrigin",
                            configurePolicy: p => p.AllowAnyOrigin()
                                                   .AllowAnyHeader()
                                                   .AllowCredentials()
                                                   .AllowAnyMethod()
                        ));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        var expireTime = builder.Configuration.GetSection("TokenExpirationTime").Value;
        var cookieName = builder.Configuration.GetSection("CookieTokenName").Value;

        opt.Cookie.Name = cookieName;
        opt.ExpireTimeSpan = TimeSpan.FromMinutes(int.Parse(expireTime));
        opt.SlidingExpiration = true;
        opt.Events.OnRedirectToLogin = (op) => Task.FromResult(op.Response.StatusCode = 401);
    });

builder.Services.AddDbContext<PetHelperDbContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var culture = app.Configuration.GetSection("Culture").Value;
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);
CultureInfo.CurrentCulture = new CultureInfo(culture);
CultureInfo.CurrentUICulture = new CultureInfo(culture);


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
