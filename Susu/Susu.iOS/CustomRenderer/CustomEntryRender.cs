using Susu.CustomControl;
using Susu.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRender))]
namespace Susu.iOS.CustomRenderer
{
    public class CustomEntryRender: EntryRenderer
    {
        public CustomEntryRender()
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (this.Control == null)
                return;
            this.Control.BorderStyle = UIKit.UITextBorderStyle.RoundedRect;
            //this.Control.Layer.BorderColor = UIKit.UIColor.Blue.CGColor;
            this.Control.Layer.BorderWidth = 1;
            this.Control.Layer.BorderColor = new UIColor(red: 0.03f, green: 0.23f, blue: 0.40f, alpha: 1.00f).CGColor;
        //this.Control.BorderColor = UIColor.Blue;        

        }
    }
}
