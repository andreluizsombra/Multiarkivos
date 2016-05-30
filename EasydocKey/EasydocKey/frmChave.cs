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
    public partial class frmChave : Form
    {
        public frmChave()
        {
            InitializeComponent();
        }

        private void frmChave_Load(object sender, EventArgs e)
        {
            txtchave.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                if (txtchave.Text == "") MessageBox.Show("Por favor, preencher campo valor...");
                else
                    txtresultado.Text = new Conexao(txtchave.Text).Encript();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro, "+ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtchave.Text == "") MessageBox.Show("Por favor, preencher campo valor...");
                else
                    txtresultado.Text = new Conexao(txtchave.Text).Decript();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro, "+ex.Message);
            }
        }

        private void btnrestchave_Click(object sender, EventArgs e)
        {
            txtchave.Text = txtresultado.Text;
            txtresultado.Text = "";

        }

        private void frmChave_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnlimpar_Click(object sender, EventArgs e)
        {
            txtchave.Clear();
            txtresultado.Clear();
        }
    }
}
