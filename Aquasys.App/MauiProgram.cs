
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
            builder.Services.AddSingleton<ILocalRepository<Inspection>, LocalRepository<Inspection>>();
            builder.Services.AddSingleton<ILocalRepository<TypeVessel>, LocalRepository<TypeVessel>>();
            builder.Services.AddSingleton<ILocalRepository<HoldCargo>, LocalRepository<HoldCargo>>();
            builder.Services.AddSingleton<ILocalRepository<HoldImage>, LocalRepository<HoldImage>>();
            builder.Services.AddSingleton<ILocalRepository<HoldCondition>, LocalRepository<HoldCondition>>();
            builder.Services.AddSingleton<ILocalRepository<HoldInspection>, LocalRepository<HoldInspection>>();
            builder.Services.AddSingleton<ILocalRepository<VesselImage>, LocalRepository<VesselImage>>();
            builder.Services.AddSingleton<ReportGeneratorService>();


            // Report
            var assemblies = new[] { typeof(VesselReport).Assembly };
            builder.Services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo<IReportTemplate>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

            builder.Services.AddSingleton<Aquasys.Reports.Services.ReportGeneratorService>();

            // Login
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<CreateAccountViewModel>();

            //Main Page
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<OptionsViewModel>();

            //Vessel e Tabs
            builder.Services.AddTransient<VesselMainViewModel>();
            builder.Services.AddTransient<VesselListViewModel>();
            builder.Services.AddTransient<VesselHoldRegistrationTabViewModel>();
            builder.Services.AddTransient<VesselInspectionRegistrationTabViewModel>();
            builder.Services.AddTransient<VesselRegistrationTabViewModel>();
            builder.Services.AddTransient<HoldImageViewModel>();
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
            builder.Services.AddTransient<VesselMainPage>();
            builder.Services.AddTransient<VesselListPage>();
            builder.Services.AddTransient<HoldImagePage>();
            builder.Services.AddTransient<HoldInspectionPage>();
            builder.Services.AddTransient<HoldPage>();
            builder.Services.AddTransient<VesselImagePage>();

            // Vessel/Tabs
            builder.Services.AddTransient<VesselHoldRegistrationTabPage>();
            builder.Services.AddTransient<VesselInspectionRegistrationTabPage>();
            builder.Services.AddTransient<VesselRegistrationTabPage>();

            builder.Services.AddSingleton<App>();

            return builder.Build();
        }
    }
}