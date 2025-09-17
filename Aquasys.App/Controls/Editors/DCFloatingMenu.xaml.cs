using Aquasys.App.Core.Utils;
using CommunityToolkit.Mvvm.Input;
using RGPopup.Maui.Extensions;
using System.Windows.Input;

namespace Aquasys.App.Controls.Editors
{
    public partial class DCFloatingMenu : ContentView
    {
        public DCFloatingMenu()
        {
            InitializeComponent();
        }

        #region MainButtonText
        public static readonly BindableProperty MainButtonTextProperty = BindableProperty.Create(nameof(MainButtonText), typeof(string), typeof(DCFloatingMenu));
        public string MainButtonText
        {
            get => (string)GetValue(MainButtonTextProperty);
            set => SetValue(MainButtonTextProperty, value);
        }
        #endregion MainButtonIcon

        #region MainButtonIcon
        public static readonly BindableProperty MainButtonIconProperty = BindableProperty.Create(nameof(MainButtonIcon), typeof(string), typeof(DCFloatingMenu));
        public string MainButtonIcon
        {
            get => (string)GetValue(MainButtonIconProperty);
            set => SetValue(MainButtonIconProperty, value);
        }
        #endregion MainButtonIcon

        #region MainButtonBackgroundColor
        public static readonly BindableProperty MainButtonBackgroundColorProperty = BindableProperty.Create(nameof(MainButtonBackgroundColor), typeof(Color), typeof(DCFloatingMenu), (Color)ResourceUtils.GetResourceValue("Primary"));
        public Color MainButtonBackgroundColor
        {
            get { return (Color)GetValue(MainButtonBackgroundColorProperty); }
            set { SetValue(MainButtonBackgroundColorProperty, value); }
        }
        #endregion MainButtonBackgroundColor

        #region MainButtonForegroundColor
        [Obsolete]
        public static readonly BindableProperty MainButtonForegroundColorProperty = BindableProperty.Create(nameof(MainButtonForegroundColor), typeof(Color), typeof(DCFloatingMenu), (Color)ResourceUtils.GetResourceValue("Primary"));
        [Obsolete]
        public Color MainButtonForegroundColor
        {
            get { return (Color)GetValue(MainButtonForegroundColorProperty); }
            set { SetValue(MainButtonForegroundColorProperty, value); }
        }
        #endregion MainButtonForegroundColor

        #region Items
        public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(List<DCFloatingMenuItem>), typeof(DCFloatingMenu));
        public List<DCFloatingMenuItem> Items
        {
            get => (List<DCFloatingMenuItem>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }
        #endregion Items

        #region Command
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(DCFloatingMenu));
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        #endregion Command

        #region WidthRequest
        public static readonly BindableProperty WidthRequestProperty = BindableProperty.Create(nameof(WidthRequest), typeof(double), typeof(DCFloatingMenu), defaultValue: -1d);
        public double WidthRequest
        {
            get { return (double)GetValue(WidthRequestProperty); }
            set { SetValue(WidthRequestProperty, value); }
        }
        #endregion WidthRequest

        #region HeightRequest
        public static readonly BindableProperty HeightRequestProperty = BindableProperty.Create(nameof(HeightRequest), typeof(double), typeof(DCFloatingMenu), defaultValue: -1d);
        public double HeightRequest
        {
            get { return (double)GetValue(HeightRequestProperty); }
            set { SetValue(HeightRequestProperty, value); }
        }
        #endregion HeightRequestMainButton

        private SemaphoreSlim avoidDoubleClick = new(1, 1);

        private async void BtnPlus_Clicked(object sender, EventArgs e)
        {
            if (avoidDoubleClick.CurrentCount == 0)
                return;
            await avoidDoubleClick.WaitAsync();

            try
            {
                if (Command != null)
                {
                    Command.Execute(null);
                    return;
                }

                    //await Application.Current!.MainPage!.Navigation.PushPopupAsync(new DCFloatingMenuPopup(this));        
            }
            finally
            {
                avoidDoubleClick.Release();
            }
        }

    }
    public class DCFloatingMenuItem
    {
        public string ItemText { get; set; }
        public ICommand ItemCommand { get; set; }
        public IAsyncRelayCommand AsyncItemCommand { get; set; }
        public string AutomationId { get; set; }
        public string ItemIcon { get; set; }
        [Obsolete]
        public string ItemIconSvg { get { return ItemIcon; } set { ItemIcon = value; } }
        [Obsolete]
        public int ItemIconSvgHeight { get; set; }
    }
}

