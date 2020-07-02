using Susu.CustomControl;
using Susu.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomListViewCell), typeof(CustomListViewCellRenderer))]
namespace Susu.iOS.CustomRenderer
{
    public class CustomListViewCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            var view = item as CustomListViewCell;
            cell.SelectedBackgroundView = new UIView
            {
                BackgroundColor = view.SelectedBackgroundColor.ToUIColor(),
            };
            cell.SelectionStyle = UITableViewCellSelectionStyle.Default;
            return cell;
        }
    }
}