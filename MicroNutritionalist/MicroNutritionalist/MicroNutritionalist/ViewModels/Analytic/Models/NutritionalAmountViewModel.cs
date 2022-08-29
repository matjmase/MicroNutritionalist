using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MicroNutritionalist.ViewModels.Analytic.Models
{
    public class NutritionalAmountViewModel : INotifyPropertyChanged
    {
        private string _nutritionalName;
        private int _amountMg;

        public string NutritionalName
        {
            get => _nutritionalName;
            set
            {
                _nutritionalName = value;
                NotifyPropertyChanged();
            }
        }

        public int AmountMg
        {
            get => _amountMg;
            set
            {
                _amountMg = value;
                NotifyPropertyChanged();
            }
        }

        public NutritionalAmountViewModel(string nutritionalName, int amountMg)
        {
            NutritionalName = nutritionalName;
            AmountMg = amountMg;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
