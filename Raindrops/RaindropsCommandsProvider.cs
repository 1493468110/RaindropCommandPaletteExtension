using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Raindrops.Helpers;
using Raindrops.Pages;

namespace Raindrops;

public partial class RaindropsCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;
    private static readonly RaindropsSettingsManager _settingsManager = new();

    public RaindropsCommandsProvider()
    {
        DisplayName = RaindropsPage.PageTitle;
        // Use the same local asset icon
        Icon = RaindropsPage.AppIcon;
        Settings = _settingsManager.Settings;

        _commands = [
            new CommandItem(new RaindropsPage(_settingsManager))
            {
                Title = DisplayName,
                Subtitle = RaindropsPage.PageSubtitle
            },
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _commands;
    }
}
