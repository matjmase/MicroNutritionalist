using MicroNutritionalist.Db.Models;
using MicroNutritionalist.Events;
using MicroNutritionalist.ViewModels.Product.Models;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MicroNutritionalist.ViewModels.Product.Edit
{
    public class NutritionAmountDetailsViewModel : ViewModelBase
    {
        public const string ParameterCurrentProduct = "CurrentProduct";
        public const string ParameterProductNutritionAmount = "ProductNutritionAmount";

        private ProductWrapperViewModel _product;
        private NutritionWrapperViewModel _nutrition;
        private ProductNutritionAmountWrapperViewModel _amount;

        public ProductWrapperViewModel Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }
        public NutritionWrapperViewModel Nutrition
        {
            get => _nutrition;
            set => SetProperty(ref _nutrition, value);
        }


        public ProductNutritionAmountWrapperViewModel Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        public Command SaveChangesClicked => new Command(OnSaveChangesClicked);

        public NutritionAmountDetailsViewModel(INavigationService navigationService, IEventAggregator ea) : base(navigationService, ea)
        {
        }

        public async override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            var innerProduct = parameters.GetValue<Db.Models.Product>(ParameterCurrentProduct);
            Amount = new ProductNutritionAmountWrapperViewModel(parameters.GetValue<ProductNutritionAmount>(ParameterProductNutritionAmount), null, null);
            
            var innerNutrition = await App.Database.GetNutrition(Amount.InnerAmount.NutritionId);

            Product = new ProductWrapperViewModel(innerProduct, null, null);
            Nutrition = new NutritionWrapperViewModel(innerNutrition, null, null);
        }

        private async void OnSaveChangesClicked()
        {
            EventAggregator.GetEvent<ProductNutritionAmountSelected>().Publish(Amount.InnerAmount);
            await NavigationService.GoBackAsync();
        }
    }
}
