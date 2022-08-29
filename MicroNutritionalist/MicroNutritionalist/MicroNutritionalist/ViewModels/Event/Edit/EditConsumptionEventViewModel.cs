using MicroNutritionalist.Db.Models;
using MicroNutritionalist.Events;
using MicroNutritionalist.ViewModels.Event.Models;
using MicroNutritionalist.ViewModels.Product.Models;
using MicroNutritionalist.Views.Product.Search;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MicroNutritionalist.ViewModels.Event.Edit
{
    public class EditConsumptionEventViewModel : ViewModelBase
    {
        public const string ParameterInitialEvent = "InitialEvent";

        private ConsumptionEventWrapperViewModel _consumeEvent;
        private ProductWrapperViewModel _product;

        public ConsumptionEventWrapperViewModel ConsumptionEvent
        {
            get => _consumeEvent;
            set => SetProperty(ref _consumeEvent, value);
        }

        public ProductWrapperViewModel Product
        {
            get => _product;
            set 
            {
                _product = value;
                RaisePropertyChanged();
                RaisePropertyChanged("ProductIsValid");
            }
        }

        public bool ProductIsValid => _product?.InnerProduct?.Id != null && _product?.InnerProduct?.Id != 0;

        public Command SelectProductClicked => new Command(OnSelectProductClicked);
        public Command SaveChangesClicked => new Command(OnSaveChangesClicked);

        public EditConsumptionEventViewModel(INavigationService navigationService, IEventAggregator ea) : base(navigationService, ea)
        {
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            var ev = parameters.GetValue<ConsumptionEvent>(ParameterInitialEvent);

            ConsumptionEvent = new ConsumptionEventWrapperViewModel(ev, null, null);
            if(ev.ProductId != 0)
            {
                var prod = await App.Database.GetProduct(ev.ProductId);
                Product = new ProductWrapperViewModel(prod, null, null);
            }
            else
            {
                Product = new ProductWrapperViewModel(new Db.Models.Product() { Id = 0, Calories = 0, Name = "", ServingDescription = "" }, null, null);
            }

            EventAggregator.GetEvent<ProductSelected>().Subscribe(SearchProductSelected);
        }

        private void SearchProductSelected(Db.Models.Product product)
        {
            ConsumptionEvent.InnerEvent.ProductId = product.Id;
            Product = new ProductWrapperViewModel(product, null, null);
        }

        public override void Destroy()
        {
            EventAggregator.GetEvent<ProductSelected>().Unsubscribe(SearchProductSelected);

            base.Destroy();
        }

        private async void OnSelectProductClicked()
        {
            await NavigationService.NavigateAsync(nameof(ProductSearchPage));
        }

        private async void OnSaveChangesClicked()
        {
            if (ConsumptionEvent.InnerEvent.Id == 0)
                await App.Database.InsertConsumptionEvent(ConsumptionEvent.InnerEvent);
            else
                await App.Database.UpdateConsumptionEvent(ConsumptionEvent.InnerEvent);

            await NavigationService.GoBackAsync();
        }
    }
}
