using System.Windows;
using NobleIPT2Domain.Commands;
using NobleIPT2Domain.Queries;
using Framework.Commands;
using Framework.Queries;
using Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NobleIPT2WPF.Services;
using NobleIPT2WPF.Stores;
using NobleIPT2WPF.ViewModels;
using NobleIPT2WPF.Views;

namespace NobleIPT2WPF
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider = null!;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var services = new ServiceCollection();

            // Configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            services.AddSingleton<IConfiguration>(config);

            // Repository & DB setup
            services.AddSingleton<Repository>();
            services.AddSingleton<DatabaseAutoSetup>();

            // NobleIPT2Domain commands & queries
            services.AddTransient<ICreateCommand, CreateCommand>();
            services.AddTransient<IUpdateCommand, UpdateCommand>();
            services.AddTransient<IDeleteCommand, DeleteCommand>();
            services.AddTransient<IGetAllSensors, GetAllSensors>();
            services.AddTransient<IReadSensorsById, ReadSensorsById>();

            // Navigation
            services.AddSingleton<NavigationStore>();

            services.AddTransient<HomeViewModel>(sp => new HomeViewModel(
                new NavigationService<AddSensorsViewModel>(
                    sp.GetRequiredService<NavigationStore>(),
                    () => sp.GetRequiredService<AddSensorsViewModel>()
                )
            ));

            services.AddTransient<AddSensorsViewModel>(sp => new AddSensorsViewModel(
                sp.GetRequiredService<ICreateCommand>(),
                sp.GetRequiredService<IGetAllSensors>(),
                sp.GetRequiredService<IUpdateCommand>(),
                sp.GetRequiredService<IDeleteCommand>(),
                new NavigationService<HomeViewModel>(
                    sp.GetRequiredService<NavigationStore>(),
                    () => sp.GetRequiredService<HomeViewModel>()
                )
            ));

            services.AddSingleton<MainViewModel>(sp => new MainViewModel(
                sp.GetRequiredService<NavigationStore>()
            ));

            services.AddSingleton<MainView>(sp =>
            {
                var window = new MainView();
                window.DataContext = sp.GetRequiredService<MainViewModel>();
                return window;
            });

            _serviceProvider = services.BuildServiceProvider();

            // Show window immediately
            var navStore = _serviceProvider.GetRequiredService<NavigationStore>();
            navStore.CurrentViewModel = _serviceProvider.GetRequiredService<HomeViewModel>();

            var mainWindow = _serviceProvider.GetRequiredService<MainView>();
            mainWindow.Show();

            // Run DB setup in background, show error if it fails
            var dbSetup = _serviceProvider.GetRequiredService<DatabaseAutoSetup>();
            _ = dbSetup.EnsureDatabaseSetupAsync().ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        System.Windows.MessageBox.Show(
                            $"Database setup failed:\n{t.Exception?.InnerException?.Message ?? t.Exception?.Message}",
                            "DB Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    });
                }
            });
        }
    }
}
