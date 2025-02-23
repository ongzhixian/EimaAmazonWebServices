namespace Eima.AuthenticationServices.Tests;

[TestClass()]
public class AuthenticationService_ValidateJwtTests
{
    private const string testIssuer = "SomeIssuingApplicationOrSystem";
    private const string testAudience = "SomeRequestingApplicationOrSystem";
    private const string testSubject = "SomeUserId";
    private readonly AuthenticationService authenticationService = new AuthenticationService();
    private readonly byte[] securityBytes = System.Security.Cryptography.RandomNumberGenerator.GetBytes(256 / 8);

    [TestMethod()]
    [DataRow(testIssuer, testAudience)]
    [DataRow(testIssuer, null)]
    [DataRow(null, testAudience)]
    [DataRow(null, null)]
    public void WhenValidatingWithIssuerAndAudience_ReturnTrue(string issuer, string audience)
    {
        var jwt = authenticationService.CreateJwt(securityBytes, testIssuer, testAudience, testSubject);

        bool isValidJwt = authenticationService.ValidateJwt(jwt, securityBytes, issuer, audience);

        Assert.IsNotNull(jwt);
        Assert.IsTrue(isValidJwt);
    }


    [TestMethod()]
    public void WhenIssuerSigningKeyIsDifferent_ReturnFalse()
    {
        var securityBytes2 = System.Security.Cryptography.RandomNumberGenerator.GetBytes(256 / 8);
     
        var jwt = authenticationService.CreateJwt(securityBytes, testIssuer, testAudience, testSubject);

        bool isValidJwt = authenticationService.ValidateJwt(jwt, securityBytes2, testIssuer, testAudience);

        Assert.IsNotNull(jwt);
        Assert.IsFalse(isValidJwt);
    }

    [TestMethod()]
    [DataRow("wrongIssuer", testAudience)]
    [DataRow(testIssuer, "wrongAudience")]
    public void WhenValidatingWithIssuerAndAudience_ReturnFalse(string issuer, string audience)
    {
        var jwt = authenticationService.CreateJwt(securityBytes, testIssuer, testAudience, testSubject);

        bool isValidJwt = authenticationService.ValidateJwt(jwt, securityBytes, issuer, audience);

        Assert.IsNotNull(jwt);
        Assert.IsFalse(isValidJwt);
    }

}