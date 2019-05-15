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
            this.toolStripButton4.Click += new System.EventHandler(async (s1, e1) => await this.toolStripButton4_ClickAsync());
            this.toolStripButton6.Click += new System.EventHandler(async (s1, e1) => await this.toolStripButton6_ClickAsync());
            this.toolStripButton5.Click += new System.EventHandler(async (s1, e1) => await this.toolStripButton5_ClickAsync());
            this.toolStripButton7.Click += new System.EventHandler(async (s1, e1) => await this.toolStripButton7_ClickAsync());
            this.toolStripButton1.Click += new System.EventHandler(async (s1, e1) => await this.toolStripButton1_ClickAsync());
            this.toolStripButton2.Click += new System.EventHandler(async (s1, e1) => await this.toolStripButton2_ClickAsync());
            this.toolStripButton3.Click += new System.EventHandler(async (s1, e1) => await this.toolStripButton3_ClickAsync());
            this.toolStripButton8.Click += new System.EventHandler(async (s1, e1) => await this.toolStripButton8_ClickAsync());
            this.toolStripButton9.Click += new System.EventHandler(async (s1, e1) => await this.toolStripButton9_ClickAsync());
        }
        private async Task toolStripButton4_ClickAsync()
        {
            await ExportToDHCS.Export837PAsync("305");
        }
        private async Task toolStripButton6_ClickAsync()
        {
            await ExportToDHCS.Export837PAsync("306");
        }
        private async Task toolStripButton5_ClickAsync()
        {
            await ExportToDHCS.Export837IAsync("305");
        }
        private async Task toolStripButton7_ClickAsync()
        {
            await ExportToDHCS.Export837IAsync("306");
        }
        private async Task toolStripButton1_ClickAsync()
        {
            //load data, xml document to staging tables, professional
            await LoadData.SubHist_P305Async();
        }

        private async Task toolStripButton2_ClickAsync()
        {
            //load data, xml document to staging tables, institutional
            await LoadData.SubHist_I305Async();
        }

        private async Task toolStripButton3_ClickAsync()
        {
            //load xml documents, professional
            await LoadData.SubHist_P306Async();
        }

        private async Task toolStripButton8_ClickAsync()
        {
            //load xml documents, institutional
            await LoadData.SubHist_I306Async();
        }

        private async Task toolStripButton9_ClickAsync()
        {
            await Export_TP_Extra.Export_TP_Extra_IAsync();
        }
    }
}
