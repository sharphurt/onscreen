using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace onscreen.Controls;

public partial class ColorPalette
{
    private readonly List<Color> _paletteColors = new List<Color>
    {
        Color.FromRgb(255, 255, 255),
        Color.FromRgb(128, 128, 128),
        Color.FromRgb(192, 0, 0),
        Color.FromRgb(255, 0, 0),
        Color.FromRgb(255, 138, 0),
        Color.FromRgb(241, 233, 34),
        Color.FromRgb(0, 176, 80),

        Color.FromRgb(204, 204, 204),
        Color.FromRgb(0, 0, 0),
        Color.FromRgb(0, 176, 240),
        Color.FromRgb(0, 112, 192),
        Color.FromRgb(0, 32, 96),
        Color.FromRgb(112, 48, 160),
        Color.FromRgb(255, 64, 196)
    };

    public ColorPalette()
    {
        InitializeComponent();
        GenerateColorPalleteButtons(_paletteColors);
    }

    public event Action<Color> OnPaletteChangingColor; 

    void GenerateColorPalleteButtons(List<Color> colors)
    {
        var colorButtonControlTemplate =
            (ControlTemplate)Application.Current.Resources["ColorPalleteRadioButtonControlTemplate"];
        var whiteButtonControlTemplate =
            (ControlTemplate)Application.Current.Resources["ColorPaletteWhiteRadioButtonControlTemplate"];

        foreach (var button in colors.Select(color =>
                 {
                     var button = new RadioButton
                     {
                         Margin = new Thickness(0, 0, 14, 14),
                         Width = 30,
                         Height = 30,
                         Background = new SolidColorBrush(color),
                         Template = color == Colors.White ? whiteButtonControlTemplate : colorButtonControlTemplate,
                         Focusable = false,
                     };

                     button.Checked += (_, _) => OnPaletteChangingColor?.Invoke(color);

                     return button;
                 }))
        {
            ColorPalleteWrapPanel.Children.Add(button);
        }
    }
}