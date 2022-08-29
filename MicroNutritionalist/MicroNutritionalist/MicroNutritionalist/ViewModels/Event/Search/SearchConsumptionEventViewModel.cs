using MicroNutritionalist.Events;
using MicroNutritionalist.ViewModels.Event.Models;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroNutritionalist.ViewModels.Event.Search
{
    public class SearchConsumptionEventViewModel : ViewModelBase
    {
        private DateTime _start = DateTime.Now.AddDays(-1);
        private DateTime _end = DateTime.Now;

        public DateTime Start
        {
            get => _start;
            set
            {
                SetProperty(ref _start, value.Add(_start.TimeOfDay));

                FilterConsumeEvents();
            }
        }
        public DateTime End
        {
            get => _end;
            set
            {
                SetProperty(ref _end, value.Add(_end.TimeOfDay));

                FilterConsumeEvents();
            }
        }

        public TimeSpan StartTime
        {
            get
            {
                return _start.TimeOfDay;
            }
            set
            {
                _start = _start.Add(-_start.TimeOfDay);
                _start = _start.Add(value);

                RaisePropertyChanged();
                FilterConsumeEvents();
            }
        }
        public TimeSpan EndTime
        {
            get
            {
                return _end.TimeOfDay;
            }
            set
            {
                _end = End.Add(-_end.TimeOfDay);
                _end = End.Add(value);

                RaisePropertyChanged();
                FilterConsumeEvents();
            }
        }

        private void FilterConsumeEvents()
        {
            ConsumeEvents.Clear();
            foreach (var item in _totalEvents.Where(e => e.InnerEvent.Time >= _start && e.InnerEvent.Time <= _end))
            {
                ConsumeEvents.Add(item);
            }
        }

        public ObservableCollection<ConsumptionEventWrapperViewModel> ConsumeEvents { get; set; } = new ObservableCollection<ConsumptionEventWrapperViewModel>();
        private List<ConsumptionEventWrapperViewModel> _totalEvents = new List<ConsumptionEventWrapperViewModel>();

        public SearchConsumptionEventViewModel(INavigationService navigationService, IEventAggregator ea) : base(navigationService, ea)
        {
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            var consumeEvents = await App.Database.GetAllConsumptionEvent();

            ConsumeEvents.Clear();
            _totalEvents.Clear();
            foreach (var consumeEvent in consumeEvents)
            {
                var toAdd = new ConsumptionEventWrapperViewModel(consumeEvent, ItemSelected, null);

                _totalEvents.Add(toAdd);
                ConsumeEvents.Add(toAdd);
            }

            FilterConsumeEvents();
        }

        private async Task ItemSelected(ConsumptionEventWrapperViewModel item)
        {
            await NavigationService.GoBackAsync();
            EventAggregator.GetEvent<ConsumptionEventSelected>().Publish(item.InnerEvent);
        }
    }
}
