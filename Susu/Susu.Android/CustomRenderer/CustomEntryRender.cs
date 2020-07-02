using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Susu.CustomControl;
using Susu.Droid.CustomRenderer;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(CustomEntry),typeof(CustomEntryRender))]
namespace Susu.Droid.CustomRenderer
{
    public class CustomEntryRender : EntryRenderer
    {
        public CustomEntryRender(Context context):base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            try
            {
                if (e.OldElement == null)
                {
                    var nativeEditText = (EditText)Control;
                    var shape = new ShapeDrawable(new Android.Graphics.Drawables.Shapes.RectShape());
                    shape.Paint.Color = Xamarin.Forms.Color.Gray.ToAndroid();
                    shape.Paint.SetStyle(Paint.Style.Stroke);
                    nativeEditText.Background = shape;
                    GradientDrawable gd = new GradientDrawable();
                    gd.SetColor(Android.Graphics.Color.White);
                    gd.SetCornerRadius(20);
                    gd.SetStroke(3, Android.Graphics.Color.ParseColor("#2d67e4"));
                    nativeEditText.SetBackground(gd);
                    //nativeEditText.Background = Resources.GetDrawable("EntryBorderColor");
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}