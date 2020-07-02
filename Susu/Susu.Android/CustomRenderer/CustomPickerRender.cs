using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Susu.CustomControl;
using Susu.Droid.CustomRenderer;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRender))]
namespace Susu.Droid.CustomRenderer
{
    public class CustomPickerRender : Xamarin.Forms.Platform.Android.AppCompat.PickerRenderer
        {
            CustomPicker picker = null;
            public CustomPickerRender(Context context) : base(context)
            {
                
            }
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
                    Control.Background = AddPickerStyles();
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

            protected override void UpdatePlaceHolderText()
            {
                UpdatePickerPlaceholder();
            }

            void UpdatePickerPlaceholder()
            {
                if (picker == null)
                    picker = Element as CustomPicker;
                if (picker.Placeholder != null)
                {
                    Control.Hint = picker.Placeholder;
                    Control.SetHintTextColor(Android.Graphics.Color.Gray);
                }
                   
            }
        public LayerDrawable AddPickerStyles()
        {
            ShapeDrawable border = new ShapeDrawable();
            border.Paint.Color = Android.Graphics.Color.ParseColor("#2d67e4");
            border.SetPadding(20, 20, 20, 20);
            border.Paint.SetStyle(Paint.Style.Stroke);
            //border.Paint.StrokeWidth = 5;
            Drawable[] layers = { border };
            LayerDrawable layerDrawable = new LayerDrawable(layers);
            layerDrawable.SetLayerInset(0, 0, 0, 0, 0);
            return layerDrawable;
        }

    }
           

}