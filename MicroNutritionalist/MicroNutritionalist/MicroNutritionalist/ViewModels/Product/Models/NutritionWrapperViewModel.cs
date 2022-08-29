using MicroNutritionalist.Db.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MicroNutritionalist.ViewModels.Product.Models
{
    public class NutritionWrapperViewModel : INotifyPropertyChanged
    {
        public Nutrition InnerNutrition;
        private Func<NutritionWrapperViewModel, Task> _selected;
        private Func<NutritionWrapperViewModel, Task> _remove;


        public string Name
        {
            get => InnerNutrition.Name;
            set {
                InnerNutrition.Name = value;
                NotifyPropertyChanged();
            }
        }

        public Command SelectClicked => new Command(OnSelectClicked);
        public Command RemoveClicked => new Command(OnRemoveClicked);

        public NutritionWrapperViewModel(Nutrition innerNutrition, Func<NutritionWrapperViewModel, Task> selected, Func<NutritionWrapperViewModel, Task> remove)
        {
            InnerNutrition = innerNutrition;
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
