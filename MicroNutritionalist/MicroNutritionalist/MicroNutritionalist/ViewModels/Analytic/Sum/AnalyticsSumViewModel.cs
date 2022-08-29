using MicroNutritionalist.Db.Models;
using MicroNutritionalist.ViewModels.Analytic.Models;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MicroNutritionalist.ViewModels.Analytic.Sum
{
    public class AnalyticsSumViewModel : ViewModelBase
    {

        private DateTime _start = DateTime.Now.AddDays(-1);
        private DateTime _end = DateTime.Now;

        private int _totalCalories;

        public DateTime Start
        {
            get => _start;
            set
            {
                SetProperty(ref _start, value.Add(_start.TimeOfDay));

                Device.InvokeOnMainThreadAsync(LoadNutritionAmountsVoid);
            }
        }
        public DateTime End
        {
            get => _end;
            set
            {
                SetProperty(ref _end, value.Add(_end.TimeOfDay));

                Device.InvokeOnMainThreadAsync(LoadNutritionAmountsVoid);
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

                Device.InvokeOnMainThreadAsync(LoadNutritionAmountsVoid);
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

                _end = _end.Add(-_end.TimeOfDay);
                _end = _end.Add(value);

                RaisePropertyChanged();

                Device.InvokeOnMainThreadAsync(LoadNutritionAmountsVoid);
            }
        }

        public int TotalCalories
        {
            get => _totalCalories;
            set
            {
                SetProperty(ref _totalCalories, value);
            }
        }

        public ObservableCollection<NutritionalAmountViewModel> NutritionAmounts { get; set; } = new ObservableCollection<NutritionalAmountViewModel>();

        public AnalyticsSumViewModel(INavigationService navigationService, IEventAggregator ea) : base(navigationService, ea)
        {
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            await LoadNutritionAmounts();
        }

        private async void LoadNutritionAmountsVoid()
        {
            await LoadNutritionAmounts();
        }

        private async Task LoadNutritionAmounts()
        {
            var allEvents = await App.Database.GetAllConsumptionEventByTimeWindow(Start, End);

            var products = new Dictionary<int, MicroNutritionalist.Db.Models.Product>();
            foreach (var ev in allEvents)
            {
                if (!products.ContainsKey(ev.ProductId))
                    products.Add(ev.ProductId, await App.Database.GetProduct(ev.ProductId));
            }

            var amounts = new Dictionary<int, ProductNutritionAmount>();
            foreach (var product in products)
            {
                var subAmounts = await App.Database.GetAllProductNutritionAmountByProduct(product.Key);
                foreach (var amt in subAmounts)
                {
                    if (!amounts.ContainsKey(amt.Id))
                    {
                        amounts.Add(amt.Id, amt);
                    }
                }
            }

            var nutritions = new Dictionary<int, Nutrition>();
            foreach (var amt in amounts)
            {
                if (!nutritions.ContainsKey(amt.Value.NutritionId))
                {
                    nutritions.Add(amt.Value.NutritionId, await App.Database.GetNutrition(amt.Value.NutritionId));
                }
            }

            var nutritionAmounts = new Dictionary<string, int>();
            TotalCalories = 0;

            foreach (var ev in allEvents)
            {
                TotalCalories += products[ev.ProductId].Calories;

                foreach (var amt in amounts.Values.Where(e => e.ProductId == ev.ProductId))
                {
                    if (!nutritionAmounts.ContainsKey(nutritions[amt.NutritionId].Name))
                    {
                        nutritionAmounts.Add(nutritions[amt.NutritionId].Name, 0);
                    }

                    nutritionAmounts[nutritions[amt.NutritionId].Name] += (int)Math.Round(ev.Proportion * amt.AmountMg);
                }
            }

            NutritionAmounts.Clear();
            foreach (var nutriAmt in nutritionAmounts)
            {
                NutritionAmounts.Add(new NutritionalAmountViewModel(nutriAmt.Key, nutriAmt.Value));
            }
        }
    }
}
