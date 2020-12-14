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
using System.Security.Cryptography;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace WpfApp1 {
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public bool auth = false;
		public string email = "";
		public Stack<Page> parents = new Stack<Page>();
		private static string addr = "localhost";
		private static string port = "63589";
		private static string user = "root";
		private static string password = "P5UsT78";
		private static string database = "marathon";

		public static string db = String.Format("Server={0};Uid={1};Database={2};Port={3};Pwd={4};", addr, user, database, port, password);
		public MainWindow() {
			InitializeComponent();
			MainPage page = new MainPage(this);
			this.Content = page;
		}

		public static string getSHA256(string str) {
			SHA256 sha = SHA256.Create();
			byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(str));
			string ret = "";
			foreach (byte b in hash)
				ret += b.ToString("x2");
			return ret;
		}

		public static string crypto(string str) {
			string fst_hash = getSHA256(str);
			string snd_hash = getSHA256(fst_hash);
			fst_hash = snd_hash.Substring(0, 32) + fst_hash + snd_hash.Substring(32, 32);
			string ret = getSHA256(fst_hash);
			return ret;
		}

		public static bool DateValid(string date) {
			int[] days = new int[12] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

			if (!Regex.IsMatch(date, @"\d{4}-\d{2}-\d{2}"))
				return false;
			string [] elems = date.Split('-');
			int year = int.Parse(elems[0]);
			int month = int.Parse(elems[1]);
			int day = int.Parse(elems[2]);
			if (year < 1800)
				return false;
			if (month > 12)
				return false;
			if (year % 4 == 0 && year % 100 != 0 || year % 400 == 0)
				days[1]++;
			if (day > days[month - 1])
				return false;

			return true;
		}
	}
}
