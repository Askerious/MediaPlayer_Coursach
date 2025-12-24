using System;
using System.Collections.Generic;
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

namespace UI_please
{
    /// <summary>
    /// Логика взаимодействия для PaymentWindow.xaml
    /// </summary>
    public partial class PaymentWindow : Window
    {
        public PaymentWindow()
        {
            InitializeComponent();
        }

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            var app = (UI.App)Application.Current;
            var user = app.CurrentUser;

            var num = CredName.Text.Trim().Replace(" ", "");
            if (string.IsNullOrEmpty(num) || num.Length < 12)
            {
                MessageBox.Show("Введите корректный номер карты");
                return;
            }

            if (!int.TryParse(expM.Text, out int month) || !int.TryParse(expY.Text, out int year))
            {
                MessageBox.Show("Введите корректную дату");
                return;
            }

            if (!int.TryParse(CVV.Text, out int cvv))
            {
                MessageBox.Show("Введите корректную дату");
                return;
            }

            var card = new Domain.Credentials(user.Id, num, month, year, cvv, DateTime.Now);

            app._paymentRepository.Add(card);
            user.isPayed = true;
            app._userRepository.Update(user);

            MessageBox.Show("Оплата успешно сохранена.");
            DialogResult = true;
            Close();
        }
    }
}
