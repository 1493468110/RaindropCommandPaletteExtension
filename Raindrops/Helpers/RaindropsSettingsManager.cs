using Meziantou.Framework.Win32;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Raindrops.Constants;

namespace Raindrops.Helpers;

public class RaindropsSettingsManager
{
    private readonly TextSetting _apiToken;

    public Settings Settings { get; } = new Settings();

    public RaindropsSettingsManager()
    {
        var existing = CredentialManager.ReadCredential(Application.Name);

        _apiToken = new TextSetting(
            "raindrop_api_token",
            "Raindrop.io API Token",
            "Your personal token from app.raindrop.io/settings/integrations. Leave blank and save to clear the saved token.",
            existing?.Password ?? string.Empty);

        Settings.Add(_apiToken);
        Settings.SettingsChanged += OnSettingsChanged;
    }

    public string? ApiToken => CredentialManager.ReadCredential(Application.Name)?.Password;

    private void OnSettingsChanged(object? sender, Settings args)
    {
        var token = _apiToken.Value ?? string.Empty;

        if (string.IsNullOrWhiteSpace(token))
        {
            CredentialManager.DeleteCredential(Application.Name);
        }
        else
        {
            CredentialManager.WriteCredential(
                applicationName: Application.Name,
                userName: "API_TOKEN",
                secret: token,
                persistence: CredentialPersistence.LocalMachine);
        }
    }
}
