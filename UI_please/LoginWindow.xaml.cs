using Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UI;
using Path = System.IO.Path;

namespace UI_please
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        public void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            var app = (UI.App)Application.Current;

            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            var user = app._userRepository.GetByName(username);

            if (user == null || user.Password != password)
            {
                MessageBox.Show("Неверный пароль");
                return;
            }

            app.CurrentUser = user;

            var tracksRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MediaPlayerCoursach", "tracks", user.Username);
            if (!Directory.Exists(tracksRoot))
                Directory.CreateDirectory(tracksRoot);
            app.RootDirectory = tracksRoot;

            if (!user.isPayed)
            {
                var payWindow = new PaymentWindow();
                var res = payWindow.ShowDialog();
                if (res != true || !user.isPayed) return;
                else
                {
                    var main = new MainWindow(user);
                    main.Show();
                    Close();
                }
            } else
            {
                var main = new MainWindow(user);
                main.Show();
                Close();
            }
        }

        public void RegestrationButtonClick(object sender, RoutedEventArgs e)
        {
            var app = (UI.App)Application.Current;

            string username = null;
            string password = null;
            User user = null;

            if ((UsernameBox.Text == string.Empty) && (PasswordBox.Password == string.Empty))
            {
                MessageBox.Show("Неверные данные");
            }

            username = UsernameBox.Text;
            password = PasswordBox.Password;
            
            if (!app._dbContext.Users.Any(u => u.Username == username))
            {
                user = new User(username, password);
                app._userRepository.Add(user);
                app.CurrentUser = user;

                var payWindow = new PaymentWindow();
                var res = payWindow.ShowDialog();
                if (res != true || !user.isPayed) return;
                else
                {
                    var main = new MainWindow(user);
                    main.Show();
                    Close();
                }
            }
        }
    }
}
