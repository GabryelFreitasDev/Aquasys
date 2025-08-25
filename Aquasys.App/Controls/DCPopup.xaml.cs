using Aquasys.App.Core.Utils;
using RGPopup.Maui.Extensions;

namespace Aquasys.App.Controls
{

    public partial class DCPopup : DCPopupPage
    {
        private TaskCompletionSource<bool?> TaskCompletionSourceBool { get; set; } = new TaskCompletionSource<bool?>();
        public Task<bool?> PageClosedTaskBool => TaskCompletionSourceBool.Task;
        private bool? ResultBool { get; set; }

        private TaskCompletionSource<int?> TaskCompletionSourceInt { get; set; } = new TaskCompletionSource<int?>();
        public Task<int?> PageClosedTaskInt => TaskCompletionSourceInt.Task;
        private int? ResultInt { get; set; }

        private bool IsConfirmation => !string.IsNullOrEmpty(ConfirmButtonLabel);

        private List<string> _OptionsList;
        public List<string> OptionsList
        {
            get { return _OptionsList ?? (_OptionsList = new List<string>()); }
            set
            {
                _OptionsList = value;
                OptionsListContainer.IsVisible = value?.Any() ?? false;
            }
        }
        public bool IsOptionsList => OptionsList.Any();

        public List<string> ButtonsList = new List<string>();

        public string Message
        {
            get { return Mensagem.Text; }
            set
            {
                Mensagem.Text = value;
                Mensagem.IsVisible = !string.IsNullOrEmpty(value);
            }
        }

        public string CancelButtonLabel
        {
            get { return CancelButton.Text; }
            set
            {
                CancelButton.Text = value;
                CancelButton.IsVisible = !string.IsNullOrEmpty(value);
            }
        }

        public bool IsLastButtonGreen { get; set; }

        public string ConfirmButtonLabel
        {
            get { return ConfirmButton.Text; }
            set
            {
                ConfirmButton.Text = value;
                ConfirmButton.IsVisible = !string.IsNullOrEmpty(value) && !IsOptionsList;
            }
        }

        public StackOrientation ButtonsOrientation
        {
            get { return ButtonsListContainer.Orientation; }
            set { ButtonsListContainer.Orientation = value; }
        }

        public DCPopup(string title, bool showCloseButton, bool ehPopUpBottom)
        {
            Titulo = title;

            ShowCloseButton = showCloseButton;

            var tipoMenu = ehPopUpBottom ? DCPopupOptions.BottomBar : DCPopupOptions.CenterFloat;

            SetTipoMenu(tipoMenu);

            InitializeComponent();

            PopupCorpo.VerticalOptions = ehPopUpBottom ? LayoutOptions.End : LayoutOptions.Center;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            foreach (string optionButtonLabel in OptionsList)
                OptionsListContainer.Children.Add(CreateOptionButton(optionButtonLabel, OptionsList.IndexOf(optionButtonLabel)));

            foreach (string extraButtonLabel in ButtonsList)
                ButtonsListContainer.Children.Insert(ButtonsList.IndexOf(extraButtonLabel), CreateExtraButton(extraButtonLabel));
        }

        private Button CreateOptionButton(string optionLabel, int index)
        {
            Button optionButton = new Button()
            {
                AutomationId = "AIDCPopupOptionButton-" + optionLabel,
                TextColor = (Color)ResourceUtils.GetResourceValue("White"),
                BorderColor = (Color)ResourceUtils.GetResourceValue("Primary"),
                BorderWidth = 1,
                TextTransform = TextTransform.None,
                BackgroundColor = (Color)ResourceUtils.GetResourceValue("Primary"),
                Text = optionLabel,
                FontFamily = "Quicksand600Font"
            };

            optionButton.Clicked += (object sender, EventArgs e) =>
            {
                OptionButton_Clicked(index);
            };
            return optionButton;
        }

        private bool IsGreenButton(int index) =>
            IsLastButtonGreen && index + 1 == OptionsList.Count();

        private Button CreateExtraButton(string buttonLabel)
        {
            Button extraButton = new Button()
            {
                Style = (Style)ResourceUtils.GetResourceValue("PopupButtonStyle"),
                Text = buttonLabel
            };
            extraButton.Clicked += ExtraButton_Clicked;
            return extraButton;
        }

        protected override void OnDisappearing()
        {
            TaskCompletionSourceBool.SetResult(ResultBool);
            TaskCompletionSourceInt.SetResult(ResultInt);

            base.OnDisappearing();
        }

        private async void OptionButton_Clicked(int optionsListClickedIndex)
        {
            ResultBool = null;
            ResultInt = optionsListClickedIndex;

            await Navigation.PopPopupAsync();
        }

        private async void ConfirmButton_Clicked(object sender, System.EventArgs e)
        {
            ResultBool = true;
            ResultInt = 1;

            await Navigation.PopPopupAsync();
        }

        private async void CancelButton_Clicked(object sender, System.EventArgs e)
        {
            ResultBool = false;
            ResultInt = null;

            await Navigation.PopPopupAsync();
        }

        private async void ExtraButton_Clicked(object sender, System.EventArgs e)
        {
            ResultBool = null;
            ResultInt = ButtonsList.IndexOf(((Button)sender).Text);

            await Navigation.PopPopupAsync();
        }
    }
}