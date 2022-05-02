using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using api.Configurations;
using api.Context;
using api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FlotillaDbContext>(options => options.UseInMemoryDatabase("flotilla"));
builder.Services.AddScoped<RobotService>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger(builder.Configuration);  

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireRole(builder.Configuration.GetSection("Authorization")["Roles"])
        .Build();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.OAuthClientId(builder.Configuration["AzureAd:ClientId"]);
        // The following parameter represents the "audience" of the access token.
        c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>{ {"Resource", builder.Configuration["AzureAd:ClientId"] } });
        c.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();