using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace WpfApp1 {
	/// <summary>
	/// Логика взаимодействия для LoginPage.xaml
	/// </summary>
	public partial class LoginPage : Page {
		MainWindow window;
		public LoginPage(MainWindow window) {
			InitializeComponent();
			this.window = window;
		}

		private void Back(object sender, RoutedEventArgs e) {
			if (window.parents.Count != 0)
				window.Content = window.parents.Pop();
		}

		private void Login(object sender, RoutedEventArgs e) {
			string email = Email.Text;
			string pass = MainWindow.crypto(Password.Password);
			MySqlConnection conn = new MySqlConnection(MainWindow.db);
			MySqlCommand cmd = new MySqlCommand("select email, password from runners where email = @email and password = @password", conn);
			cmd.Parameters.AddWithValue("@email", email);
			cmd.Parameters.AddWithValue("@password", pass);
			conn.Open();
			MySqlDataReader r = cmd.ExecuteReader();
			if (r.Read()) {
				window.auth = true;
				window.email = r["email"].ToString();
				window.parents = new Stack<Page>();
				AccountPage page = new AccountPage(window);
				window.Content = page;
			}
			else {
				MessageBox.Show("Неверный логин или пароль", "Ошибка");
			}
			conn.Close();
		}
	}
}
