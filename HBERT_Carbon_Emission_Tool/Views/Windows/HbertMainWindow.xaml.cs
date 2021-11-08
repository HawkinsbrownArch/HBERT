using System.Windows;
using CarbonEmissionTool.ViewModels;

namespace CarbonEmissionTool.Views
{
    /// <summary>
    /// Interaction logic for HbertMainWindow.xaml
    /// </summary>
    public partial class HbertMainWindow : Window
    {
        public HbertMainWindow()
        {
            InitializeComponent();

            var carbonEmissionToolPage = new CarbonEmissionToolPage(this);

            this.PageFrame.Content = carbonEmissionToolPage;
        }
    }
}
