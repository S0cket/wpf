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
	/// Логика взаимодействия для MainPage.xaml
	/// </summary>
	public partial class MainPage : Page {
		MainWindow window;
		public MainPage(MainWindow window) {
			InitializeComponent();
			this.window = window;
		}

		private void Info(object sender, RoutedEventArgs e) {
			window.parents.Push(this);
			InfoPage page = new InfoPage(window);
			window.Content = page;
		}

		private void Runner(object sender, RoutedEventArgs e) {
			window.parents.Push(this);
			RunnersSelectPage page = new RunnersSelectPage(window);
			window.Content = page;
		}

		private void Login(object sender, RoutedEventArgs e) {
			window.parents.Push(this);
			LoginPage page = new LoginPage(window);
			window.Content = page;
		}
	}
}
