using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.Certificate;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate(
    options =>
    {
        options.AllowedCertificateTypes = CertificateTypes.All;
        options.ChainTrustValidationMode = X509ChainTrustMode.CustomRootTrust;
        options.CustomTrustStore = [X509Certificate2.CreateFromPemFile("./cert.pem")];
        options.RevocationFlag = X509RevocationFlag.EntireChain;
    });
builder.Services.AddAuthorizationBuilder().AddFallbackPolicy("RequireAuthenticatedUser", authorizationPolicyBuilder => authorizationPolicyBuilder.RequireAuthenticatedUser());

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello world.\n");

app.Run();
