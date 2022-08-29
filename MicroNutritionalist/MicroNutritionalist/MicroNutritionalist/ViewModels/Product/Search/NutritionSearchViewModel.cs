using MicroNutritionalist.Common.Formatting;
using MicroNutritionalist.Db.Models;
using MicroNutritionalist.Events;
using MicroNutritionalist.ViewModels.Product.Models;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MicroNutritionalist.ViewModels.Product.Search
{
    public class NutritionSearchViewModel : ViewModelBase
    {
        public const string ParameterCurrentNutrients = "CurrentNutrients";

        private string _searchQuery = "";

        private HashSet<int> _currentNutrients = new HashSet<int>();

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                SetProperty(ref _searchQuery, value);

                if (string.IsNullOrEmpty(_searchQuery))
                {
                    Nutrients.Clear();
                    foreach (var item in _allNutrients.Where(e => !_currentNutrients.Contains(e.InnerNutrition.Id)))
                    {
                        Nutrients.Add(item);
                    }
                }
                else
                {
                    Nutrients.Clear();
                    foreach (var item in _allNutrients.Where(e => !_currentNutrients.Contains(e.InnerNutrition.Id) && e.Name.StartsWith(NameFormatting.FormatName(_searchQuery))))
                    {
                        Nutrients.Add(item);
                    }
                }

                RaisePropertyChanged(nameof(IsUniqueValue));
            }
        }

        public bool IsUniqueValue => !string.IsNullOrEmpty(_searchQuery) && Nutrients.Count == 0;

        public Command AddNutrientClick => new Command(OnAddNutrientClick);

        public ObservableCollection<NutritionWrapperViewModel> Nutrients { get; set; } = new ObservableCollection<NutritionWrapperViewModel>();
        private List<NutritionWrapperViewModel> _allNutrients = new List<NutritionWrapperViewModel>();

        public NutritionSearchViewModel(INavigationService navigationService, IEventAggregator ea) : base(navigationService, ea)
        {
        }

        public async override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            var currNutrients = parameters.GetValue<IEnumerable<Nutrition>>(ParameterCurrentNutrients);

            _currentNutrients.Clear();
            foreach (var item in currNutrients)
                _currentNutrients.Add(item.Id);

            var nutrients = await App.Database.GetAllNutrition();

            _allNutrients.Clear();
            Nutrients.Clear();
            foreach (var item in nutrients)
            {
                var toAdd = new NutritionWrapperViewModel(item, ItemSelected, null);

                _allNutrients.Add(toAdd);

                if(!_currentNutrients.Contains(toAdd.InnerNutrition.Id))
                    Nutrients.Add(toAdd);
            }
        }

        private async Task ItemSelected(NutritionWrapperViewModel item)
        {
            EventAggregator.GetEvent<ProductNutritionAmountSelected>().Publish(new ProductNutritionAmount() { NutritionId = item.InnerNutrition.Id });
            await NavigationService.GoBackAsync();
        }

        private async void OnAddNutrientClick()
        {
            var newItem = new Nutrition() { Name = NameFormatting.FormatName(SearchQuery) };

            await App.Database.InsertNutrition(newItem);
            var dbItem = await App.Database.GetNutrition(newItem.Name);

            EventAggregator.GetEvent<ProductNutritionAmountSelected>().Publish(new ProductNutritionAmount() { NutritionId = dbItem.Id});
            await NavigationService.GoBackAsync();

        }
    }
}
