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
using System.IO;
using MySql.Data.MySqlClient;

namespace WpfApp1 {
	/// <summary>
	/// Логика взаимодействия для AccountPage.xaml
	/// </summary>
	public partial class AccountPage : Page {
		MainWindow window;
		string email;
		string name;
		string surname;
		string gender;
		string birthdate;
		string country;
		byte[] image;
		public bool isregister;
		public AccountPage(MainWindow window) {
			this.window = window;
			InitializeComponent();
			Init();

			try {
				MemoryStream mem = new MemoryStream(this.image);
				Photo.Source = BitmapFrame.Create(mem);
			}
			catch (Exception exc) {}
			Email.Text = this.email;
			SurnameName.Text = this.surname + " " + this.name;
			Gender.Text = this.gender;
			Birthdate.Text = this.birthdate.Split(' ')[0];
			Country.Text = this.country;
			IsRegister.Text = (this.isregister) ? "Да" : "Нет";

		}

		private void Init() {
			this.email = window.email;
			MySqlConnection conn = new MySqlConnection(MainWindow.db);
			MySqlCommand cmd = new MySqlCommand("select * from runners where email = @email", conn);
			cmd.Parameters.AddWithValue("@email", this.email);
			conn.Open();
			MySqlDataReader r = cmd.ExecuteReader();
			r.Read();
			this.name = r["name"].ToString();
			this.surname = r["surname"].ToString();
			this.gender = r["gender"].ToString();
			this.birthdate = r["birthdate"].ToString();
			this.country = r["country"].ToString();
			this.image = (byte[])r["photo"];
			r.Close();

			cmd = new MySqlCommand("select email from marathon where email = @email", conn);
			cmd.Parameters.AddWithValue("@email", this.email);
			r = cmd.ExecuteReader();
			this.isregister = r.Read();

			conn.Close();
		}

		public void Update() {
			MySqlConnection conn = new MySqlConnection(MainWindow.db);
			conn.Open();
			MySqlCommand cmd = new MySqlCommand("select email from marathon where email = @email", conn);
			cmd.Parameters.AddWithValue("@email", this.email);
			MySqlDataReader r = cmd.ExecuteReader();
			this.isregister = r.Read();
			r.Close();
			conn.Close();
			IsRegister.Text = (this.isregister) ? "Да" : "Нет";
		}

		private void Logout(object sender, RoutedEventArgs e) {
			window.auth = false;
			window.parents = new Stack<Page>();
			window.parents.Push(new MainPage(window));
			window.Content = new LoginPage(window);
		}

		private void Register(object sender, RoutedEventArgs e) {
			if (this.isregister) {
				MessageBox.Show("Вы уже зарегистрированы", "Ошибка");
			}
			else {
				try {
					this.window.parents.Push(this);
					this.window.Content = new MarathonRegisterPage(window);
				}
				catch (Exception exc) {
					MessageBox.Show(exc.Message);
				}
			}
		}

		private void ListRunners(object sender, RoutedEventArgs e) {
			window.parents.Push(this);
			ListRunnersPage page = new ListRunnersPage(window);
			window.Content = page;
		}

		private void Info(object sender, RoutedEventArgs e) {
			window.parents.Push(this);
			window.Content = new InfoPage(window);
		}
	}
}
