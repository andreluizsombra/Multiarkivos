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
        private void IncreaseProgressBar(object sender, EventArgs e)
        {
            // Increment the value of the ProgressBar a value of one each time.
            pBar.Increment(1);
            // Display the textual value of the ProgressBar in the StatusBar control's first panel.
            pBar.Text = pBar.Value.ToString() + "% Completed";
            // Determine if we have completed by comparing the value of the Value property to the Maximum value.
            if (pBar.Value == pBar.Maximum)
                // Stop the timer.
                time.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pBar.Value = 0;
            lbx_arqs.Items.Clear();
            this.openFileDialog1.Filter = "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|" + "All files (*.*)|*.*";

            // Allow the user to select multiple images.
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "Lista de imagens da pasta";

            DialogResult dr = this.openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                time.Interval = 250;
                // Connect the Tick event of the timer to its event handler.
                time.Tick += new EventHandler(IncreaseProgressBar);
                // Start the timer.
                time.Start();

                //int i = 0;

                var lstArquivos = new List<Arquivos>();

                foreach (String file in openFileDialog1.FileNames)
                {
                    pBar.Maximum = file.Count();
                    //System.Threading.Thread.Sleep(70);
                    // Create a PictureBox.
                    try
                    {
                        String arquivo = file;
                        lstArquivos.Add(new Arquivos() { Original = file, DataCriacao = File.GetCreationTime(file) });
                        lbx_arqs.Items.Add(file + " data_criacao: " + File.GetCreationTime(file));
                       // i++;
                       // this.pBar.Increment(i);//(int)((100 * i) / file.Count());
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
                    lbx_arqs.Items.Add(lst.Original+"_"+n.ToString());
                    n++;
                }

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
