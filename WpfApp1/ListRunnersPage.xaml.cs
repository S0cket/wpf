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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace WpfApp1 {
	/// <summary>
	/// Логика взаимодействия для ListRunnersPage.xaml
	/// </summary>
	public partial class ListRunnersPage : Page {
		MainWindow window;
		public ListRunnersPage(MainWindow window) {
			this.window = window;
			InitializeComponent();
		}

		private void Back(object sender, RoutedEventArgs e) {
			if (window.parents.Count != 0)
				window.Content = window.parents.Pop();
		}

		private void Logout(object sender, RoutedEventArgs e) {
			window.auth = false;
			window.parents = new Stack<Page>();
			window.parents.Push(new MainPage(window));
			window.Content = new LoginPage(window);
		}

		private void Refresh(object sender, RoutedEventArgs e) {
			
		}

		private void Init() {
			MySqlConnection conn = new MySqlConnection(MainWindow.db);
			MySqlCommand cmd = new MySqlCommand("select marathon.email, runners.name, runners.surname, runners.gender, runners.country, runners.photo from marathon join runners on marathon.email = runners.email order by runners.surname", conn);
			conn.Open();
			MySqlDataReader r = cmd.ExecuteReader();
			while (r.Read()) {

			}
		}
	}
}
