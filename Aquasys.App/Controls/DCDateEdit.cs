using Aquasys.App.Core.Utils;
using DevExpress.Maui.Core;
using DevExpress.Maui.Editors;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Aquasys.App.Controls
{
    public class DCDateEdit : DateEdit
    {
        public static readonly BindableProperty PropertyNameProperty = BindableProperty.Create(nameof(PropertyName), typeof(string), typeof(DCDateEdit), defaultBindingMode: BindingMode.TwoWay);

        public DCDateEdit()
        {
            this.StyleId = Guid.NewGuid().ToString();

            PickerHeaderAppearance = new CalendarHeaderAppearance();
            PickerHeaderAppearance.HeaderTitleFontFamily = "OpenSansRegular";
            PickerHeaderAppearance.HeaderTitleFontSize = 20;
            PickerHeaderAppearance.HeaderTitleTextColor = (Color)ResourceUtils.GetResourceValue("ColorWhite");
            PickerHeaderAppearance.HeaderSubtitleFontFamily = "OpenSansRegular";
            PickerHeaderAppearance.HeaderSubtitleFontSize = 12;
            PickerHeaderAppearance.HeaderSubtitleTextColor = (Color)ResourceUtils.GetResourceValue("ColorWhite");
            PickerHeaderAppearance.BackgroundColor = (Color)ResourceUtils.GetResourceValue("Primary");

            PickerDayCellAppearance = new CalendarDayCellAppearance();
            PickerDayCellAppearance.FontFamily = "OpenSansRegular";
            PickerDayCellAppearance.FontSize = 14;
            PickerDayCellAppearance.DisabledFontSize = 14;
            PickerDayCellAppearance.DisabledTextColor = (Color)ResourceUtils.GetResourceValue("Black");
            PickerDayCellAppearance.TextColor = (Color)ResourceUtils.GetResourceValue("Black");
            PickerDayCellAppearance.BackgroundColor = (Color)ResourceUtils.GetResourceValue("ColorWhite");

            PickerDayOfWeekCellAppearance = new CalendarDayOfWeekCellAppearance();
            PickerDayOfWeekCellAppearance.FontFamily = "OpenSansRegular";
            PickerDayOfWeekCellAppearance.FontSize = 14;
            PickerDayOfWeekCellAppearance.TextColor = (Color)ResourceUtils.GetResourceValue("Black");
            PickerDayOfWeekCellAppearance.BackgroundColor = (Color)ResourceUtils.GetResourceValue("ColorWhite");

            PickerShowTrailingDays = true;

            PickerBackgroundColor = (Color)ResourceUtils.GetResourceValue("ColorWhite");
            PickerCustomDayCellAppearance += DCDateEdit_PickerCustomDayCellAppearance;

            var xaml = $@"<DataTemplate xmlns=""http://schemas.microsoft.com/dotnet/2021/maui"" 
                                        xmlns:x=""http://schemas.microsoft.com/winfx/2009/xaml"">
                              <HorizontalStackLayout Spacing=""8"" 
                                                     Margin=""20,0,20,20"" 
                                                     HorizontalOptions=""End"">
                                  <Button
                                          Command=""{{Binding CancelCommand}}"" 
                                          FontFamily=""OpenSansRegular"" 
                                          Text=""Cancelar""
                                          TextColor=""{{StaticResource Primary}}""
                                          BackgroundColor=""{{StaticResource ColorWhite}}""
                                          BorderColor=""{{StaticResource PrimaryDark}}"" />
                                  <Button
                                          Text=""Confirmar""
                                          Command=""{{Binding ConfirmCommand}}"" 
                                          FontFamily = ""OpenSansRegular"" 
                                          TextColor=""{{StaticResource Primary}}""
                                          BackgroundColor=""{{StaticResource PrimaryDark}}"" />
                              </HorizontalStackLayout>
                          </DataTemplate>";
            var buttonAreaTemplate = new DataTemplate().LoadFromXaml(xaml);
            PickerButtonAreaTemplate = buttonAreaTemplate;
            DateChanged += DCDateEdit_DateChanged;
        }

        private async void DCDateEdit_DateChanged(object? sender, EventArgs e)
        {
            //essa nova implementação foi devido ao um bug no componente, é nescessário um delay para dar tempo de atualizar a UI, senão vai entrar em loop.

            if (DateChangedCommand?.CanExecute(DateChangedCommandParameter) ?? false)
            {
                await Task.Delay(100);
                DateChangedCommand.Execute(DateChangedCommandParameter);
            }
        }

        public static readonly BindableProperty DateChangedCommandProperty = BindableProperty.Create(nameof(DateChangedCommand), typeof(ICommand), typeof(DCDateEdit));
        public new ICommand DateChangedCommand
        {
            get
            {
                return (ICommand)GetValue(DateChangedCommandProperty);
            }
            set
            {
                SetValue(DateChangedCommandProperty, value);
            }
        }

        private void DCDateEdit_PickerCustomDayCellAppearance(object? sender, CustomSelectableCellAppearanceEventArgs e)
        {
            if (e.IsTrailing)
            {
                e.FontFamily = "OpenSansRegular";
                e.FontSize = 14;
                e.TextColor = (Color)ResourceUtils.GetResourceValue("Black");
            }

            if (e.IsSelected)
            {
                e.FontFamily = "OpenSansRegular";
                e.FontSize = 18;
                e.TextColor = (Color)ResourceUtils.GetResourceValue("ColorWhite");
                e.EllipseBackgroundColor = (Color)ResourceUtils.GetResourceValue("Primary");
            }
            else
            {
                if (e.Date == DateTime.Now.Date)
                {
                    if (!e.IsSelected)
                    {
                        e.FontFamily = "OpenSansRegular";
                        e.FontSize = 14;
                        e.TextColor = (Color)ResourceUtils.GetResourceValue("Primary");
                        e.EllipseBackgroundColor = (Color)ResourceUtils.GetResourceValue("PrimaryDark");
                    }
                }
            }
        }
        public string PropertyName
        {
            get { return ResourceUtils.GetNameOfBinding(this, DateProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public static readonly BindableProperty IsRequiredProperty = BindableProperty.Create(nameof(IsRequired), typeof(bool), typeof(DCDateEdit), false);
        public bool IsRequired
        {
            get { return (bool)GetValue(IsRequiredProperty); }
            set { SetValue(IsRequiredProperty, value); }
        }
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(LabelText) || propertyName == nameof(IsRequired))
            {
                if (!string.IsNullOrEmpty(LabelText) && !LabelText.EndsWith(" *") && IsRequired)
                {
                    LabelText += " *";
                }
                else if (IsRequired == false && LabelText != null && LabelText.EndsWith(" *"))
                {
                    LabelText = LabelText.Replace(" *", "");
                }

                if (Date != null && !IsRequired)
                    ClearIconVisibility = IconVisibility.Always;
                else
                    ClearIconVisibility = IconVisibility.Never;

            }

            if (propertyName == nameof(Date))
            {
                if (Date.HasValue && !IsRequired && IsEnabled)
                    ClearIconVisibility = IconVisibility.Always;
                else
                    ClearIconVisibility = IconVisibility.Never;

                if (Date.HasValue)
                {
                    TextFontFamily = "OpenSansRegular";
                    LabelColor = (Color)ResourceUtils.GetResourceValue("Black");

                    if (LabelText.IsNullOrEmpty())
                        LabelText = PlaceholderText;

                    Margin = 0;
                }
                else
                {
                    TextFontFamily = "OpenSansRegular";
                    LabelColor = (Color)ResourceUtils.GetResourceValue("Black");

                    if (PlaceholderText.IsNullOrEmpty())
                        PlaceholderText = LabelText;

                    PlaceholderColor = (Color)ResourceUtils.GetResourceValue("Black");
                    LabelText = "";
                    Margin = new Thickness(0, 8, 0, 0);
                }
            }

            if (propertyName == nameof(IsVisible))
                OnPropertyChanged(nameof(HeightRequest));
        }

        public static readonly BindableProperty LabelTextChangerProperty = BindableProperty.Create(nameof(LabelTextChanger), typeof(string), typeof(DCDateEdit), defaultBindingMode: BindingMode.TwoWay, propertyChanged: AlternateLabelTextChanged);

        public string LabelTextChanger
        {
            get { return (string)GetValue(LabelTextChangerProperty); }
            set { SetValue(LabelTextChangerProperty, value); }
        }

        private static void AlternateLabelTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not DCDateEdit edit)
                return;

            if (!edit.LabelTextChanger.IsNullOrEmpty())
            {
                edit.LabelText = edit.LabelTextChanger;
                edit.OnPropertyChanged(nameof(DCDateEdit.LabelText));
            }
        }
    }
}
