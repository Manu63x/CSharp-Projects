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
        public void moveFilesTo() //sposta i file da un path sorgente ad una destinazione
        {
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            foreach (FileInfo fi in f)
            {
                try
                {
                    fi.MoveTo(dest + "\\" + fi.Name);
                }
                catch (IOException)
                {
                    FileInfo[] f2 = this.dInfoDest.GetFiles("*arkivium.*", SearchOption.AllDirectories);
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
        public void moveFilesToByDate(DateTime datetime1, DateTime datetime2) //sposta i file da un path sorgente ad una destinazione filtrandoli se compresi fra 2 date
        {
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            foreach (FileInfo fi in f)
            {
                if (filterByDate(datetime1, datetime2, fi.Name) == true)
                {
                    try
                    {
                        fi.MoveTo(dest + "\\" + fi.Name);
                    }
                    catch (IOException)
                    {
                        FileInfo[] f2 = this.dInfoDest.GetFiles("*arkivium.*", SearchOption.AllDirectories);
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

        }
        public void compressAndMove() //sposta i file da un path sorgente ad una destinazione comprimendoli in un archivio zip
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
        public void compressAndMoveByDate(DateTime datetime1, DateTime datetime2)
        {
            DateTime date = DateTime.Now;
            string formattedString = String.Format("logCompression-{0}-{1}-{2}", date.Year, date.Month, date.Day);
            Directory.CreateDirectory(dest + "\\tmp");
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            foreach (FileInfo fi in f)
            {
                if (filterByDate(datetime1, datetime2, fi.Name) == true)
                {
                    try
                    {
                        fi.MoveTo(dest + "\\tmp\\" + fi.Name);
                    }
                    catch (IOException)
                    {
                        FileInfo[] f2 = this.dInfoDest.GetFiles("*arkivium.*", SearchOption.AllDirectories);
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
            try
            {
                ZipFile.CreateFromDirectory(dest + "\\tmp", dest + "\\" + formattedString + ".zip");
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
                        ZipFile.CreateFromDirectory(dest + "\\tmp", dest + "\\" + String.Format(formattedString + "-({0}).zip", count));
                    }
                    catch (IOException)
                    {

                    }
                }
                Directory.Delete(dest + "\\tmp", true);
            }

        }
        public void deleteFiles() // elimina i file specificati
        {
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            foreach (FileInfo fi in f)
            {
                fi.Delete();
            }
        }
        public void deleteFiles(RichTextBox r) //uguale a deleteFiles ma richiede come parametro un richtextbox dove stampare i path dei file eliminati
        {
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            foreach (FileInfo fi in f)
            {
                fi.Delete();
                r.Text += "\n" + fi;
            }
        }
        public void deleteFilesByDate(DateTime datetime1, DateTime datetime2, RichTextBox r) // elimina i file filtrati dalla funzione filterbydate stampando i path su un richtextbox 
        {
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            foreach (FileInfo fi in f)
            {
                if (filterByDate(datetime1, datetime2, fi.Name) == true)
                {
                    fi.Delete();
                    r.Text += "\n" + fi;
                }
            }
        }
        public long filesNum() // ritorna il numero di files presenti
        {
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            return f.Length;
        }
        public int filesNumByDate(DateTime datetime1, DateTime datetime2)
        {
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            int num = 0;
            foreach (FileInfo fi in f)
            {
                if (filterByDate(datetime1, datetime2, fi.Name) == true)
                {
                    num++;
                }
            }
            return num;
        }
        public long folderNum() // ritorna il numero di cartelle trovate
        {
            return this.folder.GetDirectories().Length;
        }
        public long folderSize()
        {
            long size = 0;
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            foreach (FileInfo fi in f)
            {
                size += fi.Length;
            }
            return size;
        }
        public long folderSizeByDate(DateTime datetime1, DateTime datetime2)
        {
            long size = 0;
            FileInfo[] f = this.folder.GetFiles("*arkivium.*", SearchOption.AllDirectories);
            foreach (FileInfo fi in f)
            {
                if (filterByDate(datetime1, datetime2, fi.Name) == true)
                {
                    size += fi.Length;
                }
            }
            return size;
        }
        public bool filterByDate(DateTime dateTime1, DateTime dateTime2, string fileName)
        {
            try
            {
                try
                {
                    string yearFromFileName = fileName.Substring(fileName.IndexOf(".log.") + 5, 4);
                    string monthFromFileName = fileName.Substring(fileName.IndexOf(".log.") + 10, 2);
                    string dayFromFileName = fileName.Substring(fileName.IndexOf(".log.") + 13, 2);
                    Calendar gregorian = new GregorianCalendar();
                    DateTime dt = new DateTime(int.Parse(yearFromFileName), int.Parse(monthFromFileName), int.Parse(dayFromFileName), 0, 0, 0, gregorian);
                    DateTime dt1 = new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day, 0, 0, 0, gregorian);
                    DateTime dt2 = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, 0, 0, 0, gregorian);
                    if ((dt.CompareTo(dt1) >= 0) && (dt.CompareTo(dt2) <= 0))
                    {
                        return true;
                    }
                }
                catch(FormatException)
                {
                    return false;
                }
            }
            catch (ArgumentOutOfRangeException)
            {

            }
            return false;
        }
    }
}