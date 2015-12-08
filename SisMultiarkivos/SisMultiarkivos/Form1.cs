using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SisMultiarkivos
{
    public partial class frmPrincipal : Form
    {
        private Timer time = new Timer();

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int TotalArqs = 0;
            pBar.Value = 0;
            lbx_arqs.Items.Clear();
            this.openFileDialog1.Filter = "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|" + "All files (*.*)|*.*";

            // Allow the user to select multiple images.
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "Lista de imagens da pasta";

            DialogResult dr = this.openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {

                int i = 0;

                var lstArquivos = new List<Arquivos>();

                foreach (String file in openFileDialog1.FileNames)
                {
                    TotalArqs = file.Count();
                    pBar.Maximum = TotalArqs;
                   
                    try
                    {
                        String arquivo = file;
                        lstArquivos.Add(new Arquivos() { Original = file, DataCriacao = File.GetCreationTime(file) });
                        lbx_arqs.Items.Add(file + " data_criacao: " + File.GetCreationTime(file));
                        i++;
                        pBar.Value = 1;
                       
                    }
                    catch (Exception ex)
                    {
                        // Could not load the image - probably related to Windows file system permissions.
                        MessageBox.Show("Cannot display the image: " + file.Substring(file.LastIndexOf('\\'))
                            + ". You may not have permission to read the file, or " +
                            "it may be corrupt.\n\nReported error: " + ex.Message);
                    }
                }

                var lista_arquivos = lstArquivos.OrderBy(f => f.DataCriacao).ToList();
                lbx_arqs.Items.Clear();

                int n=0;
                foreach (var lst in lista_arquivos)
                {
                    string _arquivo = lst.Original.Remove(lst.Original.Length - 4);
                    string _extensao = lst.Original.Substring(lst.Original.Length - 4);
                    string _novo_arquivo = _arquivo + "_" + n.ToString() + _extensao;
                    lbx_arqs.Items.Add(_novo_arquivo);

                    File.Move(lst.Original, _novo_arquivo);

                    n++;
                }
                pBar.Value = TotalArqs;
                MessageBox.Show("Operação efetuada com sucesso.");

            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            lbx_arqs.Items.Clear();
        }
    }
}
