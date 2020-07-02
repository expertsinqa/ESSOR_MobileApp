using Android.Content;
using Susu.CustomControl;
using Susu.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRender))]
namespace Susu.Droid.CustomRenderer
{
    public class CustomLabelRender : LabelRenderer
    {
        public CustomLabelRender(Context context):base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                var item = Control;
                //item.TextAlignment = Android.Views.TextAlignment.ViewEnd;
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                {
                    Control.JustificationMode = Android.Text.JustificationMode.InterWord;
                }
            }
        }
    }
}