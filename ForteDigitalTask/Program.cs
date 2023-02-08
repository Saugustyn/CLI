using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonConverter = Newtonsoft.Json.JsonConverter;
using JsonException = Newtonsoft.Json.JsonException;

class Intern
{
    public int id { get; set; }
    public int age { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public DateTime internshipStart { get; set; }
    public string internshipStartString { get; set; }
    public DateTime internshipEnd { get; set; }
    public string internshipEndString { get; set; }
}

class Program
{
    static void Main(string[] argss)
    {
        string arg = Console.ReadLine();
        string[] args = arg.Split(' ');
        if (arg == null)
        {
            Console.WriteLine("Error: No input entered.");
            return;
        }

        if (args.Length < 2)
        {
            Console.WriteLine("Error: Invalid command.");
            return;
        }

        string command = args[0];
        string url = args[1];
        int? ageThreshold = null;
        bool greaterThan = false;

        if (command == "count")
        {
                if (args.Length > 3)
                {
                string ageOption = args[2];
                if (ageOption == "--age-gt")
                {
                    greaterThan = true;
                }
                else if (ageOption == "--age-lt")
                {
                    greaterThan = false;
                }
                else
                {
                    Console.WriteLine("Error: Invalid command.");
                    return;
                }
                ageThreshold = int.Parse(args[3]);
            }
        }

        switch (command)
        {
            case "count":
                CountInterns(url, ageThreshold, greaterThan);
                break;
            case "max-age":
                MaxAge(url);
                break;
            default:
                Console.WriteLine("Error: Invalid command.");
                break;
        }
    }

    private static void CountInterns(string url, int? ageThreshold, bool greaterThan)
    {
        List<Intern> interns = ParseInternsFromFile(url);
        int count = 0;
        if (ageThreshold.HasValue)
        {
            if (greaterThan)
            {
                count = interns.Count(i => i.age > ageThreshold.Value);
            }
            else
            {
                count = interns.Count(i => i.age < ageThreshold.Value);
            }
        }
        else
        {
            count = interns.Count;
        }

        Console.WriteLine(count);
    }

    private static void MaxAge(string url)
    {
        List<Intern> interns = ParseInternsFromFile(url);
        int maxAge = interns.Max(i => i.age);
        Console.WriteLine(maxAge);
    }

    private static List<Intern> ParseInternsFromFile(string url)
    {
        string fileContent = GetFileContent(url);
        string contentType = GetContentType(url);
        switch (contentType)
        {
            case "application/json; charset=utf-8":
                return ParseInternsFromJson(fileContent);
            case "text/csv; charset=utf-8":
                return ParseInternsFromCsv(fileContent);
            case "application/zip":
                return ParseInternsFromZip(url);
            default:
                Console.WriteLine("Error: Unsupported file format.");
                return new List<Intern>();
        }
    }


    private static string GetFileContent(string url)
    {
        WebClient client = new WebClient();
        return client.DownloadString(url);
    }

    private static string GetContentType(string url)
    {
        WebClient client = new WebClient();
        byte[] data = client.DownloadData(url);
        return client.ResponseHeaders["Content-Type"];
    }



    private static List<Intern> ParseInternsFromJson(string jsonContent)
    {
        JObject data = JObject.Parse(jsonContent);
        JArray interns = (JArray)data["interns"];

        List<Intern> listOfInterns = new List<Intern>();

        foreach (var intern in interns)
        {
            Intern newIntern = new Intern
            {
                id = (int)intern["id"],
                age = (int)intern["age"],
                name = (string)intern["name"],
                email = (string)intern["email"],
                internshipStart = DateTime.ParseExact(intern["internshipStart"].ToString(), "yyyy-MM-ddTHH:mm+00Z", CultureInfo.InvariantCulture),
                internshipEnd = DateTime.ParseExact(intern["internshipEnd"].ToString(), "yyyy-MM-ddTHH:mm+00Z", CultureInfo.InvariantCulture)
            };
            listOfInterns.Add(newIntern);
        }


        return listOfInterns;
    }

    private static List<Intern> ParseInternsFromZip(string url)
    {
        List<Intern> interns = new List<Intern>();
        using (WebClient client = new WebClient())
        {
            byte[] archiveBytes = client.DownloadData(url);
            using (MemoryStream stream = new MemoryStream(archiveBytes, false))
            {
                using (var archive = SharpCompress.Archives.Zip.ZipArchive.Open(stream))
                {
                    var entry = archive.Entries.FirstOrDefault();
                    if (entry != null)
                    {
                        using (StreamReader reader = new StreamReader(entry.OpenEntryStream()))
                        {
                            string line = reader.ReadLine();
                            while ((line = reader.ReadLine()) != null)
                            {
                                string[] values = line.Split(',');
                                Intern intern = new Intern
                                {
                                    id = int.Parse(values[0]),
                                    age = int.Parse(values[1]),
                                    name = values[2],
                                    email = values[3],
                                    internshipStart = DateTime.ParseExact(values[4], "yyyy-MM-ddTHH:mm+00Z", CultureInfo.InvariantCulture),
                                    internshipEnd = DateTime.ParseExact(values[5], "yyyy-MM-ddTHH:mm+00Z", CultureInfo.InvariantCulture)
                                };
                                interns.Add(intern);
                            }
                        }
                    }
                }
            }
        }
        return interns;
    }


    //private static List<Intern> ParseInternsFromZip(string fileContent)
    //{
    //    List<Intern> interns = new List<Intern>();
    //    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent), false))
    //    {
    //        using (var archive = SharpCompress.Archives.Zip.ZipArchive.Open(stream))
    //        {
    //            if (archive.Entries.Count > 0)
    //            {
    //                foreach (var entry in archive.Entries)
    //                {
    //                    if (entry.Key.EndsWith(".csv"))
    //                    {
    //                        using (var reader = new StreamReader(entry.OpenEntryStream()))
    //                        {
    //                            string line = reader.ReadLine();
    //                            while ((line = reader.ReadLine()) != null)
    //                            {
    //                                string[] values = line.Split(',');
    //                                Intern intern = new Intern
    //                                {
    //                                    id = int.Parse(values[0]),
    //                                    age = int.Parse(values[1]),
    //                                    name = values[2],
    //                                    email = values[3],
    //                                    internshipStart = DateTime.ParseExact(values[4], "yyyy-MM-ddTHH:mm+00Z", CultureInfo.InvariantCulture),
    //                                    internshipEnd = DateTime.ParseExact(values[5], "yyyy-MM-ddTHH:mm+00Z", CultureInfo.InvariantCulture)
    //                                };
    //                                interns.Add(intern);
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                Console.WriteLine("No entries found in the zip archive.");
    //            }
    //        }
    //    }
    //    return interns;
    //}





    private static List<Intern> ParseInternsFromCsv(string fileContent)
    {
        List<Intern> interns = new List<Intern>();
        using (StringReader reader = new StringReader(fileContent))
        {
            string line = reader.ReadLine();
            while ((line = reader.ReadLine()) != null)
            {
                string[] values = line.Split(',');
                Intern intern = new Intern
                {
                    id = int.Parse(values[0]),
                    age = int.Parse(values[1]),
                    name = values[2],
                    email = values[3],
                    internshipStart = DateTime.ParseExact(values[4], "yyyy-MM-ddTHH:mm+00Z", CultureInfo.InvariantCulture),
                    internshipEnd = DateTime.ParseExact(values[5], "yyyy-MM-ddTHH:mm+00Z", CultureInfo.InvariantCulture)
                };
                interns.Add(intern);
            }
        }
        return interns;
    }
}