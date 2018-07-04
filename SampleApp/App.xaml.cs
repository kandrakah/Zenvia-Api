using SampleApp.Context;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SampleApp
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            InitializeDBContext();

            var win = new MainWindow();
            win.Show();
        }

        private void InitializeDBContext()
        {
            var context = new AppDBContext();
            context.CreateDB();
        }
    }
}
