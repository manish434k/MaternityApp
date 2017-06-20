using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Symlconnect.Maternity.Wpf.Views
{
    /// <summary>
    ///     Interaction logic for FormView.xaml
    /// </summary>
    public partial class FormView
    {
        public FormView()
        {
            InitializeComponent();
        }

        private void OnMenuItemClick(object sender, RoutedEventArgs e)
        {
            var child = FindChild(Form, ((FrameworkElement) sender).DataContext);
            if (child != null)
            {
                var firstTextBlock = FindChild<TextBlock>((FrameworkElement) child);
                Form.ScrollToBottom();
                if (firstTextBlock != null)
                {
                    firstTextBlock.BringIntoView();
                }
                else
                {
                    ((FrameworkElement) child).BringIntoView();
                }
            }
        }

        /// <summary>
        ///     Looks for a child control within a parent by name
        /// </summary>
        public static DependencyObject FindChild(DependencyObject parent, object dataContext)
        {
            // confirm parent and name are valid.
            if (parent == null || dataContext == null)
            {
                return null;
            }

            if (parent is FrameworkElement && ((FrameworkElement) parent).DataContext == dataContext)
            {
                return parent;
            }

            DependencyObject result = null;

            (parent as FrameworkElement)?.ApplyTemplate();

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                result = FindChild(child, dataContext);
                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        ///     Looks for a child control within a parent by type
        /// </summary>
        public static T FindChild<T>(DependencyObject parent)
            where T : DependencyObject
        {
            // confirm parent is valid.
            if (parent == null)
            {
                return null;
            }
            if (parent is T)
            {
                return (T) parent;
            }

            DependencyObject foundChild = null;

            (parent as FrameworkElement)?.ApplyTemplate();

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                foundChild = FindChild<T>(child);
                if (foundChild != null)
                {
                    break;
                }
            }

            return (T) foundChild;
        }
    }
}