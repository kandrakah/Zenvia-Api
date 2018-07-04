using SampleApp.Converters;
using SampleApp.ViewModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace SampleApp.View
{
    /// <summary>
    /// Lógica interna para SendMessageWindow.xaml
    /// </summary>
    public partial class SendMessageWindow : Window
    {
        public SendMessageWindow(MainViewModel context)
        {
            InitializeComponent();
            DataContext = context;
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void OnPhoneEntered(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
