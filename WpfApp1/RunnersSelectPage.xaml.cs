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

namespace WpfApp1 {
	/// <summary>
	/// Логика взаимодействия для RunnersSelectPage.xaml
	/// </summary>
	public partial class RunnersSelectPage : Page {
		MainWindow window;
		public RunnersSelectPage(MainWindow window) {
			InitializeComponent();
			this.window = window;
		}

		private void Back(object sender, RoutedEventArgs e) {
			if (window.parents.Count != 0)
				window.Content = window.parents.Pop();
		}

		private void Login(object sender, RoutedEventArgs e) {
			window.parents.Push(this);
			LoginPage page = new LoginPage(window);
			window.Content = page;
		}

		private void Register(object sender, RoutedEventArgs e) {
			window.parents.Push(this);
			RegisterPage page = new RegisterPage(window);
			window.Content = page;
		}
	}
}
