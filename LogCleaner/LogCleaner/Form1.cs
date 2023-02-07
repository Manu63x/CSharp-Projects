namespace LogCleaner
{
    public partial class Form1 : Form
    {
        string srcpath = null;
        string destpath = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                srcpath = diag.SelectedPath;
                label1.Text = "Cartella sorgente selezionata: " + srcpath;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                destpath = diag.SelectedPath;
                label2.Text = "Cartella di destinazione selezionata: " + destpath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (srcpath == null)
            {
                MessageBox.Show("Nessuna cartella sorgente selezionata.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (destpath == null)
            {
                MessageBox.Show("Nessuna cartella di destinazione selezionata.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Utilities ut = new Utilities(srcpath, destpath);
                if (radioButton1.Checked)
                {
                    ut.moveFilesTo(destpath);
                    MessageBox.Show("File spostati con successo in " + destpath, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    richTextBox1.Text += "\n------Spostamento file------" + "\nSorgente: " + srcpath + "\nDestinazione: " + destpath + "\n-----------------------------------------------------------------";
                }
                else if (radioButton2.Checked)
                {
                    ut.compressAndMove(destpath);
                    MessageBox.Show("File compressi e inviati a " + destpath, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //richTextBox1.Text += ;
                }
                else if (radioButton3.Checked)
                {
                    if (DialogResult.Yes == MessageBox.Show("Vuoi davvero eliminare i file?", "Conferma", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    {
                        richTextBox1.Text += "\n------Eliminazione file------";
                        ut.deleteFiles(richTextBox1);
                        richTextBox1.Text += "\n-----------------------------------------------------------------";
                    }
                }
                else if (radioButton4.Checked)
                {
                    MessageBox.Show("File trovati: " + ut.filesNum() + "\nCartelle trovate: " + ut.folderNum() + "\nGrandezza cartella: " + ut.folderSizeBytes() + " byte", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    richTextBox1.Text += "\n------Analisi di " + srcpath + "------" + "\nFile trovati: " + ut.filesNum() + "\nCartelle trovate: " + ut.folderNum() + "\nGrandezza cartella: " + ut.folderSizeBytes() + " byte" + "\n-----------------------------------------------------------------";
                }
                else
                {
                    MessageBox.Show("Nessuna opzione selezionata.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}