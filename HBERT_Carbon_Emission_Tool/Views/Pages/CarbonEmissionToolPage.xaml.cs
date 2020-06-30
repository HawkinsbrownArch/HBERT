using System;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace CarbonEmissionTool.Views
{
    /// <summary>
    /// Interaction logic for CarbonEmissionToolPage.xaml
    /// </summary>
    public partial class CarbonEmissionToolPage : Page
    {
        private PublishPage PublishPage { get; }

        public CarbonEmissionToolPage(Window window)
        {
            InitializeComponent();

            this.PublishPage = new PublishPage(this, window);

            this.Loaded += OnLoaded;
        }

        /// <summary>
        /// Force update the bindings of the required inputs so the validation checks fire.
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.NameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            this.RevisionTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            this.AreaIntegerUpDown.GetBindingExpression(IntegerUpDown.ValueProperty).UpdateSource();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(this.PublishPage);
        }

        private void RefurbishedButton_Checked(object sender, RoutedEventArgs e)
        {
            this.NewBuildButton.IsChecked = false;
        }

        private void NewBuildButton_Checked(object sender, RoutedEventArgs e)
        {
            this.RefurbishedButton.IsChecked = false;
        }
    }
}
