using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Aquasys.MVVM.Views.MainPage;
using Aquasys.MVVM.Views.Login;
using Aquasys.MVVM.Views.Vessel;
using Aquasys.MVVM.ViewModels.Login;
using Aquasys.MVVM.ViewModels.Vessel;
using Aquasys.Core.Data;
using Aquasys.Core.BO;
using DevExpress.Maui;
using InputKit.Handlers;

namespace Aquasys
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseDevExpress()
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
            builder.Services.AddSingleton<DatabaseContext>();
            
            ////BOs
            //builder.Services.AddTransient<UserBO>();
            //builder.Services.AddTransient<VesselBO>();

            ////Login
            //builder.Services.AddTransient<LoginView>();
            //builder.Services.AddTransient<LoadingPage>();
            //builder.Services.AddTransient<CreateAccountView>();

            ////MainPage
            //builder.Services.AddTransient<MainPageView>();

            ////Vessel
            //builder.Services.AddTransient<VesselListView>();
            //builder.Services.AddTransient<VesselListViewModel>();
            //builder.Services.AddTransient<VesselMainView>();
            //builder.Services.AddTransient<VesselMainViewModel>();
            //builder.Services.AddTransient<VesselCargoHoldInspectionView>();

            DevExpress.Maui.Charts.Initializer.Init();
            DevExpress.Maui.CollectionView.Initializer.Init();
            DevExpress.Maui.Controls.Initializer.Init();
            DevExpress.Maui.Editors.Initializer.Init();
            DevExpress.Maui.Scheduler.Initializer.Init();

            return builder.Build();
        }
    }
}