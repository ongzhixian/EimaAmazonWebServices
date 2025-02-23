using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Eima.AuthenticationServices.Models;
using Eima.DataServices;

using Microsoft.IdentityModel.Tokens;

namespace Eima.AuthenticationServices;

public interface IAuthenticationService
{
    Task<bool> ValidateUserCredentialsAsync(UserCredentials userCredentials);

    string CreateJwt(byte[] securityKeyBytes, string jwtIssuerIdentifier, string targetApplicationName, string targetRecipientIdentifier);

    bool ValidateJwt(string jwt, byte[] securityKeyBytes, string? expectedIssuer = null, string? expectedAudience = null);

    void RefreshJwt(string jwt);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly IDataService dataService;

    public async Task<bool> ValidateUserCredentialsAsync(UserCredentials userCredentials)
    {
        //var o = await dataService.GetAsync("counter", "Home");
        return true;
    }


    // Assuming you have the JWT string and the public key (for RS256) or shared secret (for HS256)
    public bool ValidateJwt(string jwt, byte[] securityKeyBytes, string? expectedIssuer = null, string? expectedAudience = null)
    {
        var securityKey = new SymmetricSecurityKey(securityKeyBytes);

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey, //new RsaSecurityKey(publicKey), // Or new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sharedSecret)) for HS256
            };

            if (validationParameters.ValidateIssuer = expectedIssuer is not null) validationParameters.ValidIssuer = expectedIssuer;

            if (validationParameters.ValidateAudience = expectedAudience is not null) validationParameters.ValidAudience = expectedAudience;

            var principal = new JwtSecurityTokenHandler().ValidateToken(jwt, validationParameters, out SecurityToken validatedToken);

            return principal is not null;

            // If validation succeeds, you can access the claims from the principal
            //string userId = principal.FindFirstValue("sub"); // Example: getting the "sub" claim

            // ... use the validated token
        }
        catch (SecurityTokenExpiredException)
        {
            // Handle expired token
        }
        catch (Exception ex)
        {
            // Handle other validation errors
            Console.WriteLine($"Token validation failed: {ex.Message}");
        }

        return false;
    }


    public string CreateJwt(byte[] securityKeyBytes, string jwtIssuerIdentifier, string targetApplicationName, string targetRecipientIdentifier)
    {
        // Consider expose claims
        var claims = new[] 
        {
            new Claim(JwtRegisteredClaimNames.Sub, targetRecipientIdentifier), 
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()), 
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        
            //new Claim(ClaimTypes.NameIdentifier, userId),
            //new Claim(ClaimTypes.Name, username),
            //new Claim(ClaimTypes.Role, role), // Example role claim
            //new Claim(JwtRegisteredClaimNames.Iss, issuer), // Optional: Issuer
            //new Claim(JwtRegisteredClaimNames.Aud, audience), // Optional: Audience
            //new Claim("custom-claim", "some-value"), // Example of a custom claim
        };

        var securityKey = new SymmetricSecurityKey(securityKeyBytes);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // 4. Create JWT Token
        
        var token = new JwtSecurityToken(
            issuer: jwtIssuerIdentifier,    
            audience: targetApplicationName, 
            
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(1), // Consider expose
            signingCredentials: credentials);

        // 5. Serialize Token to String
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public void RefreshJwt(string jwt)
    {
        throw new NotImplementedException();
    }

    public bool AuthenticateUserCredentials(UserCredentials userCredentials)
    {
        throw new NotImplementedException();
    }
}
