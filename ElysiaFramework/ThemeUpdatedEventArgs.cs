using System.Windows.Media;

namespace ElysiaFramework;

public class ThemeUpdatedEventArgs
{
    public int ThemeMode = 0;
    public Color Primary;
    public Color Secondary;
    public int RealThemeMode = 0;
}