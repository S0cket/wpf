using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Controls;

namespace WpfApp1 {
	class Option {
		private uint id;
		private uint price;
		private string description;
		private bool selected;
		
		public Option(uint id, uint price, string description, bool selected) {
			this.id = id;
			this.price = price;
			this.description = description;
			this.selected = selected;
		}
		public uint getid() {
			return this.id;
		}
		public uint getprice() {
			return this.price;
		}
		public string getdesc() {
			return this.description;
		}
		public bool getselect() {
			return this.selected;
		}
	}
}
