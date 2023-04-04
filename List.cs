using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SchoolGate
{
    public partial class List : Form
    {
        public List()
        {
            InitializeComponent();
        }

        private void List_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'gateattendance.GateAttendance' table. You can move, or remove it, as needed.
            this.gateAttendanceTableAdapter.Fill(this.gateattendance.GateAttendance);
          
        }
    }
}
