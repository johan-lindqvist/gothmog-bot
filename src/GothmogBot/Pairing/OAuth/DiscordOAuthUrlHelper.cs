using System.Text.Encodings.Web;

namespace GothmogBot.Pairing.OAuth;

public static class DiscordOAuthUrlHelper
{
	public static Uri GenerateRedirectUrl(string clientId, string redirectUri) =>
		new($"https://discord.com/api/oauth2/authorize?client_id={clientId}&redirect_uri={UrlEncoder.Default.Encode(redirectUri)}&response_type=code&scope=connections identify");
}
