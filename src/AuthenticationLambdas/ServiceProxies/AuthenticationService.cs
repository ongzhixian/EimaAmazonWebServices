
using Eima.AuthenticationServices.Models;

namespace AuthenticationLambdas.ServiceProxies;

class AuthenticationService
{
    private readonly Eima.AuthenticationServices.AuthenticationService authenticationService = new Eima.AuthenticationServices.AuthenticationService();

    public AuthenticationService()
    {
    }

    internal async Task<bool> ValidateUserCredentialsAsync(UserCredentials userCredentials) =>
        await authenticationService.ValidateUserCredentialsAsync(userCredentials);
}
