using Susu.CustomControl;
using Susu.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRender))]
namespace Susu.iOS.CustomRenderer
{
    public class CustomLabelRender : LabelRenderer
    {
        protected override void OnElementChanged(
            ElementChangedEventArgs<Label> e)
        {

            base.OnElementChanged(e);

            var label = Control as UILabel;
            if (label != null)
            {
                label.BaselineAdjustment = UIBaselineAdjustment.None;
                //label.TextAlignment = UITextAlignment.Justified;
                label.LineBreakMode = UILineBreakMode.CharacterWrap;
            }
        }
    }
}