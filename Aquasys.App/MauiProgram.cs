
using Aquasys.App.Core.Data;
using Aquasys.App.Core.Services;
using Aquasys.App.MVVM.ViewModels.Login;
using Aquasys.App.MVVM.ViewModels.MainPage;
using Aquasys.App.MVVM.ViewModels.Vessel;
using Aquasys.App.MVVM.ViewModels.Vessel.Tabs;
using Aquasys.App.MVVM.Views.Login;
using Aquasys.App.MVVM.Views.MainPage;
using Aquasys.App.MVVM.Views.Vessel;
using Aquasys.App.MVVM.Views.Vessel.Tabs;
using Aquasys.Core.Entities;
using Aquasys.Core.Sync;
using Aquasys.Reports;
using Aquasys.Reports.Interfaces;
using Aquasys.Reports.Services;
using Aquasys.Reports.Templates;
using CommunityToolkit.Maui;
using DevExpress.Maui;
using InputKit.Handlers;
using Microsoft.Extensions.Logging;
using RGPopup.Maui.Extensions;
using Syncfusion.Licensing;

namespace Aquasys.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                .UseDevExpress()
                .UseDevExpressCharts()
                .UseDevExpressCollectionView()
                .UseDevExpressControls()
                .UseDevExpressDataGrid()
                .UseDevExpressEditors()
                .UseDevExpressScheduler()
            .UseMauiRGPopup()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Quicksand-Light.ttf", "Quicksand300Font");
                fonts.AddFont("Quicksand-Regular.ttf", "Quicksand400Font");
                fonts.AddFont("Quicksand-Medium.ttf", "Quicksand500Font");
                fonts.AddFont("Quicksand-SemiBold.ttf", "Quicksand600Font");
                fonts.AddFont("Quicksand-Bold.ttf", "Quicksand700Font");

                fonts.AddFont("FontAwesome6Brands.otf", "FontAwesomeBrands");
                fonts.AddFont("FontAwesome6Regular.otf", "FontAwesomeRegular");
                fonts.AddFont("FontAwesome6Solid.otf", "FontAwesomeSolid");

                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddInputKitHandlers();
            });
#if DEBUG
            builder.Logging.AddDebug();
#endif

            DevExpress.Maui.Charts.Initializer.Init();
            DevExpress.Maui.CollectionView.Initializer.Init();
            DevExpress.Maui.Controls.Initializer.Init();
            DevExpress.Maui.Editors.Initializer.Init();
            DevExpress.Maui.Scheduler.Initializer.Init();

            builder.Services.AddSingleton(new HttpClient
            {
                // IMPORTANTE: Use o endereço da sua API. Para testes locais, pode ser necessário um ajuste.
                //BaseAddress = new Uri("https://localhost:7182") -- smart físico
                BaseAddress = new Uri("http://10.0.2.2:5270")
                //BaseAddress = new Uri("https://10.0.2.2:7182")
            });

            builder.Services.AddSingleton<ISyncService, SyncService>();

            builder.Services.AddTransient<IAuthService, AuthService>();

            builder.Services.AddSingleton<ILocalRepository<User>, LocalRepository<User>>();
            builder.Services.AddSingleton<ILocalRepository<Vessel>, LocalRepository<Vessel>>();
            builder.Services.AddSingleton<ILocalRepository<Hold>, LocalRepository<Hold>>();
            builder.Services.AddSingleton<ILocalRepository<HoldInspectionImage>, LocalRepository<HoldInspectionImage>>();
            builder.Services.AddSingleton<ILocalRepository<HoldInspection>, LocalRepository<HoldInspection>>();
            builder.Services.AddSingleton<ILocalRepository<VesselImage>, LocalRepository<VesselImage>>();
            builder.Services.AddSingleton<ReportGeneratorService>();

            // Report
            SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF5cX2BCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWH5cdXVQRGBZUUNwWUpWYEg=");
            var assemblies = new[] { typeof(VesselReport).Assembly };
            builder.Services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo<IReportTemplate>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

            builder.Services.AddSingleton<ReportGeneratorService>();

            // Login
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<CreateAccountViewModel>();

            //Main Page
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<OptionsViewModel>();

            //Vessel e Tabs
            builder.Services.AddTransient<VesselRegistrationViewModel>();
            builder.Services.AddTransient<VesselMainViewModel>();
            builder.Services.AddTransient<VesselListViewModel>();
            builder.Services.AddTransient<VesselHoldRegistrationTabViewModel>();
            builder.Services.AddTransient<VesselRegistrationTabViewModel>();
            builder.Services.AddTransient<HoldInspectionImageViewModel>();
            builder.Services.AddTransient<HoldInspectionViewModel>();
            builder.Services.AddTransient<HoldViewModel>();
            builder.Services.AddTransient<VesselImageViewModel>();

            // Login
            builder.Services.AddTransient<CreateAccountPage>();
            builder.Services.AddTransient<LoadingPage>();
            builder.Services.AddTransient<LoginPage>();

            // MainPage
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<OptionsPage>();

            // Vessel
            builder.Services.AddTransient<VesselRegistrationPage>();
            builder.Services.AddTransient<VesselMainPage>();
            builder.Services.AddTransient<VesselListPage>();
            builder.Services.AddTransient<HoldInspectionImagePage>();
            builder.Services.AddTransient<HoldInspectionPage>();
            builder.Services.AddTransient<HoldPage>();
            builder.Services.AddTransient<VesselImagePage>();

            // Vessel/Tabs
            builder.Services.AddTransient<VesselHoldRegistrationTabPage>();
            builder.Services.AddTransient<VesselRegistrationTabPage>();

            builder.Services.AddSingleton<App>();

            return builder.Build();
        }
    }
}