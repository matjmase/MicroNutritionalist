using MicroNutritionalist.Db.Models;
using MicroNutritionalist.Events;
using MicroNutritionalist.ViewModels.Event.Edit;
using MicroNutritionalist.Views.Event.Delete;
using MicroNutritionalist.Views.Event.Edit;
using MicroNutritionalist.Views.Event.Search;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MicroNutritionalist.ViewModels.Event
{
    public class AddEditDeleteEventViewModel : ViewModelBase
    {
        public Command AddEventClicked => new Command(OnAddEventClicked);
        public Command EditEventClicked => new Command(OnEditEventClicked);
        public Command DeleteEventClicked => new Command(OnDeleteEventClicked);

        public AddEditDeleteEventViewModel(INavigationService navigationService, IEventAggregator ea) : base(navigationService, ea)
        {
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            EventAggregator.GetEvent<ConsumptionEventSelected>().Subscribe(EventSelected);
        }

        private async void EventSelected(ConsumptionEvent obj)
        {
            var navParams = new NavigationParameters();
            navParams.Add(EditConsumptionEventViewModel.ParameterInitialEvent, obj);

            await NavigationService.NavigateAsync(nameof(EditConsumptionEventPage), navParams);
        }

        public override void Destroy()
        {
            EventAggregator.GetEvent<ConsumptionEventSelected>().Subscribe(EventSelected);
            base.Destroy();
        }

        private async void OnAddEventClicked()
        {
            var navParams = new NavigationParameters();
            navParams.Add(EditConsumptionEventViewModel.ParameterInitialEvent, new ConsumptionEvent() { Time = DateTime.Now });

            await NavigationService.NavigateAsync(nameof(EditConsumptionEventPage), navParams);
        }
        private async void OnEditEventClicked()
        {
            await NavigationService.NavigateAsync(nameof(SearchConsumptionEventPage));
        }
        private async void OnDeleteEventClicked()
        {
            await NavigationService.NavigateAsync(nameof(DeleteConsumptionEventPage));
        }
    }
}
