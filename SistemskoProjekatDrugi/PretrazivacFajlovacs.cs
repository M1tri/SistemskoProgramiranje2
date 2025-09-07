using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemskoProjekatDrugi
{
    public class PretrazivacFajlova
    {
        static readonly StringBuilder ret = new("");
        static int brojFajlova = 0;

        class TaskFuncRet
        {
            public String fajlovi;
            public int brojFajlova;

            public TaskFuncRet(string fajlovi, int brojFajlova)
            {
                this.fajlovi = fajlovi;
                this.brojFajlova = brojFajlova;
            }
        }
        public static async Task<String> PretraziSaTaskovima(String kljuc)
        {
            brojFajlova = 0;
            ret.Clear();
            String content = "";

            try
            {
                String path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));

                String[] fajlovi = Directory.GetFiles(path);

                foreach (String f in fajlovi)
                {
                    String imeFajla = Path.GetFileName(f);
                    if (imeFajla.ToLower().Contains(kljuc))
                    {
                        brojFajlova++;
                        ret.Append($"<p><a href=\"/download?putanja={Uri.EscapeDataString(f)}\"> {imeFajla} </a></p>");
                    }
                }

                String[] direktorijumi = Directory.GetDirectories(path);

                var taskovi = new List<Task<TaskFuncRet>>();

                foreach (String dir in direktorijumi)
                {
                    taskovi.Add(PretraziDirSaTaskovima(dir, kljuc));
                }

                await Task.WhenAll(taskovi);

                foreach (var t in taskovi)
                {
                    ret.Append(t.Result.fajlovi);
                    brojFajlova += t.Result.brojFajlova;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Javio se izuzetak: " + e.Message);
            }

            Console.WriteLine("Sa taskovima: Zavrsena pretraga!");

            if (String.IsNullOrEmpty(ret.ToString()))
            {
                content += "<html> <head>\n";
                content += "<title>Projekat 2 - Rezultat</title>\n";
                content += "</head>\n";
                content += "<body>\n";
                content += "<h1>Sistemsko programiranje - Projekat 2</h1>\n";
                content += $"<h3>Rec koju ste pretrazili je: <span style='color:blue;'>{kljuc}</span></h3>\n";
                content += "<h3>Nije pronadjen nijedan fajl na osnovu kljuca.</h3>\n";
                content += "</body> </html>\n";

                return content;
            }

            content += "<html> <head>\n";
            content += "<title>Projekat 2 - Rezultat</title>\n";
            content += "</head>\n";
            content += "<body>\n";
            content += "<h1>Sistemsko programiranje - Projekat 2</h1>\n";
            content += $"<h3>Rec koju ste pretrazili je: <span style='color:blue;'>{kljuc}</span></h3>\n";
            content += $"<h3>Rezultati pretrage: </h3>\n";
            content += $"<h4>Broj pronadjenih fajlova: {brojFajlova} </h4>\n";
            content += "<h4>Kliknite na fajl da biste ga preuzeli.</h4>\n";
            content += $"<h4>{ret}</h4>\n";
            content += "</body> </html>\n";

            return content;
        }

        static async Task<TaskFuncRet> PretraziDirSaTaskovima(String path, String kljuc)
        {
            try
            {
                StringBuilder files = new("");
                int brFiles = 0;

                String[] fajlovi = Directory.GetFiles(path);

                foreach (String f in fajlovi)
                {
                    String imeFajla = Path.GetFileName(f);
                    if (imeFajla.ToLower().Contains(kljuc))
                    {
                        brFiles++;
                        files.Append($"<p><a href=\"/download?putanja={Uri.EscapeDataString(f)}\"> {imeFajla} </a></p>");
                    }
                }

                String[] direktorijumi = Directory.GetDirectories(path);

                var taskovi = new List<Task<TaskFuncRet>>();

                foreach(String dir in direktorijumi)
                {
                    taskovi.Add(PretraziDirSaTaskovima(dir, kljuc));
                }

                await Task.WhenAll(taskovi);

                foreach (var t in taskovi)
                {
                    files.Append(t.Result.fajlovi);
                    brFiles += t.Result.brojFajlova;
                }

                return new TaskFuncRet(files.ToString(), brFiles);
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Javio se izuzetak: "+ e.Message);
                return new TaskFuncRet("", 0);
            }
        }

        public static String PretraziBezTaskova(String kljuc)
        {
            ret.Clear();
            brojFajlova = 0;
            String content = "";

            try
            {
                String path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));

                String[] fajlovi = Directory.GetFiles(path);

                foreach (String f in fajlovi)
                {
                    String imeFajla = Path.GetFileName(f);
                    if (imeFajla.ToLower().Contains(kljuc))
                    {
                        brojFajlova++;
                        ret.Append($"<p><a href=\"/download?putanja={Uri.EscapeDataString(f)}\"> {imeFajla} </a></p>");
                    }
                }

                String[] direktorijumi = Directory.GetDirectories(path);

                foreach (String dir in direktorijumi)
                {
                    ret.Append(PretraziDirBezTaskova(dir, kljuc));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Bez taskova: Zavrsena pretraga!");

            if (String.IsNullOrEmpty(ret.ToString()))
            {
                content += "<html> <head>\n";
                content += "<title>Projekat 2 - Rezultat</title>\n";
                content += "</head>\n";
                content += "<body>\n";
                content += "<h1>Sistemsko programiranje - Projekat 2</h1>\n";
                content += $"<h3>Rec koju ste pretrazili je: <span style='color:blue;'>{kljuc}</span></h3>\n";
                content += "<h3>Nije pronadjen nijedan fajl na osnovu kljuca.</h3>\n";
                content += "</body> </html>\n";

                return content;
            }

            content += "<html> <head>\n";
            content += "<title>Projekat 2 - Rezultat</title>\n";
            content += "</head>\n";
            content += "<body>\n";
            content += "<h1>Sistemsko programiranje - Projekat 2</h1>\n";
            content += $"<h3>Rec koju ste pretrazili je: <span style='color:blue;'>{kljuc}</span></h3>\n";
            content += $"<h3>Rezultati pretrage: </h3>\n";
            content += $"<h4>Broj pronadjenih fajlova: {brojFajlova} </h4>\n";
            content += "<h4>Kliknite na fajl da biste ga preuzeli.</h4>\n";
            content += $"<h4>{ret}</h4>\n";
            content += "</body> </html>\n";

            return content;
        }

        static String PretraziDirBezTaskova(String path, String kljuc)
        {
            String files = "";

            try
            {
                String[] fajlovi = Directory.GetFiles(path);

                foreach (String f in fajlovi)
                {
                    String imeFajla = Path.GetFileName(f);
                    if (imeFajla.ToLower().Contains(kljuc))
                    {
                        brojFajlova++;
                        files += $"<p><a href=\"/download?putanja={Uri.EscapeDataString(f)}\"> {imeFajla} </a></p>";
                    }
                }

                String[] direktorijumi = Directory.GetDirectories(path);

                foreach (String dir in direktorijumi)
                {
                    files += PretraziDirBezTaskova(dir, kljuc);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Javio se izuzetak tokom pretrage u {path}: {e.Message}");
            }

            return files;
        }
    }
}
