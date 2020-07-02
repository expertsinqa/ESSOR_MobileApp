using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Susu.CustomControl;
using Susu.Droid.CustomRenderer;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(CustomDatePicker),typeof(CustomDatePickerRender))]
namespace Susu.Droid.CustomRenderer
{
    public class CustomDatePickerRender : ViewRenderer<CustomDatePicker, EditText>
    {
        DatePickerDialog _dialog;

        public CustomDatePickerRender(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<CustomDatePicker> e)
        {
            base.OnElementChanged(e);
            CustomDatePicker element = (CustomDatePicker)this.Element;
            this.SetNativeControl(new Android.Widget.EditText(Context));
            if (Control == null || e.NewElement == null)
                return;
            if (Control != null && this.Element != null && !string.IsNullOrEmpty(element.Image))
                Control.Background = AddPickerStyles(element.Image);
            //Control.SetSingleLine(false);

            var entry = (CustomDatePicker)this.Element;

            this.Control.Click += OnPickerClick;
            this.Control.Text = !entry.NullableDate.HasValue ? entry.PlaceHolder : Element.Date.ToString(Element.Format);
            //this.Control.TextColors = Android.Graphics.Color.Gray;
            this.Control.KeyListener = null;
            this.Control.FocusChange += OnPickerFocusChange;
            this.Control.Enabled = Element.IsEnabled;

        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Xamarin.Forms.DatePicker.DateProperty.PropertyName || e.PropertyName == Xamarin.Forms.DatePicker.FormatProperty.PropertyName)
            {
                SetDate(Element.Date);
            }
            //else
            //{
            //    var entry = (CustomDatePicker)this.Element;
            //    if (this.Element.Date.ToString() == entry.PlaceHolder)
            //    {
            //        this.Control.Text = entry.PlaceHolder;
            //        return;
            //    }
            //}
            base.OnElementPropertyChanged(sender, e);
        }
        void OnPickerFocusChange(object sender, Android.Views.View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                ShowDatePicker();
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                this.Control.Click -= OnPickerClick;
                this.Control.FocusChange -= OnPickerFocusChange;

                if (_dialog != null)
                {
                    _dialog.Hide();
                    _dialog.Dispose();
                    _dialog = null;
                }
            }

            base.Dispose(disposing);
        }
        void OnPickerClick(object sender, EventArgs e)
        {
            ShowDatePicker();
        }

        void SetDate(DateTime date)
        {
            this.Control.Text = date.ToString(Element.Format);
            //Element.Date = date;
        }

        private void ShowDatePicker()
        {
            CreateDatePickerDialog(this.Element.Date.Year, this.Element.Date.Month - 1, this.Element.Date.Day);
            _dialog.Show();
        }
        void CreateDatePickerDialog(int year, int month, int day)
        {
            CustomDatePicker view = Element;
            _dialog = new DatePickerDialog(Context, (o, e) =>
            {
                view.Date = e.Date;
                ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                Control.ClearFocus();

                _dialog = null;
            }, year, month, day);
            DateTime today = DateTime.Today;
            //double maxSeconds = (DateTime.Today - new DateTime(1970, 1, 1)).TotalMilliseconds;
            double minSeconds = (new DateTime(today.Year, today.Month, today.Day) - new DateTime(1970, 1, 1)).TotalMilliseconds;
            // _dialog.DatePicker.MaxDate = (long)maxSeconds;
            _dialog.DatePicker.MinDate = (long)minSeconds;
            _dialog.SetButton("OK", (sender, e) =>
            {
                this.Element.Format = this.Element._originalFormat;
                SetDate(_dialog.DatePicker.DateTime);
                this.Element.AssignValue(_dialog.DatePicker.DateTime);
            });
            _dialog.SetButton2("Cancel", (sender, e) =>
            {

                if (_dialog.DatePicker.DateTime.ToString(Element.Format) == DateTime.Today.ToString(Element.Format))
                {
                    this.Element.CleanDate();
                    //Control.Text = _dialog.DatePicker.DateTime.ToString(Element.Format);
                }

                var entry = (CustomDatePicker)this.Element;
            });
        }
        public LayerDrawable AddPickerStyles(string imagePath)
        {
            ShapeDrawable border = new ShapeDrawable();
            border.Paint.Color = Android.Graphics.Color.Black;
            border.SetPadding(20, 20, 20, 20);
            border.Paint.SetStyle(Paint.Style.Stroke);

            Drawable[] layers = { border, GetDrawable(imagePath) };
            LayerDrawable layerDrawable = new LayerDrawable(layers);
            layerDrawable.SetLayerInset(0, 0, 0, 0, 0);
            return layerDrawable;
        }

        private BitmapDrawable GetDrawable(string imagePath)
        {
            var drawable = Resources.GetDrawable(imagePath);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;
            var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, 50, 50, true));
            result.Gravity = Android.Views.GravityFlags.Right;
            return result;
        }
    }
}