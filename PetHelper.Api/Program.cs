using Microsoft.Extensions.Options;
using PetHelper.Api.Extensions;
using PetHelper.Domain.Auth;
using PetHelper.Startup.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterDependencies();
builder.Services.AddCors(x => x.AllowAnyOriginPolicy());

builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureAuthentication(builder.Configuration.GetSection("Jwt").Get<JwtSettings>());
builder.Services.ConfigureDbConnection(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCulture();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
