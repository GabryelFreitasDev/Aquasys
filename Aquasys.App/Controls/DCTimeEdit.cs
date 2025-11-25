using System.Runtime.CompilerServices;
using Aquasys.App.Core.Utils;
using DevExpress.Maui.Core;
using DevExpress.Maui.Editors;

namespace Aquasys.App.Controls
{
    public class DCTimeEdit : TimeEdit
    {

        public static readonly BindableProperty PropertyNameProperty = BindableProperty.Create(nameof(PropertyName), typeof(string), typeof(DCTimeEdit), defaultBindingMode: BindingMode.TwoWay);

        public string PropertyName
        {
            get { return ResourceUtils.GetNameOfBinding(this, TimeProperty); }
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
                    LabelText += " *";

                if (Time != null && !IsRequired)
                {
                    this.ClearIconVisibility = IconVisibility.Always;
                }
                else
                    this.ClearIconVisibility = IconVisibility.Never;
            }

            if (propertyName == nameof(Time))
            {
                if (Time.HasValue && !IsRequired && IsEnabled)
                    this.ClearIconVisibility = IconVisibility.Always;
                else
                    this.ClearIconVisibility = IconVisibility.Never;

                if (Time.HasValue)
                {
                    this.TextFontFamily = "OpenSansRegular";
                    this.LabelColor = (Color)ResourceUtils.GetResourceValue("ColorGray500");
                }
                else
                {
                    this.TextFontFamily = "OpenSansRegular";
                    this.LabelColor = (Color)ResourceUtils.GetResourceValue("ColorGray400");
                }
            }
        }
    }
}
