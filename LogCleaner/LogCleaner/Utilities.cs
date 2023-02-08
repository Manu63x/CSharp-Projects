using System.Diagnostics;
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
        public Utilities(string src)
        {
            this.src = src;
            this.folder = new DirectoryInfo(src);
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
        public bool filterByDate(DateTime dateTime1, DateTime dateTime2, string fileName)
        {
            try
            {
                string yearFromFileName = fileName.Substring(fileName.IndexOf(".log.") + 5, 4);
                string monthFromFileName = fileName.Substring(fileName.IndexOf(yearFromFileName) + 5, 2);
                string dayFromFileName = fileName.Substring(fileName.IndexOf(monthFromFileName) + 3, 2);
                Calendar gregorian = new GregorianCalendar();
                DateTime dt = new DateTime(int.Parse(yearFromFileName), int.Parse(monthFromFileName), int.Parse(dayFromFileName), 0, 0, 0, gregorian);
                //se passo direttamente dateTime1 e dateTime2 avranno l'ora di quando vengono premuti e se con il CompareTo dt e datetime1 coincidono come data restituirà comunque false, istanzio quindi dt1 che avrà h, min e sec uguali a dt
                DateTime dt1 = new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day, 0, 0, 0, gregorian);
                DateTime dt2 = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, 0, 0, 0, gregorian);
                Debug.WriteLine(dt.ToString() + "\n" + dt1.ToString() + "\n" + dt2.ToString());
                if ((dt.CompareTo(dt1) >= 0) && (dt.CompareTo(dt2) <= 0))
                {
                    return true;
                }
            }
            catch (ArgumentOutOfRangeException)
            {

            }
            return false;
        }
    }
}