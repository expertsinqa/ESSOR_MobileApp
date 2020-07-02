using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Susu.CustomControl
{
    public class CustomListViewCell : ViewCell
    {
        public static readonly BindableProperty SelectedBackgroundColorProperty =
            BindableProperty.Create("SelectedBackgroundColor",
                                    typeof(Color),
                                    typeof(CustomListViewCell),
                                    Color.White);

        public Color SelectedBackgroundColor
        {
            get { return (Color)GetValue(SelectedBackgroundColorProperty); }
            set { SetValue(SelectedBackgroundColorProperty, value); }
        }
    }
}
