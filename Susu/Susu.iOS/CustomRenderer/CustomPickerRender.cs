using System.ComponentModel;
using Susu.CustomControl;
using Susu.iOS.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRender))]
namespace Susu.iOS.CustomRenderer
{
    public class CustomPickerRender: PickerRenderer
    {
        CustomPicker picker = null;
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                picker = Element as CustomPicker;
                UpdatePickerPlaceholder();
                if (picker.SelectedIndex <= -1)
                {
                    UpdatePickerPlaceholder();
                }
               // Control.TextAlignment = UIKit.UITextAlignment.Center;
                //Control.BackgroundColor = Color.FromHex("#2d67e4").ToUIColor();
                Control.TextColor = Color.Black.ToUIColor();
                Control.Layer.MasksToBounds = true; //It is important
            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (picker != null)
            {
                if (e.PropertyName.Equals(CustomPicker.PlaceholderProperty.PropertyName))
                {
                    UpdatePickerPlaceholder();
                }
            }
        }

        void UpdatePickerPlaceholder()
        {
            if (picker == null)
                picker = Element as CustomPicker;
            if (picker.Placeholder != null)
                Control.Placeholder = picker.Placeholder;
        }
    }
}