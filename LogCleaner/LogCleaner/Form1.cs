namespace LogCleaner
{
    public partial class Form1 : Form
    {
        string srcpath;
        string destpath;
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
            else if (radioButton3.Checked)
            {
                Utilities ut = new Utilities(srcpath);
                if (DialogResult.Yes == MessageBox.Show("Vuoi davvero eliminare i file?", "Conferma", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    if (checkBox1.Checked)
                    {
                        richTextBox1.Text += "\n------Eliminazione file------";
                        ut.deleteFiles(richTextBox1);
                        richTextBox1.Text += "\n-----------------------------------------------------------------";
                    }
                    else
                    {
                        richTextBox1.Text += "\n------Eliminazione file------";
                        ut.deleteFilesByDate(dateTimePicker1.Value, dateTimePicker2.Value, richTextBox1);
                        richTextBox1.Text += "\n-----------------------------------------------------------------";
                    }
                }
            }
            else if (radioButton4.Checked)
            {
                Utilities ut = new Utilities(srcpath);
                if (checkBox1.Checked)
                {
                    MessageBox.Show("File trovati: " + ut.filesNum() + "\nCartelle trovate: " + ut.folderNum() + "\nGrandezza cartella: " + ut.folderSize() + " byte", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    richTextBox1.Text += "\n------Analisi di " + srcpath + "------" + "\nFile trovati: " + ut.filesNum() + "\nCartelle trovate: " + ut.folderNum() + "\nGrandezza cartella: " + ut.folderSize() + " byte" + "\n-----------------------------------------------------------------";
                }
                else
                {
                    MessageBox.Show("File trovati: " + ut.filesNumByDate(dateTimePicker1.Value, dateTimePicker2.Value) + "\nCartelle trovate: " + ut.folderNum() + "\nGrandezza cartella: " + ut.folderSizeByDate(dateTimePicker1.Value, dateTimePicker2.Value) + " byte", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    richTextBox1.Text += "\n------Analisi di " + srcpath + "------" + "\nFile trovati: " + ut.filesNumByDate(dateTimePicker1.Value, dateTimePicker2.Value) + "\nCartelle trovate: " + ut.folderNum() + "\nGrandezza cartella: " + ut.folderSizeByDate(dateTimePicker1.Value, dateTimePicker2.Value) + " byte" + "\n-----------------------------------------------------------------";
                }
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
                    if (checkBox1.Checked)
                    {
                        ut.moveFilesTo();
                        MessageBox.Show("File spostati con successo in " + destpath, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        richTextBox1.Text += "\n------Spostamento file------" + "\nSorgente: " + srcpath + "\nDestinazione: " + destpath + "\n-----------------------------------------------------------------";
                    }
                    else
                    {
                        ut.moveFilesToByDate(dateTimePicker1.Value, dateTimePicker2.Value);
                        MessageBox.Show("File spostati con successo in " + destpath, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        richTextBox1.Text += "\n------Spostamento file------" + "\nSorgente: " + srcpath + "\nDestinazione: " + destpath + "\n-----------------------------------------------------------------";
                    }
                }
                else if (radioButton2.Checked)
                {
                    if (checkBox1.Checked)
                    {
                        ut.compressAndMove();
                        MessageBox.Show("File compressi e inviati a " + destpath, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        richTextBox1.Text += "\n------Compressione File------" + "\nDa: " + srcpath + " A: " + destpath + "\n-----------------------------------------------------------------";
                    }
                    else
                    {
                        ut.compressAndMoveByDate(dateTimePicker1.Value, dateTimePicker2.Value);
                        MessageBox.Show("File compressi e inviati a " + destpath, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        richTextBox1.Text += "\n------Compressione File------" + "\nDa: " + srcpath + " A: " + destpath + "\n-----------------------------------------------------------------";
                    }
                }
                else
                {
                    MessageBox.Show("Nessuna opzione selezionata.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            
        }
        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(srcpath!= null && destpath != null)
            {
                string tmp = "";
                tmp = srcpath;
                srcpath = destpath;
                destpath = tmp;
                label1.Text = "Cartella sorgente selezionata: " + srcpath;
                label2.Text = "Cartella di destinazione selezionata: " + destpath;
            }
        }
    }
}