using Aquasys.App.Controls;
using Aquasys.App.Core.Utils;

namespace Aquasys.App.Controls
{
    public partial class DCPopup : DCPopupPage
    {
        public static async Task ShowAlert(string title, string message, string cancelButtonLabel)
        {
            DCPopup modal = new DCPopupBuilder()
                .SetTitle(title)
                .SetMessage(message)
                .AddCancelButton(cancelButtonLabel)
                .Build();

            await NavigationUtils.PushPopupAsync(modal);
        }

        public static async Task<bool?> ShowConfirmation(string title, string message, string cancelButtonLabel, string confirmButtonLabel, bool showCloseButton = false, StackOrientation buttonsOrientation = StackOrientation.Horizontal)
        {
            DCPopup modal = new DCPopupBuilder()
                .SetTitle(title)
                .SetMessage(message)
                .AddCancelButton(cancelButtonLabel)
                .AddConfirmButton(confirmButtonLabel)
                .ShowCloseButton(showCloseButton)
                .SetButtonsOrientation(buttonsOrientation)
                .Build();

            await NavigationUtils.PushPopupAsync(modal);
            return await modal.PageClosedTaskBool;
        }

        public static async Task<int?> ShowOptionsList(IEnumerable<string> optionsList)
        {
            DCPopupBuilder builder = new DCPopupBuilder();

            foreach (string option in optionsList)
                builder.AddOption(option);

            DCPopup modal = builder
                .IsLastButtonGreen(true)
                .IsPopUpBottom(true)
                .SetTitle(" ")
                .ShowCloseButton(true)
                .Build();

            await NavigationUtils.PushPopupAsync(modal);
            return await modal.PageClosedTaskInt;
        }

        public static async Task<int?> ShowOptionsList(string title, List<string> optionsList, bool showCloseButton = false)
        {
            DCPopupBuilder builder = new DCPopupBuilder();

            foreach (string option in optionsList)
                builder.AddOption(option);

            DCPopup modal = builder
                .SetTitle(title)
                .AddCancelButton("Back")
                .ShowCloseButton(showCloseButton)
                .Build();

            await NavigationUtils.PushPopupAsync(modal);
            return await modal.PageClosedTaskInt;
        }

        public static async Task<int?> ShowButtonsList(string title, string message, List<string> buttonsList, bool showCloseButton = false)
        {
            DCPopupBuilder builder = new DCPopupBuilder();

            foreach (string button in buttonsList)
                builder.AddExtraButton(button);

            DCPopup modal = builder
                .SetTitle(title)
                .SetMessage(message)
                .AddCancelButton("Cancel")
                .ShowCloseButton(showCloseButton)
                .SetButtonsOrientation(StackOrientation.Vertical)
                .Build();

            await NavigationUtils.PushPopupAsync(modal);
            return await modal.PageClosedTaskInt;
        }
    }
}
