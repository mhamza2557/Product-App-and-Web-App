using AForge.Video;
using AForge.Video.DirectShow;
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
	public partial class Form3 : Form
	{
		FilterInfoCollection Devices;
		VideoCaptureDevice frame;
		SqlConnection con;
		SqlCommand cmd;
		String imageLocation = "";

		public Form3(Form1 form1)
		{
			InitializeComponent();
			IDTextBox.Enabled = false;

			PurchaseTimePicker.Format = DateTimePickerFormat.Time;
			PurchaseTimePicker.ShowUpDown = true;
			Controls.Add(PurchaseTimePicker);

			CaptureButton.Enabled = false;

			Devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

			for (int i = 0; i < Devices.Count; i++)
			{
				String cameraName = Devices[i].Name;
				CameraComboBox.Items.Add(cameraName);
			}
		}

		public delegate void UpdateDelegate(object sender, UpdateEventArgs args);
		public event UpdateDelegate UpdateEventHandler;

		public class UpdateEventArgs : EventArgs
		{
			public String Data { get; set; }
		}

		protected void Insert()
		{
			UpdateEventArgs args = new UpdateEventArgs();
			UpdateEventHandler.Invoke(this, args);
		}

		private void Start_Camera()
		{
			if (CameraComboBox.SelectedIndex != -1)
			{
				Devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
				frame = new VideoCaptureDevice(Devices[CameraComboBox.SelectedIndex].MonikerString);
				frame.NewFrame += new NewFrameEventHandler(NewFrame_Event);
				frame.Start();

				BrowseButton.Enabled = false;
				StartCameraButton.Enabled = false;
				SaveButton.Enabled = false;
				CaptureButton.Enabled = true;
			}
			else
			{
				MessageBox.Show("Select Camera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void NewFrame_Event(object send, NewFrameEventArgs e)
		{
			try { CameraPictureBox.Image = (Image)e.Frame.Clone(); }
			catch (Exception ex) { MessageBox.Show(ex.Message); }
		}

		private void BrowseButton_Click(object sender, EventArgs e)
		{
			try
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();

				openFileDialog.Filter = "JPEG Files (*.JPG;*.JPEG;*.JPE;*.JFIF) |*.JPG;*.JPEG;*.JPE;*.JFIF|" +
										"GIF Files (*.GIF) |*.GIF|" +
										"PNG Files (*.PNG) |*.PNG";
				openFileDialog.Title = "Product Image";

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					imageLocation = openFileDialog.FileName.ToString();
					ProductPictureBox.ImageLocation = imageLocation;
				}
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex);
			}
		}

		[Obsolete]
		private void SaveButton_Click(object sender, EventArgs e)
		{
			if (ProductNameTextBox.Text != "" && PriceTextBox.Text != "" && CustomerNameTextBox.Text != "")
			{
				Byte[] productImage = null;

				MemoryStream memoryStream = new MemoryStream();
				if (ProductPictureBox.Image != null)
				{
					ProductPictureBox.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
					productImage = memoryStream.ToArray();
				}

				String query = "UPDATE ProductApp_Table SET productname=@productname, purchasedate=@purchasedate, price=@price, customername=@customername, shopname=@shopname WHERE id=@id";

				if (productImage != null)
				{
					query = "UPDATE ProductApp_Table SET productname=@productname, purchasedate=@purchasedate, price=@price, customername=@customername, shopname=@shopname, productimage=@productimage WHERE id=@id";
				}

				con = new SqlConnection(Class1.ConnectionString());
				con.Open();
				cmd = new SqlCommand(query, con);

				String id = IDTextBox.Text;
				String productName = ProductNameTextBox.Text;
				String getDateTime = PurchaseDateTimePicker.Value.Day.ToString() + "/"
								   + PurchaseDateTimePicker.Value.Month.ToString() + "/"
								   + PurchaseDateTimePicker.Value.Year.ToString() + " "
								   + PurchaseTimePicker.Value.Hour.ToString() + ":"
								   + PurchaseTimePicker.Value.Minute.ToString() + ":"
								   + PurchaseTimePicker.Value.Second.ToString();
				DateTime purchaseDate = DateTime.Parse(getDateTime);
				String price = PriceTextBox.Text;
				String customerName = CustomerNameTextBox.Text;
				String shopName = "";
				if (ShopNameTextBox.Text != "") { shopName = ShopNameTextBox.Text; }
				
				cmd.Parameters.Add("@id", id);
				cmd.Parameters.Add("@productname", productName);
				cmd.Parameters.Add("@purchasedate", purchaseDate);
				cmd.Parameters.Add("@price", price);
				cmd.Parameters.Add("@customername", customerName);
				cmd.Parameters.Add("@shopname", shopName);
				if (productImage != null) { cmd.Parameters.Add("@productimage", productImage); }
				cmd.ExecuteNonQuery();
				con.Close();
				DialogResult result = MessageBox.Show("Database Updated", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
				
				if (result == DialogResult.OK)
				{
					Insert();
					this.Close();
				}
			}
			else
			{
				MessageBox.Show("Enter Correct Information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void CaptureButton_Click(object sender, EventArgs e)
		{
			ProductPictureBox.Image = CameraPictureBox.Image;
			frame.Stop();
			CameraPictureBox.Image = null;
			BrowseButton.Enabled = true;
			StartCameraButton.Enabled = true;
			SaveButton.Enabled = true;
			CaptureButton.Enabled = false;
		}

		private void StartCameraButton_Click(object sender, EventArgs e)
		{
			Start_Camera();
		}

		private void Form3_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (StartCameraButton.Enabled == false) { frame.Stop(); }
		}
	}
}
