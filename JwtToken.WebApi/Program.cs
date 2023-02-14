using System.Security.Cryptography.X509Certificates;
using IdentityServer4.Services;
using JwtToken.WebApi.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ITokenCreationService, JweTokenCreationService>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "cookie";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("cookie")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "http://localhost:5000";
        options.ClientId = "mvc";
        options.ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0";
        options.ResponseType = "id_token code";

        options.SignInScheme = "cookie";
        options.RequireHttpsMetadata = false;
        options.ProtocolValidator = new JweProtocolValidator
        {
            RequireStateValidation = options.ProtocolValidator.RequireStateValidation,
            NonceLifetime = options.ProtocolValidator.NonceLifetime
        };
        options.Events = new OpenIdConnectEvents
        {
            // after initial validation, but before calling ProtocolValidator
            OnTokenValidated = context =>
            {
                context.SecurityToken = context.SecurityToken.InnerToken ?? context.SecurityToken;
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            TokenDecryptionKey = new X509SecurityKey(new X509Certificate2("idsrv3test.pfx", "idsrv3test"))
        };
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
