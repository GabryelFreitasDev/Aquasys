using System.Runtime.CompilerServices;
using System.Windows.Input;
using Aquasys.App.Core.Utils;
using DevExpress.Maui.Controls;

namespace Aquasys.App.Controls
{
    public class DCTabView : TabView
    {
        public static readonly BindableProperty ItemHeaderTappedCommandProperty =
            BindableProperty.Create(
                nameof(ItemHeaderTappedCommand),
                typeof(ICommand),
                typeof(DCTabView));

        public ICommand ItemHeaderTappedCommand
        {
            get => (ICommand)GetValue(ItemHeaderTappedCommandProperty);
            set => SetValue(ItemHeaderTappedCommandProperty, value);
        }

        public DCTabView()
        {
            ItemHeaderTapped += DCTabView_ItemHeaderTapped;
            ItemHeaderTextColor = (Color)ResourceUtils.GetResourceValue("ColorGray700");
            HeaderPanelShadowColor = (Color)ResourceUtils.GetResourceValue("ColorGray200");
            HeaderPanelShadowHeight = 1;

            SelectedItemHeaderTextColor = (Color)ResourceUtils.GetResourceValue("PrimaryDa");
            SelectedItemIndicatorHeight = 2;
            SelectedItemIndicatorColor = (Color)ResourceUtils.GetResourceValue("Primary500");

            ItemHeaderFontFamily = "OpenSansRegular";

        }

        private void DCTabView_ItemHeaderTapped(object sender, ItemHeaderTappedEventArgs e)
        {
            if (ItemHeaderTappedCommand?.CanExecute(e.Index) ?? false)
                ItemHeaderTappedCommand.Execute(e.Index);
        }
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName.Equals(nameof(SelectedItemIndex)))
            {
                if (ItemHeaderTappedCommand?.CanExecute(SelectedItemIndex) ?? false)
                    ItemHeaderTappedCommand.Execute(SelectedItemIndex);
            }
        }

    }
    public class DCTabViewItem : TabViewItem { }
}
