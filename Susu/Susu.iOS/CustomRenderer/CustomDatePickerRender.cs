using Susu.CustomControl;
using Susu.iOS.CustomRenderer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRender))]
namespace Susu.iOS.CustomRenderer
{
    public  class CustomDatePickerRender:DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null && this.Control != null)
            {
                this.AddClearButton();

                var entry = (CustomDatePicker)this.Element;
                if (!entry.NullableDate.HasValue)
                {
                    this.Control.Text = entry.PlaceHolder;
                }

                if (Device.Idiom == TargetIdiom.Tablet)
                {
                    this.Control.Font = UIFont.SystemFontOfSize(25);
                }
                this.Control.BorderStyle = UITextBorderStyle.Line;
                Control.Layer.BorderColor = UIColor.LightGray.CGColor;
                Control.Layer.BorderWidth = 1;

                var imageview = new UIImageView(UIImage.FromBundle(entry.Image))
                {
                    Frame = new RectangleF(10, 0, 20, 20)
                };
                UIView objLeftView = new UIView(new System.Drawing.Rectangle(0, 0, 40, 20));
                objLeftView.AddSubview(imageview);
                Control.RightViewMode = UITextFieldViewMode.Always;
                Control.RightView = objLeftView;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Xamarin.Forms.DatePicker.DateProperty.PropertyName || e.PropertyName == Xamarin.Forms.DatePicker.FormatProperty.PropertyName)
            {
                SetDate(Element.Date);
            }
            base.OnElementPropertyChanged(sender, e);
        }

        private void AddClearButton()
        {
            var originalToolbar = this.Control.InputAccessoryView as UIToolbar;

            if (originalToolbar != null && originalToolbar.Items.Length <= 2)
            {
                var clearButton = new UIBarButtonItem("Clear", UIBarButtonItemStyle.Done, ((sender, ev) =>
                {
                    CustomDatePicker baseDatePicker = this.Element as CustomDatePicker;
                    this.Element.Unfocus();
                    if (this.Element.Date == DateTime.Today)
                    {
                        Control.Text = "MM-dd-yyyy";
                        baseDatePicker.CleanDate();
                    }
                }));
                var OkButton = new UIBarButtonItem("OK", UIBarButtonItemStyle.Done, ((sender, ev) =>
                {
                    CustomDatePicker baseDatePicker = this.Element as CustomDatePicker;
                    SetDate(this.Element.Date);
                    baseDatePicker.AssignValue(this.Element.Date);
                    this.Element.Unfocus();

                }));

                var newItems = new List<UIBarButtonItem>();
                newItems.Insert(0, clearButton);
                newItems.Insert(1, OkButton);
                var flexibleSpace = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
                originalToolbar.SetItems(new[] { clearButton, flexibleSpace, OkButton }, false);
            }

        }

        void SetDate(DateTime date)
        {
            this.Control.Text = date.ToString(Element.Format);
        }
    }
}