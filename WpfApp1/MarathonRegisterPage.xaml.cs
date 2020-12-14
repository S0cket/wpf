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
using System.Text.RegularExpressions;

namespace WpfApp1 {
	/// <summary>
	/// Логика взаимодействия для MarathonRegisterPage.xaml
	/// </summary>
	public partial class MarathonRegisterPage : Page {
		MainWindow window;
		uint cost = 0;
		uint fundcost = 0;
		uint cur_price = 0;
		List<Option> options;
		uint fullcost = 145;
		uint halfcost = 75;
		uint shortcost = 20;
		string fund = "";
		int CheckedOption = -1;

		public MarathonRegisterPage(MainWindow window) {
			this.window = window;
			InitializeComponent();
			Full.Content = String.Format("42km Полный марафон (${0})", this.fullcost);
			Half.Content = String.Format("21km Полумарафон (${0})", this.halfcost);
			Short.Content = String.Format("5km Малая дистанция (${0})", this.shortcost);
			InitOptions();
			InitFundes();

			Refresh();
		}

		private void InitOptions() {
			OptionBlock.Children.Clear();
			List<Option> lst = new List<Option>();
			MySqlConnection conn = new MySqlConnection(MainWindow.db);
			MySqlCommand cmd = new MySqlCommand("select * from options", conn);
			conn.Open();
			MySqlDataReader r = cmd.ExecuteReader();
			while (r.Read()) {
				uint id = Convert.ToUInt32(r["id"]);
				uint price = Convert.ToUInt32(r["price"]);
				string description = r["description"].ToString();
				bool selected = Convert.ToBoolean(r["selected"]);
				lst.Add(new Option(id, price, description, selected));
			}
			this.options = lst;
			r.Close();
			cmd.Cancel();
			conn.Close();
			bool flag = false;
			foreach (Option opt in this.options) {
				RadioButton rbtn = new RadioButton();
				rbtn.Name = "N" + opt.getid().ToString();
				rbtn.GroupName = "Option";
				rbtn.HorizontalAlignment = HorizontalAlignment.Left;
				rbtn.Checked += CheckOption;
				rbtn.Unchecked += UncheckOption;
				if (!flag && opt.getselect()) {
					flag = true;
					rbtn.IsChecked = true;
				}
				TextBlock text = new TextBlock();
				text.Inlines.Add(new Bold(new Run(String.Format("Вариант №{0}(${1}): ", opt.getid(), opt.getprice()))));
				text.Inlines.Add(new Run(opt.getdesc()));
				rbtn.Content = text;
				OptionBlock.Children.Add(rbtn);
			}
		}

		private void InitFundes() {
			MySqlConnection conn = new MySqlConnection(MainWindow.db);
			MySqlCommand cmd = new MySqlCommand("select * from fundes", conn);
			conn.Open();
			MySqlDataReader r = cmd.ExecuteReader();
			while (r.Read()) {
				ComboBoxItem item = new ComboBoxItem();
				TextBlock text = new TextBlock();
				text.Text = r["name"].ToString();
				item.Content = text;
				Fund.Items.Add(item);
			}
			r.Close();
			conn.Close();
		}

		private void Refresh() {
			uint cost = this.cost + fundcost;
			Cost.Text = cost.ToString() + "$";
		}

		private void CheckOption(object sender, RoutedEventArgs e) {
			RadioButton rbtn = (RadioButton)sender;
			string name = rbtn.Name;
			uint id = Convert.ToUInt32(name.Remove(0, 1));
			foreach (Option opt in this.options) {
				if (opt.getid() == id) {
					this.cost += opt.getprice();
					this.cur_price = opt.getprice();
					break;
				}
			}
			Refresh();
			this.CheckedOption = Convert.ToInt32(id);
		}

		private void UncheckOption(object sender, RoutedEventArgs e) {
			this.cost -= this.cur_price;
			Refresh();
		}

		private void Check(object sender, RoutedEventArgs e) {
			if (sender == Full)
				cost += this.fullcost;
			else if (sender == Half)
				cost += this.halfcost;
			else
				cost += this.shortcost;
			Refresh();
		}

		private void Uncheck(object sender, RoutedEventArgs e) {
			if (sender == Full)
				cost -= this.fullcost;
			else if (sender == Half)
				cost -= this.halfcost;
			else
				cost -= this.shortcost;
			Refresh();
		}

		private void LostFocusCostFund(object sender, RoutedEventArgs e) {
			Regex re = new Regex(@"\d+");
			if (!re.IsMatch(CostFund.Text) && CostFund.Text != "") {
				if (sender != null)
					MessageBox.Show("Вводите только цифры", "Неверный формат");
				fundcost = 0;
				CostFund.Text = "0";
			}
			else if (re.IsMatch(CostFund.Text)) {
				if (this.fund != "" && Convert.ToInt32(CostFund.Text) > 0)
					fundcost = Convert.ToUInt32(CostFund.Text);
				else { 
					fundcost = 0;
					CostFund.Text = "0";
				}
			}
			else if (CostFund.Text == "") {
				fundcost = 0;
				CostFund.Text = "0";
			}
			Refresh();
		}

		private void SelectFund(object sender, RoutedEventArgs e) {
			if (!this.IsLoaded) {
				return;
			}
			ComboBox box = (ComboBox)sender;
			ComboBoxItem item = (ComboBoxItem)box.SelectedItem;
			if (item == None) {
				fund = "";
				fundcost = 0;
				CostFund.Text = "0";
				CostFund.IsReadOnly = true;
			}
			else {
				CostFund.IsReadOnly = false;
				TextBlock text = (TextBlock)item.Content;
				fund = text.Text;
			}
			LostFocusCostFund(null, null);
			Refresh();
		}

		private void Register(object sender, RoutedEventArgs e) {
			Regex re = new Regex(@"\d+");
			if (!(bool)this.Full.IsChecked && !(bool)this.Half.IsChecked && !(bool)this.Short.IsChecked) {
				MessageBox.Show("Выберите вид марафона", "Ошибка");
			}
			else if (this.CheckedOption == -1) {
				MessageBox.Show("Выберите варианты комплектов", "Ошибка");
			}
			else if (!re.IsMatch(CostFund.Text)) {
				if (fund != "")
					MessageBox.Show("Введите сумму", "Ошибка");
			}
			else {
				string msg;
				if (fund == "") {
					msg = String.Format("Вы уверены? Общая сумма: {0}$ (из них 0$ на счёт фондов)", this.cost);
				}
				else {
					msg = String.Format("Вы уверены? Общая сумма: {0}$ (из них {1}$ на счёт фонда: {2})", this.cost + this.fundcost, this.fundcost, this.fund);
				}
				MessageBoxResult result = MessageBox.Show(msg, "Подтверждение", MessageBoxButton.YesNo);
				if (result == MessageBoxResult.Yes) {
					MySqlConnection conn = new MySqlConnection(MainWindow.db);
					MySqlCommand cmd = new MySqlCommand("insert into marathon(email, full, half, short, fund, sfund, option) values(@email, @full, @half, @short, @fund, @sfund, @option)", conn);
					cmd.Parameters.AddWithValue("@email", window.email);
					cmd.Parameters.AddWithValue("@full", (bool)this.Full.IsChecked);
					cmd.Parameters.AddWithValue("@half", (bool)this.Half.IsChecked);
					cmd.Parameters.AddWithValue("@short", (bool)this.Short.IsChecked);
					cmd.Parameters.AddWithValue("@fund", this.fund);
					cmd.Parameters.AddWithValue("@sfund", this.fundcost);
					cmd.Parameters.AddWithValue("@option", this.CheckedOption);
					conn.Open();
					cmd.ExecuteNonQuery();
					conn.Close();
					MessageBox.Show("Вы успешно зарегистрированы на марафон", "Успех");

					try {
						AccountPage page = (AccountPage)window.parents.Pop();
						page.Update();
						window.Content = page;
					}
					catch (Exception exc) {
						MessageBox.Show(exc.Message);
					}
				}
			}
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
	}
}
