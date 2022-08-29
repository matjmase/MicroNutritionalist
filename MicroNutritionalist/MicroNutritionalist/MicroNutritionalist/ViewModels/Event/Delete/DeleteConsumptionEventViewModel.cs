using MicroNutritionalist.ViewModels.Event.Models;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroNutritionalist.ViewModels.Event.Delete
{
    public class DeleteConsumptionEventViewModel : ViewModelBase
    {
        private IPageDialogService _dialogService;

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

        public DeleteConsumptionEventViewModel(INavigationService navigationService, IEventAggregator ea, IPageDialogService dialogService) : base(navigationService, ea)
        {
            _dialogService = dialogService;
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            await LoadEvents();
        }

        private async Task LoadEvents()
        {
            var consumeEvents = await App.Database.GetAllConsumptionEvent();

            ConsumeEvents.Clear();
            _totalEvents.Clear();
            foreach (var consumeEvent in consumeEvents)
            {
                var toAdd = new ConsumptionEventWrapperViewModel(consumeEvent, null, ItemRemoved);

                _totalEvents.Add(toAdd);
                ConsumeEvents.Add(toAdd);
            }

            FilterConsumeEvents();
        }

        private async Task ItemRemoved(ConsumptionEventWrapperViewModel arg)
        {
            if (await _dialogService.DisplayAlertAsync("Confirm Delete", "Are you sure you want to delete the event?", "Yes", "No"))
            {
                await App.Database.DeleteConsumptionEvent(arg.InnerEvent);
                await LoadEvents();
            }
        }
    }
}
