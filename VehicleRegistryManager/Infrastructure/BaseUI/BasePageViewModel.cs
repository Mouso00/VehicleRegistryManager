using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers.Commands;

namespace VehicleRegistryManager.Infrastructure.BaseUI
{
    public partial class BasePageViewModel : BaseNotifyPropertyChanged, IBasePageViewModel, ITransient
    {
        protected INavigationService NavigationService { get; private set; }
        public ICommand BackCommand { get; private set; }

        public IParameters? Parameters { get; set; }

        public BasePageViewModel(IDependencyObj dependencyObj)
        {
            NavigationService = dependencyObj.NavigationService;

            BackCommand = new AsyncCommand(NavigateBackAsync);
        }

       

        public async virtual Task NavigateBackAsync()
        {
            await NavigationService.NavigateBackAsync();
        }

        // Shows a Yes/No dialog and returns true if the user confirmed.
        protected async Task<bool> ConfirmAsync(string title, string message)
        {
            var page = Application.Current?.Windows.Count > 0
                ? Application.Current.Windows[0].Page
                : null;

            return page != null && await page.DisplayAlert(title, message, "Yes", "No");
        }

        // Runs an async operation and shows a friendly alert if anything fails,
        // so a thrown exception never crashes the app.
        protected async Task RunSafeAsync(Func<Task> action, string? errorMessage = null)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                var page = Application.Current?.Windows.Count > 0
                    ? Application.Current.Windows[0].Page
                    : null;

                if (page != null)
                    await page.DisplayAlert("Something went wrong", errorMessage ?? ex.Message, "OK");
            }
        }

        public virtual Task InitializeAsync() => Task.CompletedTask;

        public virtual Task OnNavigatedToAsync() => Task.CompletedTask;

        public virtual Task OnNavigatedFromAsync() => Task.CompletedTask;

        public virtual Task OnApplyParametersAsync(IParameters? parameters) => Task.CompletedTask;

        public virtual Task OnAppearingAsync() => Task.CompletedTask;

        public virtual Task OnDisappearingAsync() => Task.CompletedTask;
    }
}
