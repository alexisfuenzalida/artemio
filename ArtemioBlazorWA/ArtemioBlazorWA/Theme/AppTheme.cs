using MudBlazor;

namespace ArtemioBlazorWA.Theme;

public static class AppTheme
{
    public static readonly MudTheme Default = new()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = Colors.Blue.Darken4,
            Background = Colors.Blue.Lighten5,
            AppbarBackground = Colors.Blue.Darken4,
            AppbarText = Colors.Shades.White,
            DrawerBackground = Colors.Shades.White,
        },
        PaletteDark = new PaletteDark()
        {
            Primary = Colors.Blue.Lighten1,
            AppbarBackground = Colors.Gray.Darken4,
            AppbarText = Colors.Shades.White,
        },
    };
}
