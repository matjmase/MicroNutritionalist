using MicroNutritionalist.Db;
using MicroNutritionalist.ViewModels;
using MicroNutritionalist.ViewModels.Analytic;
using MicroNutritionalist.ViewModels.Analytic.Average;
using MicroNutritionalist.ViewModels.Analytic.Sum;
using MicroNutritionalist.ViewModels.Event;
using MicroNutritionalist.ViewModels.Event.Delete;
using MicroNutritionalist.ViewModels.Event.Edit;
using MicroNutritionalist.ViewModels.Event.Search;
using MicroNutritionalist.ViewModels.Product;
using MicroNutritionalist.ViewModels.Product.Delete;
using MicroNutritionalist.ViewModels.Product.Edit;
using MicroNutritionalist.ViewModels.Product.Search;
using MicroNutritionalist.Views;
using MicroNutritionalist.Views.Analytic;
using MicroNutritionalist.Views.Analytic.Average;
using MicroNutritionalist.Views.Analytic.Sum;
using MicroNutritionalist.Views.Event;
using MicroNutritionalist.Views.Event.Delete;
using MicroNutritionalist.Views.Event.Edit;
using MicroNutritionalist.Views.Event.Search;
using MicroNutritionalist.Views.Product;
using MicroNutritionalist.Views.Product.Delete;
using MicroNutritionalist.Views.Product.Edit;
using MicroNutritionalist.Views.Product.Search;
using Prism;
using Prism.Ioc;
using System;
using System.IO;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace MicroNutritionalist
{
    public partial class App
    {
        private static NutritionalistDatabase database;

        public static NutritionalistDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new NutritionalistDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Nutritionalist.db3"));
                }
                return database;
            }
        }

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();

            containerRegistry.RegisterForNavigation<AddOrEditProductPage, AddOrEditProductViewModel>();
            containerRegistry.RegisterForNavigation<ProductSearchPage, ProductSearchViewModel>();
            containerRegistry.RegisterForNavigation<ProductEditPage, ProductEditViewModel>();
            containerRegistry.RegisterForNavigation<NutritionSearchPage, NutritionSearchViewModel>();
            containerRegistry.RegisterForNavigation<NutritionAmountDetailsPage, NutritionAmountDetailsViewModel>();
            containerRegistry.RegisterForNavigation<ProductDeletePage, ProductDeleteViewModel>();
            containerRegistry.RegisterForNavigation<NutritionDeletePage, NutritionDeleteViewModel>();

            containerRegistry.RegisterForNavigation<AddEditDeleteEventPage, AddEditDeleteEventViewModel>();
            containerRegistry.RegisterForNavigation<EditConsumptionEventPage, EditConsumptionEventViewModel>();
            containerRegistry.RegisterForNavigation<SearchConsumptionEventPage, SearchConsumptionEventViewModel>();
            containerRegistry.RegisterForNavigation<DeleteConsumptionEventPage, DeleteConsumptionEventViewModel>();

            containerRegistry.RegisterForNavigation<AnalyticsPickingPage, AnalyticsPickingViewModel>();
            containerRegistry.RegisterForNavigation<AnalyticsSumPage, AnalyticsSumViewModel>();
            containerRegistry.RegisterForNavigation<AnalyticsAveragePage, AnalyticsAverageViewModel>();
        }
    }
}
