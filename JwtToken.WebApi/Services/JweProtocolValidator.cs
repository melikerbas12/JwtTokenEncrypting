using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace JwtToken.WebApi.Services
{
   public class JweProtocolValidator : OpenIdConnectProtocolValidator
{
    protected override void ValidateIdToken(OpenIdConnectProtocolValidationContext validationContext)
    {
        if (validationContext.ValidatedIdToken.InnerToken != null)
            validationContext.ValidatedIdToken = validationContext.ValidatedIdToken.InnerToken;

        base.ValidateIdToken(validationContext);
    }

    public override void ValidateTokenResponse(OpenIdConnectProtocolValidationContext validationContext)
    {
        if (validationContext.ValidatedIdToken.InnerToken != null)
            validationContext.ValidatedIdToken = validationContext.ValidatedIdToken.InnerToken;

        base.ValidateTokenResponse(validationContext);
    }

    public override void ValidateUserInfoResponse(OpenIdConnectProtocolValidationContext validationContext)
    {
        if (validationContext.ValidatedIdToken.InnerToken != null)
            validationContext.ValidatedIdToken = validationContext.ValidatedIdToken.InnerToken;

        base.ValidateUserInfoResponse(validationContext);
    }
}
}