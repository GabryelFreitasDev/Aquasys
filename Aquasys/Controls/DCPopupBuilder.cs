using Aquasys.Core.Utils;

namespace Aquasys.Controls
{
    public class DCPopupBuilder
    {
        private string _Title { get; set; }
        private string _Message { get; set; }
        private bool _ShowCloseButton { get; set; }
        private string _CancelButtonLabel { get; set; } = null;
        private string _ConfirmButtonLabel { get; set; } = null;
        private bool _IsPopupBottom { get; set; }
        private bool _IsLastButtonGreen { get; set; }

        private StackOrientation _ButtonsOrientation { get; set; } = StackOrientation.Horizontal;

        private List<string> _OptionsList { get; set; } = new List<string>();
        private List<string> _ButtonsList { get; set; } = new List<string>();

        private DCPopup _popup;

        public DCPopupBuilder()
        {
            _Title = "Popup";
            _ShowCloseButton = false;
        }

        public DCPopup Build()
        {
            _popup = new DCPopup(_Title, _ShowCloseButton, _IsPopupBottom);
            _popup.BackgroundColor = (Color)ResourceUtils.GetResourceValue("ColorBlackTransparent50");
            _popup.Message = _Message;

            _popup.IsLastButtonGreen = _IsLastButtonGreen;
            _popup.CancelButtonLabel = _CancelButtonLabel;
            _popup.ConfirmButtonLabel = _ConfirmButtonLabel;

            _popup.ButtonsOrientation = _ButtonsOrientation;

            _popup.OptionsList = _OptionsList;

            _popup.ButtonsList = _ButtonsList;

            return _popup;
        }

        public DCPopupBuilder SetTitle(string title)
        {
            _Title = title;
            return this;
        }

        public DCPopupBuilder SetMessage(string message)
        {
            _Message = message;
            return this;
        }

        public DCPopupBuilder ShowCloseButton(bool showCloseButton)
        {
            _ShowCloseButton = showCloseButton;
            return this;
        }

        public DCPopupBuilder IsPopUpBottom(bool isPopUpBottom)
        {
            _IsPopupBottom = isPopUpBottom;
            return this;
        }

        public DCPopupBuilder IsLastButtonGreen(bool isLastButtonGreen)
        {
            _IsLastButtonGreen = isLastButtonGreen;
            return this;
        }

        public DCPopupBuilder AddConfirmButton(string confirmButtonLabel)
        {
            _ConfirmButtonLabel = confirmButtonLabel;
            return this;
        }

        public DCPopupBuilder AddCancelButton(string cancelButtonLabel)
        {
            _CancelButtonLabel = cancelButtonLabel;
            return this;
        }

        public DCPopupBuilder AddExtraButton(string buttonLabel)
        {
            _ButtonsList.Add(buttonLabel);
            return this;
        }

        public DCPopupBuilder AddOption(string optionLabel)
        {
            _OptionsList.Add(optionLabel);
            return this;
        }

        public DCPopupBuilder SetButtonsOrientation(StackOrientation buttonsOrientation)
        {
            _ButtonsOrientation = buttonsOrientation;
            return this;
        }
    }
}
