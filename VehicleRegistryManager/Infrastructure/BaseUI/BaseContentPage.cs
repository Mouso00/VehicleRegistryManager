using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using SimpleToolkit.Core;
using VehicleRegistryManager.Infrastructure.Services;


namespace VehicleRegistryManager.Infrastructure.BaseUI;

public class BaseContentPage : ContentPage, IQueryAttributable, IView, ITransient
{
    private IParameters parameters;

    public BaseContentPage()
    {
        this.On<iOS>().SetUseSafeArea(true);
        Microsoft.Maui.Controls.NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        Loaded += BaseContentPageLoaded;
        Unloaded += BaseContentPageUnloaded;
    }

    private void CurrentNavigating(object sender, ShellNavigatingEventArgs e)
    {
        if (SimpleToolkit.SimpleShell.SimpleShell.Current.CurrentPage == this)
        {
            // Null parameters when navigating to a different ShellContent
            var targetString = e.Target.Location.ToString();

            if (targetString.StartsWith("//") && !targetString.Contains(e.Current.Location.ToString()))
                parameters = null;
        }
    }

    private void BaseContentPageLoaded(object sender, EventArgs e)
    {
        this.Window?.SubscribeToSafeAreaChanges(OnSafeAreaChanged);
    }

    private void BaseContentPageUnloaded(object sender, EventArgs e)
    {
        this.Window?.UnsubscribeFromSafeAreaChanges(OnSafeAreaChanged);
    }

    protected virtual void OnSafeAreaChanged(Thickness safeArea)
    {
        // Only apply safe-area padding when NOT inside AppShell
        // AppShell handles its own safe-area via SimpleNavigationHost padding
        if (Shell.Current == null)
        {
            this.Padding = safeArea;
        }
        else
        {
            // Inside Shell: only apply left/right, top/bottom handled by AppShell
            this.Padding = new Thickness(safeArea.Left, 0, safeArea.Right, 0);
        }
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        /*var mainScrollObj = this.Content;

        if (mainScrollObj != null)
        {
            if (mainScrollObj is Microsoft.Maui.Controls.ScrollView)
            {
                ((Microsoft.Maui.Controls.ScrollView)mainScrollObj).ScrollToAsync(0, 0, false);
            }
        }*/

        if (BindingContext is IBasePageViewModel viewModel)
        {
            viewModel.OnNavigatedToAsync();
        }
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

        if (BindingContext is IBasePageViewModel viewModel)
        {
            viewModel.OnNavigatedFromAsync();
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is IBasePageViewModel viewModel)
        {
            viewModel.OnAppearingAsync();
        }
    }


    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null
            || !query.TryGetValue(NavigationService.ParametersKey, out var value)
            || value is not IParameters incomingParameters)
        {
            return;
        }

        parameters = incomingParameters;

        if (BindingContext is IBasePageViewModel viewModel)
        {
            viewModel.OnApplyParametersAsync(parameters);
        }
    }


}
