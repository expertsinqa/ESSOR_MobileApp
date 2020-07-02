using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Susu.CustomControl;
using Susu.Droid.CustomRenderer;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("Xamarin")]
[assembly: ExportEffect(typeof(RoundCornersEffectDroid), nameof(RoundCornersEffect))]

namespace Susu.Droid.CustomRenderer
{
    public class RoundCornersEffectDroid:PlatformEffect
    {
        private Android.Views.View View => Control ?? Container;

        protected override void OnAttached()
        {
            try
            {
                PrepareContainer();
                SetCornerRadius();
            }
            catch (Exception e)
            {
                //Debug.WriteLine(e);
            }
        }

        protected override void OnDetached()
        {
            try
            {
                View.OutlineProvider = ViewOutlineProvider.Background;
            }
            catch (Exception e)
            {
                //Debug.WriteLine(e);
            }
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            if (args.PropertyName == RoundCornersEffect.CornerRadiusProperty.PropertyName)
                SetCornerRadius();
        }

        private void PrepareContainer()
        {
           // View.ClipToOutline = true;
            View.SetClipToOutline(true);
        }

        private void SetCornerRadius()
        {
            var cornerRadius = RoundCornersEffect.GetCornerRadius(Element) * GetDensity();
            View.OutlineProvider = new RoundedOutlineProvider(cornerRadius);
        }

        private static double GetDensity() =>
            DeviceDisplay.MainDisplayInfo.Density;

        private class RoundedOutlineProvider : ViewOutlineProvider
        {
            private readonly double _radius;

            public RoundedOutlineProvider(double radius)
            {
                _radius = radius;
            }

            public override void GetOutline(Android.Views.View view, Outline outline)
            {
                outline?.SetRoundRect(0, 0, view.Width, view.Height, (float)_radius);
                
            }

            
        }
    }
}