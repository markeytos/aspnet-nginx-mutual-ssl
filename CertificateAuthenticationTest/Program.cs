using System.Net;
using System.Security.Cryptography.X509Certificates;
using CertificateAuthenticationTest.Handlers;
using CertificateAuthenticationTest.Requirements;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
});

builder.Services.AddCertificateForwarding(options =>
{
    options.CertificateHeader = "X-SSL-CERT";
    options.HeaderConverter = headerValue =>
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(headerValue);
        return X509Certificate2.CreateFromPem(WebUtility.UrlDecode(headerValue));
    };
});

builder
    .Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate(options =>
    {
        options.AllowedCertificateTypes = CertificateTypes.All;
        options.ChainTrustValidationMode = X509ChainTrustMode.CustomRootTrust;
        options.CustomTrustStore = [X509Certificate2.CreateFromPemFile("./ca.crt")];
        options.RevocationFlag = X509RevocationFlag.EntireChain;
        // Disabled revocation checks since test certificates do not have CRL.
        options.RevocationMode = X509RevocationMode.NoCheck;
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "RequireAuthenticatedUser",
        authorizationPolicyBuilder => authorizationPolicyBuilder.RequireAuthenticatedUser()
    );
    options.AddPolicy(
        "IsTestUser",
        policy => policy.AddRequirements(new SubjectNameRequirement("Test User"))
    );
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAuthorizationHandler, SubjectNameHandler>();

var app = builder.Build();

app.UseForwardedHeaders();

app.UseCertificateForwarding();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello world.\n")
    .RequireAuthorization("RequireAuthenticatedUser")
    .RequireAuthorization("IsTestUser");

app.Run();
