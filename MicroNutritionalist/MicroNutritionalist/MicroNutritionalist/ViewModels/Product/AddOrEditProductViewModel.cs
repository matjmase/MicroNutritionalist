using MicroNutritionalist.Events;
using MicroNutritionalist.ViewModels.Product.Edit;
using MicroNutritionalist.Views.Product;
using MicroNutritionalist.Views.Product.Delete;
using MicroNutritionalist.Views.Product.Edit;
using MicroNutritionalist.Views.Product.Search;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MicroNutritionalist.ViewModels.Product
{
    public class AddOrEditProductViewModel : ViewModelBase
    {
        public Command AddClicked => new Command(OnAddClicked);
        public Command EditClicked => new Command(OnEditClicked);
        public Command DeleteProductClicked => new Command(OnDeleteProductClicked);
        public Command DeleteNutritionClicked => new Command(OnDeleteNutritionClicked);

        public AddOrEditProductViewModel(INavigationService navigationService, IEventAggregator ea) : base(navigationService, ea)
        {
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            EventAggregator.GetEvent<ProductSelected>().Subscribe(ProductSelected);
        }

        public override void Destroy()
        {
            EventAggregator.GetEvent<ProductSelected>().Unsubscribe(ProductSelected);

            base.Destroy();
        }

        private async void ProductSelected(MicroNutritionalist.Db.Models.Product prod)
        {
            var navParameters = new NavigationParameters();
            navParameters.Add(ProductEditViewModel.ParameterInitialProduct, prod);

            await NavigationService.NavigateAsync(nameof(ProductEditPage), navParameters);
        }

        private async void OnAddClicked()
        {
            var navParameters = new NavigationParameters();
            navParameters.Add(ProductEditViewModel.ParameterInitialProduct, new MicroNutritionalist.Db.Models.Product()
            {
                Id = 0,
                Name = String.Empty,
                Calories = 0
            });

            await NavigationService.NavigateAsync(nameof(ProductEditPage), navParameters);
        }

        private async void OnEditClicked()
        {
            await NavigationService.NavigateAsync(nameof(ProductSearchPage));
        }

        private async void OnDeleteProductClicked()
        {
            await NavigationService.NavigateAsync(nameof(ProductDeletePage));
        }

        private async void OnDeleteNutritionClicked()
        {
            await NavigationService.NavigateAsync(nameof(NutritionDeletePage));
        }
    }
}
