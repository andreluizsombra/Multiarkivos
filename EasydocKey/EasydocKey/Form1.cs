using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EasydocKey
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtchave.Text == "196820")
            {
                var frm = new frmChave();
                frm.ShowDialog();
            }
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            txtchave.Focus();
        }
    }
}
