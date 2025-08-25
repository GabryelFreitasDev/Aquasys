using System.Windows.Input;
using Aquasys.App.Core.Utils;
using Aquasys.App.MVVM.ViewModels;
using RGPopup.Maui.Enums;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace Aquasys.App.Controls
{

    public partial class DCPopupPage : PopupPage
    {
        public static readonly BindableProperty BeforeCloseCommandProperty = BindableProperty.Create(nameof(BeforeCloseCommand), typeof(ICommand), typeof(DCPopupPage));

        public ICommand BeforeCloseCommand
        {
            get { return (ICommand)GetValue(BeforeCloseCommandProperty); }
            set { SetValue(BeforeCloseCommandProperty, value); }
        }

        public static readonly BindableProperty ShowCloseButtonProperty = BindableProperty.Create(nameof(ShowCloseButton), typeof(bool), typeof(DCPopupPage), defaultValue: false);
        public bool ShowCloseButton
        {
            get { return (bool)GetValue(ShowCloseButtonProperty); }
            set { SetValue(ShowCloseButtonProperty, value); }
        }

        public static readonly BindableProperty TituloProperty = BindableProperty.Create(nameof(TituloProperty), typeof(string), typeof(DCPopupPage));

        public string Titulo
        {
            get { return (string)GetValue(TituloProperty); }
            set { SetValue(TituloProperty, value); }
        }

        //falta verficar em MAIU alguma função para verificar se o teclado está sendo exibido
        //private IKeyboardService keyboardService;

        public DCPopupPage()
        {
            InitializeComponent();
            DCPopup.ChildAdded += DCPopupPage_ChildAdded;
            //keyboardService = Microsoft.Maui.Controls.DependencyService.Get<IKeyboardService>();
            //keyboardService.KeyboardIsShown += KeyboardService_KeyboardIsShown;
            //keyboardService.KeyboardIsHidden += KeyboardService_KeyboardIsHidden;
        }

        /*
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
            keyboardService.KeyboardIsShown -= KeyboardService_KeyboardIsShown;
            keyboardService.KeyboardIsHidden -= KeyboardService_KeyboardIsHidden;

        }

        private void KeyboardService_KeyboardIsHidden(object sender, EventArgs e)
        {
            if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.iOS)
                DCPopup.Padding = _defaultPadding ? new Thickness(0, 100, 0, 0) : _padding;
            else
                DCPopup.Padding = _defaultPadding ? new Thickness(0, 250, 0, 0) : _padding;
        }

        private void KeyboardService_KeyboardIsShown(object sender, EventArgs e)
        {
            if (_menuOption == DCPopupOptions.BottomBar)
            {
                DCPopup.Padding = _defaultPadding ? new Thickness(0, 0, 0, keyboardService.GetKeyboardHeight()) : _padding;
            }
        }
        */

        private bool _defaultPadding = true;
        private Thickness _padding;
        private DCPopupOptions _menuOption = DCPopupOptions.RightBar;


        public void SetPadding(double left, double top, double right, double bottom)
        {
            if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                // simula um SafeArea para o Android ;)
                if (left < 20) left = 20;
                if (top < 50) top = 50;
                if (right < 20) right = 20;
                if (bottom < 70) bottom = 70;
            }

            _padding = new Thickness(left, top, right, bottom);
            _defaultPadding = false;
        }

        private Style GetPopupStyle()
        {
            switch (_menuOption)
            {
                case DCPopupOptions.LeftBar:
                    return (Style)ResourceUtils.GetResourceValue(DCPopupStyle.LEFT_BAR);
                case DCPopupOptions.BottomBar:
                    return (Style)ResourceUtils.GetResourceValue(DCPopupStyle.BOTTOM_BAR);
                case DCPopupOptions.BottomFloat:
                    return (Style)ResourceUtils.GetResourceValue(DCPopupStyle.BOTTOM_FLOAT);
                case DCPopupOptions.CenterFloat:
                    return (Style)ResourceUtils.GetResourceValue(DCPopupStyle.CENTER_FLOAT);
                case DCPopupOptions.RightBar:
                default:
                    return (Style)ResourceUtils.GetResourceValue(DCPopupStyle.RIGHT_BAR);

            }
        }

        private ImageButton CreateImageButton()
        {
            ImageButton imageButtonFechar = new ImageButton();

            imageButtonFechar.Source = ImageSource.FromFile("icon_fechar.png");
            imageButtonFechar.Clicked += ImageButtonFechar_Clicked;
            imageButtonFechar.AutomationId = "AIImageButtonFecharPopup";

            return imageButtonFechar;
        }

        private StackLayout CreateTitle()
        {
            var stackLayout = new StackLayout();
            stackLayout.Orientation = StackOrientation.Horizontal;
            stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;

            Label titulo = new Label();
            titulo.Style = (Style)ResourceUtils.GetResourceValue(LabelStyle.LABEL_TITULO);
            titulo.Text = Titulo;
            titulo.Margin = _menuOption == DCPopupOptions.BottomBar
                ? new Thickness(20, 0, 0, 0) : new Thickness(0, 0, 0, 0);

            stackLayout.Children.Add(titulo);

            if (ShowCloseButton)
            {
                ImageButton imageButtonFechar = CreateImageButton();
                imageButtonFechar.Style = (Style)ResourceUtils.GetResourceValue(DCPopupImageButtonStyle.STACK_LAYOUT_TITULO);

                stackLayout.Children.Add(imageButtonFechar);
            }

            return stackLayout;
        }

        private void DCPopupPage_ChildAdded(object sender, ElementEventArgs e)
        {
            if (e.Element is not Microsoft.Maui.Controls.Layout)
                return;

            Layout layout = (Layout)e.Element;
            layout.Style = GetPopupStyle();

            var layoutChildrenList = layout.Children.ToList();

            if (!string.IsNullOrEmpty(Titulo))
            {
                layout.Children.Clear();
                layout.Children.Add(CreateTitle());
            }
            else
            {
                if (ShowCloseButton)
                {
                    ImageButton imageButtonFechar = CreateImageButton();

                    if (e.Element is StackLayout)
                        imageButtonFechar.Style = (Style)ResourceUtils.GetResourceValue(DCPopupImageButtonStyle.STACK_LAYOUT);

                    if (e.Element is AbsoluteLayout)
                        imageButtonFechar.Style = (Style)ResourceUtils.GetResourceValue(DCPopupImageButtonStyle.ABSOLUTE_LAYOUT);

                    layout.Children.Add(imageButtonFechar);
                }
            }

            foreach (var comp in layoutChildrenList)
                layout.Children.Add(comp);
        }

        private async void ImageButtonFechar_Clicked(object sender, EventArgs e)
        {
            if (BeforeCloseCommand?.CanExecute(true) ?? false)
                BeforeCloseCommand?.Execute(true);
            await PopupNavigation.Instance.PopAsync(true);
        }

        public void SetTipoMenu(DCPopupOptions Opcao)
        {
            switch (Opcao)
            {
                case DCPopupOptions.BottomFloat:
                    DCPopup.Padding = _defaultPadding ? new Thickness(20, 350, 20, 20) : _padding;

                    DCPopupScaleAnimation.PositionIn = MoveAnimationOptions.Bottom;
                    DCPopupScaleAnimation.PositionOut = MoveAnimationOptions.Bottom;
                    break;
                case DCPopupOptions.LeftBar:
                    DCPopup.Padding = _defaultPadding ? new Thickness(0, 0, 50, 0) : _padding;

                    DCPopupScaleAnimation.PositionIn = MoveAnimationOptions.Left;
                    DCPopupScaleAnimation.PositionOut = MoveAnimationOptions.Left;
                    break;
                case DCPopupOptions.BottomBar:
                    DCPopup.Padding = _defaultPadding ? new Thickness(0, 350, 0, 0) : _padding;

                    DCPopupScaleAnimation.PositionIn = MoveAnimationOptions.Bottom;
                    DCPopupScaleAnimation.PositionOut = MoveAnimationOptions.Bottom;
                    break;
                case DCPopupOptions.CenterFloat:
                    DCPopup.Padding = _defaultPadding ? new Thickness(20, 50, 20, 70) : _padding;

                    DCPopupScaleAnimation.PositionIn = MoveAnimationOptions.Center;
                    DCPopupScaleAnimation.PositionOut = MoveAnimationOptions.Center;
                    break;
                case DCPopupOptions.RightBar:
                default:
                    DCPopup.Padding = _defaultPadding ? new Thickness(50, 0, 0, 0) : _padding;

                    DCPopupScaleAnimation.PositionIn = MoveAnimationOptions.Right;
                    DCPopupScaleAnimation.PositionOut = MoveAnimationOptions.Right;
                    break;
            }

            _menuOption = Opcao;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as BaseViewModels)?.OnAppearing();
            (BindingContext as BaseViewModels)?.SetInstancePage(this);
            await NavigationUtils.CloseLoading();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            (BindingContext as BaseViewModels)?.OnDisappearing();
        }
    }

    public static class DCPopupImageButtonStyle
    {
        public static string ABSOLUTE_LAYOUT = "DCPopupImageButtonAbsoluteLayout";
        public static string STACK_LAYOUT = "DCPopupImageButtonStackLayout";
        public static string STACK_LAYOUT_TITULO = "DCPopupImageButtonTituloStackLayout";
    }
    public static class DCPopupStyle
    {
        public static string RIGHT_BAR = "DCPopupRightBar";
        public static string LEFT_BAR = "DCPopupLeftBar";
        public static string BOTTOM_BAR = "DCPopupBottomBar";
        public static string BOTTOM_FLOAT = "DCPopupBottomFloat";
        public static string CENTER_FLOAT = "DCPopupCenterFloat";
    }
    public static class LabelStyle
    {
        public static string LABEL_TITULO = "DCPopupLabelTituloStyle";
    }
    public enum DCPopupOptions
    {
        RightBar = 0,
        LeftBar = 1,
        BottomBar = 2,
        BottomFloat = 3,
        CenterFloat = 4
    }
}