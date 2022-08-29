using MicroNutritionalist.Db.Models;
using MicroNutritionalist.Events;
using MicroNutritionalist.ViewModels.Product.Models;
using MicroNutritionalist.ViewModels.Product.Search;
using MicroNutritionalist.Views.Product.Edit;
using MicroNutritionalist.Views.Product.Search;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MicroNutritionalist.ViewModels.Product.Edit
{
    public class ProductEditViewModel : ViewModelBase
    {
        public const string ParameterInitialProduct = "InitialProduct";

        private ProductWrapperViewModel _mainProduct = new ProductWrapperViewModel(new Db.Models.Product(), null, null);

        public ProductWrapperViewModel MainProduct
        {
            get => _mainProduct;
            set
            {
                SetProperty(ref _mainProduct, value);
            }
        }

        public ObservableCollection<NutritionWrapperViewModel> Nutrients { get; set; } = new ObservableCollection<NutritionWrapperViewModel>();
        private List<ProductNutritionAmount> _amounts = new List<ProductNutritionAmount>();
        private List<ProductNutritionAmount> _amountsToRemove = new List<ProductNutritionAmount>();

        public Command AddNutrientClicked => new Command(OnAddNutrientClicked);
        public Command SaveChangesClicked => new Command(OnSaveChangesClicked);

        public ProductEditViewModel(INavigationService navigationService, IEventAggregator ea) : base(navigationService, ea)
        {
        }

        public async override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            EventAggregator.GetEvent<ProductNutritionAmountSelected>().Subscribe(AddAmountLoadNutrient);

            MainProduct = new ProductWrapperViewModel(parameters.GetValue<MicroNutritionalist.Db.Models.Product>(ParameterInitialProduct), null, null);

            await LoadAmounts();
            await LoadNutrients();
        }

        public override void Destroy()
        {
            EventAggregator.GetEvent<ProductNutritionAmountSelected>().Unsubscribe(AddAmountLoadNutrient);
            base.Destroy();
        }

        private async void AddAmountLoadNutrient(ProductNutritionAmount amount)
        {
            var found = _amounts.FirstOrDefault(e => e.ProductId == amount.ProductId && e.NutritionId == amount.NutritionId);

            if (found == null)
            {
                _amounts.Add(amount);
                await LoadNutrients();
            }
        }

        private async Task LoadAmounts()
        {
            _amounts = await App.Database.GetAllProductNutritionAmountByProduct(MainProduct.InnerProduct.Id);
        }

        private async Task LoadNutrients()
        {
            Nutrients.Clear();
            foreach (var item in _amounts)
            {
                var nutrition = await App.Database.GetNutrition(item.NutritionId);

                var toAdd = new NutritionWrapperViewModel(nutrition, OnNutrientDetailsClicked, OnNutrientRemoveClicked);

                Nutrients.Add(toAdd);
            }
        }

        private async void OnAddNutrientClicked()
        {
            var navParams = new NavigationParameters();

            navParams.Add(NutritionSearchViewModel.ParameterCurrentNutrients, Nutrients.Select(e => e.InnerNutrition));
            
            await NavigationService.NavigateAsync(nameof(NutritionSearchPage), navParams);
        }

        private async Task OnNutrientDetailsClicked(NutritionWrapperViewModel vm)
        {
            var amt = _amounts.First(e => e.NutritionId == vm.InnerNutrition.Id);

            var navParams = new NavigationParameters();
            navParams.Add(NutritionAmountDetailsViewModel.ParameterCurrentProduct, _mainProduct.InnerProduct);
            navParams.Add(NutritionAmountDetailsViewModel.ParameterProductNutritionAmount, amt);

            await NavigationService.NavigateAsync(nameof(NutritionAmountDetailsPage), navParams);
        }

        private async Task OnNutrientRemoveClicked(NutritionWrapperViewModel vm)
        {
            var amt = _amounts.First(e => e.NutritionId == vm.InnerNutrition.Id);

            _amounts.Remove(amt);
            _amountsToRemove.Add(amt);

            await LoadNutrients();
        }

        private async void OnSaveChangesClicked()
        {
            if (_mainProduct.InnerProduct.Id == 0)
                await App.Database.InsertProduct(_mainProduct.InnerProduct);
            else
                await App.Database.UpdateProduct(_mainProduct.InnerProduct);

            _mainProduct.InnerProduct = await App.Database.GetProductByName(_mainProduct.Name);

            foreach(var item in _amountsToRemove)
            {
                if (item.Id != 0)
                    await App.Database.DeleteProductNutritionAmount(item);
            }

            foreach (var item in _amounts)
            {
                if (item.Id == 0)
                {
                    item.ProductId = _mainProduct.InnerProduct.Id;
                    await App.Database.InsertProductNutritionAmount(item);
                }
                else
                    await App.Database.UpdateProductNutritionAmount(item);
            }

            await NavigationService.GoBackAsync();
        }
    }
}
