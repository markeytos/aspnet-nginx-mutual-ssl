using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using CertificateAuthenticationTest.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace CertificateAuthenticationTest.Handlers;

public class SubjectNameHandler : AuthorizationHandler<SubjectNameRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SubjectNameRequirement requirement
    )
    {
        var claim = context.User.FindFirst(c => c.Type == ClaimTypes.X500DistinguishedName);
        if (claim is null)
        {
            return Task.CompletedTask;
        }

        var sn = new X500DistinguishedName(claim.Value)
            .EnumerateRelativeDistinguishedNames()
            .FirstOrDefault(dn => dn.GetSingleElementType().FriendlyName == "SN")
            ?.GetSingleElementValue();
        if (sn == requirement.SubjectName)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
