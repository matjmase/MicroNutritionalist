using MicroNutritionalist.Db.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MicroNutritionalist.ViewModels.Event.Models
{
    public class ConsumptionEventWrapperViewModel : INotifyPropertyChanged
    {
        public ConsumptionEvent InnerEvent;
        private Func<ConsumptionEventWrapperViewModel, Task> _selected;
        private Func<ConsumptionEventWrapperViewModel, Task> _remove;

        public DateTime Date
        {
            get => InnerEvent.Time;
            set
            {
                InnerEvent.Time = value;
                NotifyPropertyChanged();
            }
        }


        public TimeSpan Time
        {
            get
            {
                return new TimeSpan(InnerEvent.Time.Hour, InnerEvent.Time.Minute, InnerEvent.Time.Second);
            }
            set
            {
                InnerEvent.Time = InnerEvent.Time.AddHours(-InnerEvent.Time.Hour);
                InnerEvent.Time = InnerEvent.Time.AddMinutes(-InnerEvent.Time.Minute);
                InnerEvent.Time = InnerEvent.Time.AddSeconds(-InnerEvent.Time.Second);
                InnerEvent.Time = InnerEvent.Time.Add(value);

                NotifyPropertyChanged();
            }
        }

        public double Proportion
        {
            get => InnerEvent.Proportion;
            set
            {
                InnerEvent.Proportion = value;
                NotifyPropertyChanged();
            }
        }

        public Command SelectClicked => new Command(OnSelectClicked);
        public Command RemoveClicked => new Command(OnRemoveClicked);

        public ConsumptionEventWrapperViewModel(ConsumptionEvent consumeEvent, Func<ConsumptionEventWrapperViewModel, Task> selected, Func<ConsumptionEventWrapperViewModel, Task> remove)
        {
            InnerEvent = consumeEvent;
            _selected = selected;
            _remove = remove;
        }

        private async void OnSelectClicked()
        {
            await _selected(this);
        }

        private async void OnRemoveClicked()
        {
            await _remove(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
