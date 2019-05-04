using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExportModule
{
    public partial class ExportModule : Form
    {
        public ExportModule()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            ExportToDHCS.Export837P("305");
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            ExportToDHCS.Export837I("305");
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            ExportToDHCS.Export837P("306");
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            ExportToDHCS.Export837I("306");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //load data p-305
            LoadData.SubHist_P305();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //load data i-305
            LoadData.SubHist_I305();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //load data p-306
            LoadData.SubHist_P306();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            //load data i-306
            LoadData.SubHist_I306();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            Export_TP_Extra.Export_TP_Extra_I();
        }
    }
}
