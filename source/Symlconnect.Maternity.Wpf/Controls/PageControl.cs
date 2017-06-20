using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Symlconnect.Maternity.Wpf
{
    /// <summary>
    ///     Simple control to handle the common layout (back button and related command, title and subtitle, content, buttons
    ///     at the bottom)
    /// </summary>
    public class PageControl : ContentControl
    {
        static PageControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageControl),
                new FrameworkPropertyMetadata(typeof(PageControl)));
        }

        public static readonly DependencyProperty BackButtonVisibilityProperty = DependencyProperty.Register(
            "BackButtonVisibility", typeof(Visibility), typeof(PageControl), new PropertyMetadata(Visibility.Collapsed));

        public Visibility BackButtonVisibility
        {
            get { return (Visibility) GetValue(BackButtonVisibilityProperty); }
            set { SetValue(BackButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(PageControl), new PropertyMetadata(default(string)));

        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(
            "Subtitle", typeof(string), typeof(PageControl), new PropertyMetadata(default(string)));

        public string Subtitle
        {
            get { return (string) GetValue(SubtitleProperty); }
            set { SetValue(SubtitleProperty, value); }
        }

        public static readonly DependencyProperty TrayContentProperty = DependencyProperty.Register(
            "TrayContent", typeof(object), typeof(PageControl), new PropertyMetadata(default(object)));

        public object TrayContent
        {
            get { return GetValue(TrayContentProperty); }
            set { SetValue(TrayContentProperty, value); }
        }

        public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register(
            "HeaderContent", typeof(object), typeof(PageControl), new PropertyMetadata(default(object)));

        public object HeaderContent
        {
            get { return GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        public static readonly DependencyProperty NavigateBackCommandProperty = DependencyProperty.Register(
            "NavigateBackCommand", typeof(ICommand), typeof(PageControl), new PropertyMetadata(default(ICommand)));

        public ICommand NavigateBackCommand
        {
            get { return (ICommand) GetValue(NavigateBackCommandProperty); }
            set { SetValue(NavigateBackCommandProperty, value); }
        }

        public static readonly DependencyProperty ContentMarginProperty = DependencyProperty.Register(
            "ContentMargin", typeof(Thickness), typeof(PageControl), new PropertyMetadata(new Thickness(43,11,11,11)));

        public Thickness ContentMargin
        {
            get { return (Thickness) GetValue(ContentMarginProperty); }
            set { SetValue(ContentMarginProperty, value); }
        }
    }
}