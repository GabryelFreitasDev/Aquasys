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
using RGPopup.Maui.Extensions;

namespace Aquasys
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
            .UseMauiRGPopup()
            .UseMauiCommunityToolkit()
            .UseDevExpress()
                .UseDevExpressCharts()
                .UseDevExpressCollectionView()
                .UseDevExpressControls()
                .UseDevExpressDataGrid()
                .UseDevExpressEditors()
                .UseDevExpressScheduler()
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

            DevExpress.Maui.Charts.Initializer.Init();
            DevExpress.Maui.CollectionView.Initializer.Init();
            DevExpress.Maui.Controls.Initializer.Init();
            DevExpress.Maui.Editors.Initializer.Init();
            DevExpress.Maui.Scheduler.Initializer.Init();

            return builder.Build();
        }
    }
}