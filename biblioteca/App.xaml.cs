using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media.Animation;

namespace biblioteca
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            using var db = new Data.LibraryContext();
            db.Database.EnsureCreated();

            base.OnStartup(e);
        }
    }
}
