using ProductApp.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductApp
{
	public partial class Form4 : Form
	{
		SqlConnection con;
		SqlCommand cmd;
		SqlDataAdapter da;

		public Form4()
		{
			InitializeComponent();
			ProductDataGridView.MultiSelect = false;
			ProductDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			ProductDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			ProductDataGridView.RowTemplate.Height = 100;

			

		}

		private void SearchButton_Click(object sender, EventArgs e)
		{
			String query = "SELECT * FROM ProductApp_Table WHERE (productname LIKE '%" + SearchTextBox.Text + "%')";
			con = new SqlConnection(Class1.ConnectionString());
			con.Open();
			cmd = new SqlCommand(query, con);
			da = new SqlDataAdapter(cmd);
			DataTable dataTable = new DataTable();
			da.Fill(dataTable);
			ProductDataGridView.DataSource = dataTable;
			((DataGridViewImageColumn)ProductDataGridView.Columns["productimage"]).ImageLayout = DataGridViewImageCellLayout.Stretch;

			ProductDataGridView.Columns["productimage"].DefaultCellStyle.NullValue = Resources.No_Image;
			con.Close();

		}
	}
}
