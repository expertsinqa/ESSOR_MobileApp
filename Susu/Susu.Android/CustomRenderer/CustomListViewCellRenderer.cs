using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Susu.CustomControl;
using Susu.Droid.CustomRenderer;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomListViewCell), typeof(CustomListViewCellRenderer))]
namespace Susu.Droid.CustomRenderer
{
    public class CustomListViewCellRenderer : ViewCellRenderer
    {
        private Android.Views.View cellCore;
        private Drawable _unselectedBackground;
        private bool _selected;
        protected override Android.Views.View GetCellCore(Cell item,
                                                         Android.Views.View convertView,
                                                         ViewGroup parent,
                                                         Context context)
        {
            cellCore = base.GetCellCore(item, convertView, parent, context);

            _selected = false;
            _unselectedBackground = cellCore.Background;

            return cellCore;
        }
        protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnCellPropertyChanged(sender, args);

            try
            {
                if (args.PropertyName == "IsSelected")
                {
                    _selected = !_selected;

                    if (_selected)
                    {
                        var extendedViewCell = sender as CustomListViewCell;
                        cellCore.SetBackgroundColor(extendedViewCell.SelectedBackgroundColor.ToAndroid());
                    }
                    else
                    {
                        cellCore.SetBackground(_unselectedBackground);
                    }
                }
            }
            catch (System.Exception)
            {
            }
        }
    }


    }