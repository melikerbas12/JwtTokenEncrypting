using System.Security.Cryptography.X509Certificates;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

public class JweTokenCreationService : DefaultTokenCreationService
{
        public JweTokenCreationService(ISystemClock clock, IKeyMaterialService keys, IdentityServerOptions options, ILogger<DefaultTokenCreationService> logger) : base(clock, keys,options, logger)
        {

        }

    public override async Task<string> CreateTokenAsync(Token token)
    {
        if (token.Type == IdentityServerConstants.TokenTypes.IdentityToken)
        {
            var payload = await base.CreatePayloadAsync(token);

            var handler = new JsonWebTokenHandler();
            var jwe = handler.CreateToken(
                payload.SerializeToJson(),
                await Keys.GetSigningCredentialsAsync(),

                // hardcoded... instead load public key per client
                new X509EncryptingCredentials(new X509Certificate2("idsrv3test.cer"))); 

            return jwe;
        }

        return await base.CreateTokenAsync(token);
    }
}