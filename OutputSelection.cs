using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TogglExport
{
    public partial class OutputSelection : Form
    {
        public int selection = 0;
        public OutputSelection()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            selection = cmbFormats.SelectedIndex;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
