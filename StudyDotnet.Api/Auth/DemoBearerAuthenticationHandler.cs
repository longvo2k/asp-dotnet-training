using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace StudyDotnet.Api.Auth;

public sealed class DemoBearerAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "DemoBearer";

    public DemoBearerAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var authorization = authorizationHeader.ToString();

        if (!authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.Fail("Authorization header must use Bearer token."));
        }

        var token = authorization["Bearer ".Length..].Trim();
        var claims = ReadClaimsFromDemoToken(token);

        if (claims.Count == 0)
        {
            return Task.FromResult(AuthenticateResult.Fail("Token is invalid."));
        }

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    private static List<Claim> ReadClaimsFromDemoToken(string token)
    {
        try
        {
            var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(token));

            return decodedToken
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(part => part.Split('=', 2))
                .Where(pair => pair.Length == 2)
                .Select(ToClaim)
                .ToList();
        }
        catch (FormatException)
        {
            return new List<Claim>();
        }
    }

    private static Claim ToClaim(string[] pair)
    {
        return pair[0] switch
        {
            "user" => new Claim(ClaimTypes.Name, pair[1]),
            "tenant" => new Claim("tenant", pair[1]),
            _ => new Claim(pair[0], pair[1])
        };
    }
}
