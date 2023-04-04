using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using System.Data.SqlClient;

namespace SchoolGate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        FilterInfoCollection FilterInfoCollection;
        VideoCaptureDevice VideoCaptureDevice;
        private void button1_Click(object sender, EventArgs e)
        {
            VideoCaptureDevice = new VideoCaptureDevice(FilterInfoCollection[comboBox1.SelectedIndex].MonikerString);
            VideoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            VideoCaptureDevice.Start();
        }
        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FilterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in FilterInfoCollection)
                comboBox1.Items.Add(filterInfo.Name);

            comboBox1.SelectedIndex = 0;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            using (SqlConnection con = connection.CONN())
            {
                if (pictureBox1.Image != null)
                {
                    BarcodeReader barcodeReader = new BarcodeReader();
                    Result result = barcodeReader.Decode((Bitmap)pictureBox1.Image);
                    if (result != null)
                    {
                        textBox1.Text = result.ToString();
                        timer1.Stop();
                        if (VideoCaptureDevice.IsRunning == true)
                        {
                            VideoCaptureDevice.Stop();
                        }

                        SqlCommand cmd = new SqlCommand("select * from [dbo].[ImportData] where [QrCode] = '" + textBox1.Text + "'", con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds1 = new DataSet();
                        da.Fill(ds1);
                        int i = ds1.Tables[0].Rows.Count;
                        if (i > 0)
                        {
                            string Name = Convert.ToString(ds1.Tables[0].Rows[0]["Name"]);
                            string RegNo = Convert.ToString(ds1.Tables[0].Rows[0]["RegNo"]);
                            string Form = Convert.ToString(ds1.Tables[0].Rows[0]["Form"]);                                                 
                                // If the student doesn't exist, insert a new record
                                SqlCommand insertCmd = new SqlCommand("INSERT INTO [dbo].[GateAttendance] ([StudentName],[RegNo],[Form] ,[Date]) VALUES (@Studentname, @Regno, @Form, @Date)", con);
                                insertCmd.Parameters.AddWithValue("@Studentname", Name);
                                insertCmd.Parameters.AddWithValue("@Regno", RegNo);
                                insertCmd.Parameters.AddWithValue("@Form", Form);
                                insertCmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString());
                                insertCmd.ExecuteNonQuery();
   
                                MessageBox.Show("SUCCESSFUL!!!!!!");
                            List list = new List();
                            list.Show();
                            this.Close();
                          
                        }
                        else
                        {
                            MessageBox.Show("tUZIDI");
                        }
                    }
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (VideoCaptureDevice.IsRunning == true)
            {
                VideoCaptureDevice.Stop();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List list = new List();
            list.Show();
        }
    }
}
