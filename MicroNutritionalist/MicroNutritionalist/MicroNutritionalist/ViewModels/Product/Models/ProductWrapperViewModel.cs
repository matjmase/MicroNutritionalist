using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MicroNutritionalist.ViewModels.Product.Models
{
    public class ProductWrapperViewModel : INotifyPropertyChanged
    {
        public Db.Models.Product InnerProduct;
        private Func<ProductWrapperViewModel, Task> _selected;
        private Func<ProductWrapperViewModel, Task> _remove;

        public string Name
        {
            get => InnerProduct.Name;
            set
            {
                InnerProduct.Name = value;
                NotifyPropertyChanged();
            }
        }
        public int Calories
        {
            get => InnerProduct.Calories;
            set
            {
                InnerProduct.Calories = value;
                NotifyPropertyChanged();
            }
        }
        public string ServingDescription
        {
            get => InnerProduct.ServingDescription;
            set
            {
                InnerProduct.ServingDescription = value;
                NotifyPropertyChanged();
            }
        }

        public Command SelectClicked => new Command(OnSelectClicked);
        public Command RemoveClicked => new Command(OnRemoveClicked);

        public ProductWrapperViewModel(Db.Models.Product prod, Func<ProductWrapperViewModel, Task> selected, Func<ProductWrapperViewModel, Task> remove)
        {
            InnerProduct = prod;
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
