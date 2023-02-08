using System.Globalization;
using System.IO.Compression;

namespace LogCleaner
{
    internal class Utilities
    {
        private string src;
        private string dest;
        private DirectoryInfo folder;
        private DirectoryInfo dInfoDest;
        public Utilities(string src, string dest)
        {
            this.src = src;
            this.dest = dest;
            folder = new DirectoryInfo(src);
            dInfoDest = new DirectoryInfo(dest);
        }
        public void moveFilesTo(string dest) //sposta i file da un path sorgente ad una destinazione
        {
            FileInfo[] f = this.folder.GetFiles("arkivium.*", SearchOption.AllDirectories);
            foreach (FileInfo fi in f)
            {
                try
                {
                    fi.MoveTo(dest + "\\" + fi.Name);
                }
                catch (IOException)
                {
                    FileInfo[] f2 = this.dInfoDest.GetFiles("arkivium.*", SearchOption.AllDirectories);
                    int count = 1;
                    foreach (FileInfo fi2 in f2) //rinomino il file se esiste già
                    {
                        while ((String.Format(fi.Name + "-({0})", count)).Equals(fi2.Name))
                        {
                            count++;
                        }
                        try
                        {
                            fi.MoveTo(dest + "\\" + String.Format("{0}-({1})", fi.Name, count++));
                        }
                        catch (IOException)
                        {

                        }
                    }
                }
            }

        }
        public void compressAndMove(string dest) // sposta i file da un path sorgente ad una destinazione comprimendoli in un archivio zip
        {
            DateTime date = DateTime.Now;
            string formattedString = String.Format("logCompression-{0}-{1}-{2}", date.Year, date.Month, date.Day);
            try
            {
                ZipFile.CreateFromDirectory(src, dest + "\\" + formattedString + ".zip");
            }
            catch (IOException)
            {
                FileInfo[] f2 = this.dInfoDest.GetFiles("logCompression-*", SearchOption.AllDirectories);
                int count = 1;
                foreach (FileInfo fi2 in f2) //rinomino il file se esiste già
                {

                    while ((String.Format(formattedString + "-({0}).zip", count)).Equals(fi2.Name))
                    {
                        count++;
                    }
                    try
                    {
                        ZipFile.CreateFromDirectory(src, dest + "\\" + String.Format(formattedString + "-({0}).zip", count));
                    }
                    catch (IOException)
                    {

                    }
                }
            }
        }
        public void deleteFiles() // elimina i file specificati
        {
            FileInfo[] f = this.folder.GetFiles("arkivium.*", SearchOption.AllDirectories);
            foreach (FileInfo fi in f)
            {
                fi.Delete();
            }
        }
        public void deleteFiles(RichTextBox r) // elimina i file specificati
        {
            FileInfo[] f = this.folder.GetFiles("arkivium.*", SearchOption.AllDirectories);
            foreach (FileInfo fi in f)
            {
                fi.Delete();
                r.Text += "\n" + fi;
            }
        }
        public long filesNum() // ritorna il numero di files presenti
        {
            FileInfo[] f = this.folder.GetFiles("arkivium.*", SearchOption.AllDirectories);
            return f.Length;
        }
        public long folderNum() // ritorna il numero di cartelle trovate
        {
            return this.folder.GetDirectories().Length;
        }
        public long folderSizeBytes()
        {
            return folderSize(this.folder);
        }
        public long folderSizeKb()
        {
            return folderSize(this.folder) / 1024;
        }
        public long folderSizeMb()
        {
            return (folderSize(this.folder) / 1024) / 1024;
        }
        private static long folderSize(DirectoryInfo folder)
        {
            long totalSizeOfDir = 0;

            FileInfo[] allFiles = folder.GetFiles("arkivium.*", SearchOption.AllDirectories);

            foreach (FileInfo file in allFiles)
            {
                totalSizeOfDir += file.Length;
            }

            DirectoryInfo[] subFolders = folder.GetDirectories();

            foreach (DirectoryInfo dir in subFolders)
            {
                totalSizeOfDir += folderSize(dir);
            }

            return totalSizeOfDir;
        }
        private bool filterByDate(DateTimePicker dtp1, DateTimePicker dtp2, string fileName)
        {
            try
            {
                string yearFromFileName = fileName.Substring(fileName.IndexOf(".log.") + 5, 4);
                string monthFromFileName = fileName.Substring(fileName.IndexOf(yearFromFileName) + 5, 2);
                string dayFromFileName = fileName.Substring(fileName.IndexOf(monthFromFileName) + 3, 2);
                Calendar gregorian = new GregorianCalendar();
                DateTime dt = new DateTime(Int16.Parse(yearFromFileName), Int16.Parse(monthFromFileName), Int16.Parse(dayFromFileName), gregorian);
                if ((dt.CompareTo(dtp1) == 1) && (dt.CompareTo(dtp2) == 1)) //da fixare con i valori di ritorno di CompareTo -inf<0<+inf
                {
                    return true;
                }
                return false;
            }
            catch (ArgumentOutOfRangeException)
            {

            }
            return false;
        }
    }
}
