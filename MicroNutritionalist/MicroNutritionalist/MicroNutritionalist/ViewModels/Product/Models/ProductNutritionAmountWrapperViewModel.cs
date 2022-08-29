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
    public class ProductNutritionAmountWrapperViewModel : INotifyPropertyChanged
    {
        public ProductNutritionAmount InnerAmount;
        private Func<ProductNutritionAmountWrapperViewModel, Task> _selected;
        private Func<ProductNutritionAmountWrapperViewModel, Task> _remove;


        public int AmountMg
        {
            get => InnerAmount.AmountMg;
            set
            {
                InnerAmount.AmountMg = value;
                NotifyPropertyChanged();
            }
        }

        public Command SelectClicked => new Command(OnSelectClicked);
        public Command RemoveClicked => new Command(OnRemoveClicked);

        public ProductNutritionAmountWrapperViewModel(ProductNutritionAmount innerAmount, Func<ProductNutritionAmountWrapperViewModel, Task> selected, Func<ProductNutritionAmountWrapperViewModel, Task> remove)
        {
            InnerAmount = innerAmount;
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
