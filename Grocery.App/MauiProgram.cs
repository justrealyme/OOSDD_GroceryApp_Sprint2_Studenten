using Grocery.App.ViewModels; 
using Grocery.App.Views;
using Grocery.Core.Services;
using Grocery.Core.Data.Repositories;
using Grocery.Core.Interfaces.Services; 
using Grocery.Core.Interfaces.Repositories; 
using Microsoft.Extensions.Logging; 

namespace Grocery.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            // Existing services
            builder.Services.AddSingleton<IGroceryListService, GroceryListService>();
            builder.Services.AddSingleton<IGroceryListItemsService, GroceryListItemsService>();
            builder.Services.AddSingleton<IProductService, ProductService>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IClientService, ClientService>();

            builder.Services.AddSingleton<IGroceryListRepository, GroceryListRepository>();
            builder.Services.AddSingleton<IGroceryListItemsRepository, GroceryListItemsRepository>();
            builder.Services.AddSingleton<IProductRepository, ProductRepository>();
            builder.Services.AddSingleton<IClientRepository, ClientRepository>();
            builder.Services.AddSingleton<GlobalViewModel>();

            // Update deze regel om GlobalViewModel toe te voegen
            builder.Services.AddTransient<GroceryListsView>(provider =>
            {
                var groceryListService = provider.GetRequiredService<IGroceryListService>();
                var globalViewModel = provider.GetRequiredService<GlobalViewModel>();
                var viewModel = new GroceryListViewModel(groceryListService, globalViewModel);
                return new GroceryListsView(viewModel);
            });

            // Of simpeler: voeg GlobalViewModel toe aan de constructor
            builder.Services.AddTransient<GroceryListViewModel>();
            builder.Services.AddTransient<GroceryListsView>();

            builder.Services.AddTransient<GroceryListItemsView>().AddTransient<GroceryListItemsViewModel>();
            builder.Services.AddTransient<ProductView>().AddTransient<ProductViewModel>();
            builder.Services.AddTransient<ChangeColorView>().AddTransient<ChangeColorViewModel>();
            builder.Services.AddTransient<LoginView>().AddTransient<LoginViewModel>();
            return builder.Build();
        }
    }
}
