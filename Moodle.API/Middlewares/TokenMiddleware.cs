using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Moodle.API.Middlewares{
    public class TokenMiddleware{
        private readonly RequestDelegate reqdel;
        private readonly IConfiguration config;
        public TokenMiddleware(RequestDelegate rqdl, IConfiguration cnfg){
            reqdel = rqdl;
            config = cnfg;
        }
        public async Task InvokeAsync(HttpContext context){
            string accessToken = "";
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var authorizationHeader = context.Request.Headers["Authorization"];
                var parts = authorizationHeader.FirstOrDefault().Split(' ');
                if (parts.Length == 2 && parts[0] == "Bearer")
                {
                    accessToken = parts[1];
                    //await Console.Out.WriteLineAsync();
                }
            }
            if (!string.IsNullOrEmpty(accessToken))
            {
                try
                {
                    var issuerSigningKey = config["Jwt:SecretKey"];
                    var issuer = config["Jwt:Issuer"];
                    var audience = config["Jwt:Audience"];

                    var tokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey)),
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateLifetime = true
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.ReadJwtToken(accessToken);

                    if (securityToken.ValidTo < DateTime.UtcNow)
                    {
                        throw new SecurityTokenException("Access token is expired.");
                    }

                    tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken validatedToken);
                }
                catch (SecurityTokenException ex)
                {
                    Console.WriteLine($"Security token validation error: {ex.Message}");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized; // Set unauthorized status code
                    context.Response.Headers.Add("Token-Expired", "true");
                    return;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Invalid token format: {ex.Message}");
                    // Decide how to handle invalid format based on your security needs
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized; // Example: Set unauthorized status code
                    return;
                }
            }
            await reqdel.Invoke(context);            
        }
    }
    public static class TokenMiddlewareExtensions{
        public static IApplicationBuilder UseTokenMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenMiddleware>(builder.ApplicationServices.GetService<IConfiguration>());
        }
    }
}