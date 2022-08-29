using MicroNutritionalist.Views.Analytic;
using MicroNutritionalist.Views.Event;
using MicroNutritionalist.Views.Product;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MicroNutritionalist.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public Command ProductClicked => new Command(OnProductClicked);
        public Command EventClicked => new Command(OnEventClicked);
        public Command AnalyticsClicked => new Command(OnAnalyticsClicked);

        public MainPageViewModel(INavigationService navigationService, IEventAggregator ea) : base(navigationService, ea)
        {
            Title = "Main Page";
        }

        private async void OnProductClicked()
        {
            await NavigationService.NavigateAsync(nameof(AddOrEditProductPage));
        }

        private async void OnEventClicked()
        {
            await NavigationService.NavigateAsync(nameof(AddEditDeleteEventPage)); 
        }

        private async void OnAnalyticsClicked()
        {
            await NavigationService.NavigateAsync(nameof(AnalyticsPickingPage));
        }
    }
}
