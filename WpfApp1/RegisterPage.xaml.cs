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
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace WpfApp1 {
	/// <summary>
	/// Логика взаимодействия для RegisterPage.xaml
	/// </summary>
	public partial class RegisterPage : Page {
		MainWindow window;
		string placeholder = "yyyy-mm-dd";
		byte[] image = new byte[0];
		string birthdate = "";
		public RegisterPage(MainWindow window) {
			this.window = window;
			InitializeComponent();
			Birthdate.Text = placeholder;
			Birthdate.Style = (Style)this.Resources["AuthPlaceHolder"];
		}

		private void Back(object sender, RoutedEventArgs e) {
			if (window.parents.Count != 0)
				window.Content = window.parents.Pop();
		}

		private void GotFocusBirthdate(object sender, RoutedEventArgs e) {
			if (birthdate == "") {
				Birthdate.Text = "";
				Birthdate.Style = (Style)this.Resources["AuthInput"];
			}
		}

		private void LostFocusBirthdate(object sender, RoutedEventArgs e) {
			birthdate = Birthdate.Text.Trim();
			if (birthdate == "") {
				Birthdate.Text = placeholder;
				Birthdate.Style = (Style)this.Resources["AuthPlaceHolder"];
			}
		}

		private void OpenFile(object sender, RoutedEventArgs e) {
			OpenFileDialog filedialog = new OpenFileDialog();
			filedialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";
			filedialog.ShowDialog();
			image = File.ReadAllBytes(filedialog.FileName);
			BitmapImage img = new BitmapImage(new Uri(filedialog.FileName));
			Imagebox.Source = img;
			Imagebox.Stretch = Stretch.Uniform;
		}

		private void Register(object sender, RoutedEventArgs e) {
			string email = Email.Text;
			string pass1 = Password.Password;
			string pass2 = RepPassword.Password;
			string name = MName.Text;
			string surname = Surname.Text;
			string gender = Gender.Text;
			string country = Country.Text;
			string birthdate = Birthdate.Text.Trim();
			bool isdate = MainWindow.DateValid(birthdate);

			MySqlConnection conn = new MySqlConnection(MainWindow.db);
			MySqlCommand find = new MySqlCommand("select email from runners where email = @email", conn);
			find.Parameters.AddWithValue("@email", email);
			conn.Open();
			MySqlDataReader r = find.ExecuteReader();

			if (email == "" || pass1 == "" || pass2 == "" || name == "" || surname == "" || gender == "" || country == "" || birthdate == "")
				MessageBox.Show("Обязательные поля не заполнены", "Ошибка");
			else if (pass1 != pass2) {
				Password.Password = "";
				RepPassword.Password = "";
				MessageBox.Show("Введённые пароли не совпадают", "Ошибка");
			}
			else if (!isdate)
				MessageBox.Show("Фомат даты неверный", "Ошибка");
			else if (r.Read())
				MessageBox.Show("Этот почтовый адрес уже зарегистрирован", "Ошибка");
			else {

				r.Close();

				string pass = MainWindow.crypto(pass1);

				MySqlCommand cmd = new MySqlCommand("insert into runners(email, password, name, surname, gender, birthdate, country, photo) values (@email, @password, @name, @surname, @gender, @birthdate, @country, @photo)", conn);
				cmd.Parameters.AddWithValue("@email", email);
				cmd.Parameters.AddWithValue("@password", pass);
				cmd.Parameters.AddWithValue("@name", name);
				cmd.Parameters.AddWithValue("@surname", surname);
				cmd.Parameters.AddWithValue("@gender", gender);
				cmd.Parameters.AddWithValue("@birthdate", birthdate);
				cmd.Parameters.AddWithValue("@country", country);
				cmd.Parameters.AddWithValue("@photo", image);
				cmd.ExecuteNonQuery();
				MessageBox.Show("Ваш аккаунт успешно зарегистрирован", "Успех");
				window.parents = new Stack<Page>();
				window.parents.Push(new MainPage(window));
				window.Content = new LoginPage(window);
			}
			conn.Close();
		}
	}
}
