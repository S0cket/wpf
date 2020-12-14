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
	/// Логика взаимодействия для InfoPage.xaml
	/// </summary>
	public partial class InfoPage : Page {
		MainWindow window;
		public InfoPage(MainWindow window) {
			InitializeComponent();
			this.window = window;
			Gen();
			//Gen();
		}

		private void Gen() {
			Top.Children.Clear();
			foreach (object e in Main.Children) {
				if (e.GetType() == new Button().GetType()) {
					Button el = (Button)e;
					if (el.Name == "LoginButton") {
						Main.Children.Remove(el);
						break;
					}
				}
			}

			if (window.parents.Count != 0) {
				Button back = new Button();
				back.Style = (Style)this.Resources["BackButton"];
				back.Content = "Назад";
				back.Click += new RoutedEventHandler(Back);
				Top.Children.Add(back);
				DockPanel.SetDock(back, Dock.Left);
			}

			TextBlock head = new TextBlock();
			head.Text = "MARATHON SKILLS 2016";
			head.Style = (Style)this.Resources["MenuHeader1"];
			Top.Children.Add(head);
			DockPanel.SetDock(head, Dock.Left);

			if (window.auth) {
				Button logout = new Button();
				logout.Style = (Style)this.Resources["LogoutButton"];
				logout.Content = "Logout";
				logout.Click += new RoutedEventHandler(Logout);
				Top.Children.Add(logout);
				DockPanel.SetDock(logout, Dock.Right);

				Label label = new Label();
				Top.Children.Add(label);
				DockPanel.SetDock(label, Dock.Right);
			}
			else {
				Button login = new Button();
				login.Style = (Style)this.Resources["LoginButton"];
				login.Content = "Login";
				login.Click += new RoutedEventHandler(Login);
				login.Name = "LoginButton";
				Main.Children.Add(login);
				DockPanel.SetDock(login, Dock.Bottom);

				Label label = new Label();
				Main.Children.Add(label);
				DockPanel.SetDock(label, Dock.Top);
			}
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

		private void Logout(object sender, RoutedEventArgs e) {
			window.auth = false;
			window.parents = new Stack<Page>();
			window.parents.Push(new MainPage(window));
			window.parents.Push(new LoginPage(window));
			Gen();
		}
	}
}
