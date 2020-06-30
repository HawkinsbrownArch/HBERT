using System;
using System.Windows;
using System.Windows.Controls;

namespace CarbonEmissionTool.Views
{
    /// <summary>
    /// Interaction logic for PublishPage.xaml
    /// </summary>
    public partial class PublishPage : Page
    {
        private Window _windowParent;

        public CarbonEmissionToolPage PreviousPage { get; }

        public event EventHandler ThresholdReached;

        protected virtual void OnThresholdReached(EventArgs e)
        {
            EventHandler handler = ThresholdReached;
            handler?.Invoke(this, e);
        }

        public PublishPage(CarbonEmissionToolPage previousPage, Window window)
        {
            _windowParent = window;

            this.PreviousPage = previousPage;

            InitializeComponent();

            this.Loaded += OnLoaded;
        }

        /// <summary>
        /// Force update the bindings of the required inputs so the validation checks fire.
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.SheetNameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            this.SheetNumberTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void BackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.NavigationService.Navigate(this.PreviousPage);
        }

        private void PublishButton_Click(object sender, RoutedEventArgs e)
        {
            _windowParent.Close();
        }
    }
}
