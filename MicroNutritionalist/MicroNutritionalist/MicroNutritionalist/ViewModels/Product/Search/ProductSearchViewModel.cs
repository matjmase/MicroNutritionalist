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
    public class ProductSearchViewModel : ViewModelBase
    {
        private string _searchQuery = "";

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                SetProperty(ref _searchQuery, value);

                if(string.IsNullOrEmpty(_searchQuery))
                {
                    Products.Clear();
                    foreach(var item in _totalProducts)
                    {
                        Products.Add(item);
                    }
                }
                else
                {
                    Products.Clear();
                    foreach (var item in _totalProducts.Where(e => e.InnerProduct.Name.ToLower().StartsWith(_searchQuery.ToLower())))
                    {
                        Products.Add(item);
                    }
                }
            }
        }

        public ObservableCollection<ProductWrapperViewModel> Products { get; set; } = new ObservableCollection<ProductWrapperViewModel>();
        private List<ProductWrapperViewModel> _totalProducts = new List<ProductWrapperViewModel>();

        public ProductSearchViewModel(INavigationService navigationService, IEventAggregator ea) : base(navigationService, ea)
        {
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            var products = await App.Database.GetAllProducts();

            Products.Clear();
            _totalProducts.Clear();
            foreach (var prod in products)
            {
                var toAdd = new ProductWrapperViewModel(prod, ItemSelected, null);

                _totalProducts.Add(toAdd);
                Products.Add(toAdd);
            }
        }

        private async Task ItemSelected(ProductWrapperViewModel item)
        {
            await NavigationService.GoBackAsync();
            EventAggregator.GetEvent<ProductSelected>().Publish(item.InnerProduct);
        }
    }
}
