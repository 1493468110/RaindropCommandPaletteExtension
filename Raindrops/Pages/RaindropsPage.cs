using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Raindrops.Helpers;
using Raindrops.Services;

namespace Raindrops.Pages;

internal sealed partial class RaindropsPage : DynamicListPage, IDisposable
{
    public const string PageId = "RaindropsPage";
    public const string PageTitle = "Raindrops";
    public const string PageSubtitle = "Your Raindrop.io Bookmarks";

    // Use local asset instead of remote URL so the icon shows in Command Palette
    public static readonly IconInfo AppIcon =
        IconHelpers.FromRelativePath("Assets\\StoreLogo.scale-100.png");

    private readonly RaindropsSettingsManager _settingsManager;
    private ListItem[]? items;
    private readonly Debouncer debouncer = new();

    public RaindropsPage(RaindropsSettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
        Id = PageId;
        Icon = AppIcon;
        Title = PageTitle;

        _settingsManager.Settings.SettingsChanged += OnSettingsChanged;
    }

    private void OnSettingsChanged(object? sender, Settings args)
    {
        items = null;
        RaiseItemsChanged(0);
    }

    public override IListItem[] GetItems()
    {
        var token = _settingsManager.ApiToken;

        if (string.IsNullOrWhiteSpace(token))
        {
            return [
                new ListItem(new NoOpCommand())
                {
                    Title = "API Token not configured",
                    Subtitle = "Open Command Palette Settings → Extensions → Raindrops to set your token"
                }
            ];
        }

        if (items is not null)
        {
            return items;
        }

        items = RaindropService.GetBookmarksAsync(
            searchTerm: SearchText,
            raindropApiKey: token,
            cancellationToken: debouncer.GetToken()).GetAwaiter().GetResult();

        if (items?.Length == 0)
        {
            return [new ListItem(new NoOpCommand()) { Title = "No bookmarks found." }];
        }

        return items ?? [];
    }

    public override async void UpdateSearchText(string oldSearch, string newSearch)
    {
        if (oldSearch == newSearch || items is null) return;

        var token = _settingsManager.ApiToken;
        if (string.IsNullOrWhiteSpace(token)) return;

        await debouncer.DebounceAsync(async () =>
        {
            IsLoading = true;

            items = await RaindropService.GetBookmarksAsync(
                raindropApiKey: token,
                searchTerm: newSearch,
                cancellationToken: debouncer.GetToken());

            RaiseItemsChanged(items.Length);
            IsLoading = false;

        }, 300);
    }

    public void Dispose()
    {
        _settingsManager.Settings.SettingsChanged -= OnSettingsChanged;
        debouncer.Dispose();
    }
}
