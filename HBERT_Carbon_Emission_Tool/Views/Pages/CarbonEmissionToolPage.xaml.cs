using System.Windows;
using System.Windows.Controls;

namespace CarbonEmissionTool.Views
{
    /// <summary>
    /// Interaction logic for CarbonEmissionToolPage.xaml
    /// </summary>
    public partial class CarbonEmissionToolPage : Page
    {
        public CarbonEmissionToolPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PublishPage(this));
        }

        private void RefurbishedButton_Checked(object sender, RoutedEventArgs e)
        {
            this.NewBuildButton.IsChecked = false;
        }

        private void NewBuildButton_Checked(object sender, RoutedEventArgs e)
        {
            this.RefurbishedButton.IsChecked = false;
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var textBox = (TextBox) sender;

            if (textBox.Text.Length == 0)
                textBox.Text = "";
        }
    }
}
