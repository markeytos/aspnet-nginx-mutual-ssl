using Microsoft.AspNetCore.Authorization;

namespace CertificateAuthenticationTest.Requirements;

public class SubjectNameRequirement(string subjectName) : IAuthorizationRequirement
{
    public string SubjectName => subjectName;
}
