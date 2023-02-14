using System.IO.Compression;
namespace LogCleaner
{
    internal class Utilities
    {
        public bool cancel = false;
        private string src;
        private string dest;
        private DirectoryInfo folder;
        private DirectoryInfo dInfoDest;
        private string[] units = { "byte", "KB", "MB", "GB", "TB", "PB" };
        public Utilities()
        {
            src = " ";
            dest = " ";
        }
        public string getSrc()
        {
            return src;
        }
        public void setSrc(string src)
        {
            this.src = src;
            folder = new DirectoryInfo(src);
        }
        public string getDest()
        {
            return dest;
        }
        public void setDest(string dest)
        {
            this.dest = dest;
            dInfoDest = new DirectoryInfo(dest);
        }
        public bool MoveFilesTo() //sposta i file da un path sorgente ad una destinazione
        {
            cancel = false;
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            bool duplicates = false;
            foreach (FileInfo fi in f)
            {
                Application.DoEvents();
                if (cancel == true)
                    break;
                try
                {
                    fi.MoveTo(dest + "\\" + fi.Name);
                }
                catch (IOException)
                {
                    duplicates = true;
                }
            }
            return duplicates;
        }
        public bool MoveFilesToByDate(DateTime datetime1, DateTime datetime2) //sposta i file da un path sorgente ad una destinazione filtrandoli se compresi fra 2 date
        {
            cancel = false;
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            bool duplicates = false;
            foreach (FileInfo fi in f)
            {
                Application.DoEvents();
                if (cancel == true)
                    break;
                if (FilterByDate(datetime1, datetime2, fi.Name) == true)
                {
                    try
                    {
                        fi.MoveTo(dest + "\\" + fi.Name);
                    }
                    catch (IOException)
                    {
                        duplicates = true;
                    }
                }
            }
            return duplicates;
        }
        public void CompressAndMove() //sposta i file da un path sorgente ad una destinazione comprimendoli in un archivio zip
        {
            cancel = false;
            DateTime date = DateTime.Now;
            string formattedString = String.Format("logCompression-{0}-{1}-{2}", date.Year, date.Month, date.Day);
            Application.DoEvents();
            try
            {
                ZipFile.CreateFromDirectory(src, dest + "\\" + formattedString + ".zip");
            }
            catch (IOException)
            {
                int count = 1;
                bool exit = false;
                while (exit != true)
                {
                    Application.DoEvents();
                    if (cancel == true)
                        break;
                    try
                    {
                        ZipFile.CreateFromDirectory(src, dest + "\\" + String.Format(formattedString + "-({0}).zip", count++));
                        exit = true;
                    }
                    catch (IOException)
                    {

                    }
                }
            }
        }
        public void CompressAndMoveByDate(DateTime datetime1, DateTime datetime2) //crea una cartella nella destinazione, copia i file filtrandoli, e zippa la cartella creata eliminandola a fine compressione
        {
            cancel = false;
            DateTime date = DateTime.Now;
            string formattedString = String.Format("logCompression-{0}-{1}-{2}", date.Year, date.Month, date.Day);
            CopyFiles(src, dest, datetime1, datetime2);
            Application.DoEvents();
            try
            {
                ZipFile.CreateFromDirectory(dest + "\\tmp", dest + "\\" + formattedString + ".zip");
            }
            catch (IOException)
            {
                int count = 1;
                bool exit = false;
                while (exit != true)
                {
                    Application.DoEvents();
                    if (cancel == true)
                        break;
                    try
                    {
                        ZipFile.CreateFromDirectory(dest + "\\tmp", dest + "\\" + String.Format(formattedString + "-({0}).zip", count++));
                        exit = true;
                    }
                    catch (IOException)
                    {

                    }
                }
            }
            Directory.Delete(dest + "\\tmp", true);
        }
        private static void CopyFiles(string sourcePath, string destPath, DateTime datetime1, DateTime datetime2) //copia i file filtrandoli
        {
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Application.DoEvents();

                Directory.CreateDirectory(dirPath.Replace(sourcePath, destPath + "\\tmp"));
            }
            foreach (string newPath in Directory.GetFiles(sourcePath, "*arkivium.*", SearchOption.AllDirectories))
            {
                string fileName = Path.GetFileName(newPath);
                if (FilterByDate(datetime1, datetime2, fileName) == true)
                {
                    File.Copy(newPath, newPath.Replace(sourcePath, destPath + "\\tmp"), true);
                }
            }
        }
        public bool DeleteFiles() // elimina i file specificati
        {
            cancel = false;
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            foreach (FileInfo fi in f)
            {
                Application.DoEvents();
                if (cancel == true)
                    return false;
                fi.Delete();
            }
            return true;
        }
        public bool DeleteFiles(RichTextBox r) //uguale a DeleteFiles ma richiede come parametro un richtextbox dove stampare i path dei file eliminati
        {
            cancel = false;
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            foreach (FileInfo fi in f)
            {
                Application.DoEvents();
                if (cancel == true)
                    return false;
                fi.Delete();
                r.Text += "\n" + fi;
            }
            return true;
        }
        public bool DeleteFilesByDate(DateTime datetime1, DateTime datetime2, RichTextBox r) // elimina i file filtrati dalla funzione filterbydate stampando i path su un richtextbox 
        {
            cancel = false;
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            foreach (FileInfo fi in f)
            {
                if (FilterByDate(datetime1, datetime2, fi.Name) == true)
                {
                    Application.DoEvents();
                    if (cancel == true)
                        return false;
                    fi.Delete();
                    r.Text += "\n" + fi;
                }
            }
            return true;
        }
        public bool DeleteFilesByDate(DateTime datetime1, DateTime datetime2) // elimina i file filtrati dalla funzione filterbydate
        {
            cancel = false;
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            foreach (FileInfo fi in f)
            {
                if (FilterByDate(datetime1, datetime2, fi.Name) == true)
                {
                    Application.DoEvents();
                    if (cancel == true)
                        return false;
                    fi.Delete();
                }
            }
            return true;
        }
        public long FilesNum() // ritorna il numero di file presenti
        {
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            return f.Length;
        }
        public int FilesNumByDate(DateTime datetime1, DateTime datetime2) // ritorna il numero di file presenti filtrandoli
        {
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            int num = 0;
            foreach (FileInfo fi in f)
            {
                if (FilterByDate(datetime1, datetime2, fi.Name) == true)
                {
                    num++;
                }
            }
            return num;
        }
        public long FolderNum() // ritorna il numero di cartelle trovate
        {
            return this.folder.GetDirectories().Length;
        }
        public string FolderSize()  // ritorna la grandezza della cartella
        {
            decimal size = 0;
            int unit = 0;
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            foreach (FileInfo fi in f)
            {
                size += fi.Length;
            }
            while (size > 1024)
            {
                size /= 1024;
                unit++;
            }
            return Math.Round(size, 1) + " " + units[unit];
        }
        public string FolderSizeByDate(DateTime datetime1, DateTime datetime2) // ritorna la grandezza della cartella filtrando i file
        {
            decimal size = 0;
            int unit = 0;
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            foreach (FileInfo fi in f)
            {
                if (FilterByDate(datetime1, datetime2, fi.Name) == true)
                {
                    size += fi.Length;
                }
            }
            while (size > 1024)
            {
                size /= 1024;
                unit++;
            }
            return Math.Round(size, 1) + " " + units[unit];
        }
        public static bool FilterByDate(DateTime fromDate, DateTime toDate, string fileName) // filtra i file in base alla data nel loro nome, se compresi o uguali agli
        {                                                                                    // estremi inseriti ritorna true, altrimenti false (anche nel caso in cui non sia presente la data nel nome del file).
            try
            {
                int day = int.Parse(fileName.Substring(fileName.Length - 2, 2));
                int month = int.Parse(fileName.Substring(fileName.Length - 5, 2));
                int year = int.Parse(fileName.Substring(fileName.Length - 10, 4));
                DateTime dt = new DateTime(year, month, day);
                if ((dt.CompareTo(fromDate.Date) >= 0) && (dt.CompareTo(toDate.Date) <= 0))
                {
                    return true;
                }
            }
            catch (FormatException)
            {
                return false;
            }
            return false;
        }
    }
}