using System;
using System.Collections.Generic;
using System.Threading;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Shapes;

namespace VehicleRegistryManager.Controls;

// A simple popup with a Picker of years (1900 to current). Returns the chosen year on Confirm.
public class YearPickerPopup : Popup<string>
{
    public YearPickerPopup(string? initialYear)
    {
        var years = new List<string>();
        for (var year = 1900; year <= DateTime.Now.Year; year++)
            years.Add(year.ToString());

        var picker = new Picker
        {
            Title = "Select year",
            ItemsSource = years,
            FontSize = 16,
            TextColor = Color.FromArgb("#1F2430"),
        };
        picker.SelectedItem = !string.IsNullOrEmpty(initialYear) && years.Contains(initialYear)
            ? initialYear
            : DateTime.Now.Year.ToString();

        var title = new Label
        {
            Text = "Select year",
            FontAttributes = FontAttributes.Bold,
            FontSize = 18,
            TextColor = Color.FromArgb("#1F2430"),
            HorizontalOptions = LayoutOptions.Center,
        };

        var cancelButton = new Button
        {
            Text = "Cancel",
            BackgroundColor = Colors.Transparent,
            TextColor = Color.FromArgb("#6B7280"),
        };
        cancelButton.Clicked += async (_, _) => await CloseAsync(string.Empty, CancellationToken.None);

        var confirmButton = new Button
        {
            Text = "Confirm",
            BackgroundColor = Color.FromArgb("#1E5EA8"),
            TextColor = Colors.White,
        };
        confirmButton.Clicked += async (_, _) => await CloseAsync(picker.SelectedItem as string ?? string.Empty, CancellationToken.None);

        var buttons = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
            },
            ColumnSpacing = 12,
        };
        buttons.Add(cancelButton, 0, 0);
        buttons.Add(confirmButton, 1, 0);

        Content = new Border
        {
            BackgroundColor = Colors.White,
            StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = 16 },
            Padding = 16,
            Content = new VerticalStackLayout
            {
                Spacing = 16,
                WidthRequest = 260,
                Children = { title, picker, buttons },
            },
        };
    }
}
