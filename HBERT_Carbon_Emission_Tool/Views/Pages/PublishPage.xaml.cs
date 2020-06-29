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
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.NavigationService.Navigate(this.PreviousPage);
        }
    }
}
