using ProductApp.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductApp
{
	public partial class Form1 : Form
	{
		SqlConnection con;
		SqlCommand cmd;
		SqlDataAdapter da;
		SqlDataReader dr;

		public Form1()
		{
			InitializeComponent();
			LoadIntoDataGridView1(ProductDataGridView);
		}

		private DataTable LoadIntoDataGridView1(DataGridView ProductDataGridView)
		{
			String query = "SELECT * FROM ProductApp_Table";
			con = new SqlConnection(Class1.ConnectionString());
			con.Open();
			da = new SqlDataAdapter(query, con);
			DataTable dataTable = new DataTable();
			da.Fill(dataTable);
			ProductDataGridView.DataSource = dataTable;
			ProductDataGridView.MultiSelect = false;
			ProductDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			ProductDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			((DataGridViewImageColumn)ProductDataGridView.Columns["productimage"]).ImageLayout = DataGridViewImageCellLayout.Stretch;
			ProductDataGridView.RowTemplate.Height = 100;
			ProductDataGridView.Columns["productimage"].DefaultCellStyle.NullValue = Resources.No_Image;

			con.Close();
			return dataTable;
		}

		private void F2_UpdateEventHandler(object sender, Form2.UpdateEventArgs args)
		{
			ProductDataGridView.DataSource = LoadIntoDataGridView1(ProductDataGridView);
		}

		private void F3_UpdateEventHandler(object sender, Form3.UpdateEventArgs args)
		{
			ProductDataGridView.DataSource = LoadIntoDataGridView1(ProductDataGridView);
		}

		private void AddToolStripButton_Click(object sender, EventArgs e)
		{
			Form2 form2 = new Form2(this);
			form2.UpdateEventHandler += F2_UpdateEventHandler;
			form2.ShowDialog();
		}

		private void RefreshToolStripButton_Click(object sender, EventArgs e)
		{
			LoadIntoDataGridView1(ProductDataGridView);
		}

		private void DeleteToolStripButton_Click(object sender, EventArgs e)
		{
			int rowIndex = ProductDataGridView.Rows.GetFirstRow(DataGridViewElementStates.Selected);

			try
			{
				if (rowIndex != -1)
				{
					int id = Convert.ToInt32(ProductDataGridView.Rows[rowIndex].Cells["id"].Value);
					String query = "DELETE FROM ProductApp_Table WHERE id='" + id + "';";
					con = new SqlConnection(Class1.ConnectionString());
					cmd = new SqlCommand(query, con);
					con.Open();
					dr = cmd.ExecuteReader();
					MessageBox.Show("Row Deleted", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
					con.Close();

					LoadIntoDataGridView1(ProductDataGridView);
				}
				else
				{
					MessageBox.Show("Please Select Row", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void UpdateToolStripButton_Click(object sender, EventArgs e)
		{
			Form3 form3 = new Form3(this);

			form3.IDTextBox.Text = ProductDataGridView.CurrentRow.Cells[0].Value.ToString();
			form3.ProductNameTextBox.Text = ProductDataGridView.CurrentRow.Cells[1].Value.ToString();
			form3.PurchaseDateTimePicker.Value = DateTime.Parse(ProductDataGridView.CurrentRow.Cells[2].Value.ToString());
			form3.PurchaseTimePicker.Value = DateTime.Parse(ProductDataGridView.CurrentRow.Cells[2].Value.ToString());
			form3.PriceTextBox.Text = ProductDataGridView.CurrentRow.Cells[3].Value.ToString();
			form3.CustomerNameTextBox.Text = ProductDataGridView.CurrentRow.Cells[4].Value.ToString();
			form3.ShopNameTextBox.Text = ProductDataGridView.CurrentRow.Cells[5].Value.ToString();

			try
			{
				if (ProductDataGridView.CurrentRow.Cells[6].Value.ToString() != "")
				{
					Byte[] productImage = (Byte[])(ProductDataGridView.CurrentRow.Cells[6].Value);
										
					MemoryStream memoryStream = new MemoryStream(productImage);
					form3.ProductPictureBox.Image = Image.FromStream(memoryStream);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			form3.UpdateEventHandler += F3_UpdateEventHandler;
			form3.ShowDialog();
		}

		private void SearchToolStripButton_Click(object sender, EventArgs e)
		{
			Form4 form4 = new Form4();
			form4.ShowDialog();
		}

		private void CloseToolStripButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void ContactUsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			const String URL = "https://www.wowapp.com/w/mhamza2557/MH-Hamza";
			System.Diagnostics.Process.Start(URL);
		}
	}
}
