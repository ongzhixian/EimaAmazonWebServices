namespace AuthenticationLambdas.Models;

class ResponseModels
{
}
public record AuthenticateCredentialsResponse(bool CredentialsAreValid, string? Jwt);
