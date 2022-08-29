using MicroNutritionalist.ViewModels.Product.Models;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace MicroNutritionalist.ViewModels.Product.Delete
{
    public class ProductDeleteViewModel : ViewModelBase
    {
        private IPageDialogService _dialogService;

        public ObservableCollection<ProductWrapperViewModel> ProductItems { get; set; } = new ObservableCollection<ProductWrapperViewModel>();

        public ProductDeleteViewModel(INavigationService navigationService, IEventAggregator ea, IPageDialogService dialogService) : base(navigationService, ea)
        {
            _dialogService = dialogService;
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            await LoadFromDatabase();
        }
        private async Task LoadFromDatabase()
        {
            var items = await App.Database.GetAllProducts();

            ProductItems.Clear();
            foreach (var item in items)
            {
                ProductItems.Add(new ProductWrapperViewModel(item, null, RemoveItemClicked));
            }
        }

        private async Task RemoveItemClicked(ProductWrapperViewModel item)
        {
            if (await _dialogService.DisplayAlertAsync("Confirm Delete", "Are you sure you want to delete the product?", "Yes", "No"))
            {
                await App.Database.DeleteProductCascade(item.InnerProduct);
                await LoadFromDatabase();
            }
        }
    }
}
