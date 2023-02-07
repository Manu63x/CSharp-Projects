namespace Music_player
{
    using NAudio.Wave;
    using NAudio.Wave.SampleProviders;
    public partial class Form1 : Form
    {
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        int afpos;
        Boolean isplaying;
        public Form1()
        {
            InitializeComponent();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folder = diag.SelectedPath;
                String[] filenames = Directory.GetFiles(folder);
                label1.Text = filenames[0];
                //play audio
                outputDevice = new WaveOutEvent();
                audioFile = new AudioFileReader(filenames[0]);
                outputDevice.Init(audioFile);
                outputDevice.Play();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(isplaying == true)
            {
                button1.Text = "|>";
                isplaying = false;
                //stop audio
                outputDevice.Stop();
            }
            else if(isplaying == false)
            {
                button1.Text = "||";
                isplaying = true;
                //play audio
                outputDevice.Play();
            }
        }
    }
}