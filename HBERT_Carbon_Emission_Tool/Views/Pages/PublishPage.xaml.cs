using System.Windows;
using System.Windows.Controls;

namespace CarbonEmissionTool.Views
{
    /// <summary>
    /// Interaction logic for PublishPage.xaml
    /// </summary>
    public partial class PublishPage : Page
    {
        public CarbonEmissionToolPage PreviousPage { get; }

        public PublishPage(CarbonEmissionToolPage previousPage)
        {
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

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.NavigationService.Navigate(this.PreviousPage);
        }
    }
}
