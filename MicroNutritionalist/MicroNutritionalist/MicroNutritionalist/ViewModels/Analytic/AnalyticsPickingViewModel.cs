using MicroNutritionalist.Views.Analytic.Average;
using MicroNutritionalist.Views.Analytic.Sum;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MicroNutritionalist.ViewModels.Analytic
{
    public class AnalyticsPickingViewModel : ViewModelBase
    {
        public Command SumClicked => new Command(OnSumClicked);
        public Command AverageClicked => new Command(OnAverageClicked);


        public AnalyticsPickingViewModel(INavigationService navigationService, IEventAggregator ea) : base(navigationService, ea)
        {
        }


        private async void OnSumClicked()
        {
            await NavigationService.NavigateAsync(nameof(AnalyticsSumPage));
        }

        private async void OnAverageClicked()
        {
            await NavigationService.NavigateAsync(nameof(AnalyticsAveragePage));
        }

    }
}
