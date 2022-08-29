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
    public class NutritionDeleteViewModel : ViewModelBase
    {
        private IPageDialogService _dialogService;

        public ObservableCollection<NutritionWrapperViewModel> NutritionItems { get; set; } = new ObservableCollection<NutritionWrapperViewModel>();

        public NutritionDeleteViewModel(INavigationService navigationService, IEventAggregator ea, IPageDialogService dialogService) : base(navigationService, ea)
        {
            _dialogService = dialogService;
        }

        public async override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            await LoadFromDatabase();
        }

        private async Task LoadFromDatabase()
        {
            var items = await App.Database.GetAllNutrition();

            NutritionItems.Clear();
            foreach (var item in items)
            {
                NutritionItems.Add(new NutritionWrapperViewModel(item, null, RemoveItemClicked));
            }
        }

        private async Task RemoveItemClicked(NutritionWrapperViewModel item)
        {
            if (await _dialogService.DisplayAlertAsync("Confirm Delete", "Are you sure you want to delete the nutrition as well as all associations with the products?", "Yes", "No"))
            {
                await App.Database.DeleteNutritionCascade(item.InnerNutrition);
                await LoadFromDatabase();
            }
        }
    }
}
