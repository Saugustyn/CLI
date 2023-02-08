using ForteDigitalTask.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ForteDigitalTask.Parser
{
    public class InternParser : InternParserInterface
    {
        private WebClient _client;

        public InternParser(WebClient client)
        {
            _client = client;
        }
        public List<Intern> ParseInternsFromFile(string url)
        {
            try
            {
                string fileContent = _client.DownloadString(url);
                string contentType = _client.ResponseHeaders["Content-Type"];

                if (contentType == "application/zip")
                {
                    return ParseInternsFromZip(url);
                }
                if (contentType == "application/json; charset=utf-8")
                {
                    return ParseInternsFromJson(fileContent);
                }
                if (contentType == "text/csv; charset=utf-8")
                {
                    return ParseInternsFromCsv(fileContent);
                }
                return new List<Intern>();
            }
            catch (WebException)
            {
               Console.WriteLine("Error: Cannot get file.");
                return new List<Intern>();
            }
            catch (Exception)
            {
                Console.WriteLine("Error: Cannot process the file.");
                return new List<Intern>();
            }
        }

        public List<Intern> ParseInternsFromJson(string jsonContent)
        {
            try
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
            catch (Exception)
            {
                Console.WriteLine("Error: Cannot process the file.");
                return new List<Intern>();
            }
        }

        public List<Intern> ParseInternsFromZip(string url)
        {
            try 
            { 
                List<Intern> listOfInterns = new List<Intern>();
                byte[] archiveBytes = _client.DownloadData(url);

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
                                    Intern intern = new Intern(values);
                                    listOfInterns.Add(intern);
                                }
                            }
                        }
                    }
                }
                return listOfInterns;
            }
            catch (Exception)
            {
                Console.WriteLine("Error: Cannot process the file.");
                return new List<Intern>();
            }
}

        public List<Intern> ParseInternsFromCsv(string fileContent)
        {
            try 
            { 
                List<Intern> listOfInterns = new List<Intern>();

                using (StringReader reader = new StringReader(fileContent))
                {
                    string line = reader.ReadLine();
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');
                        Intern intern = new Intern(values);
                        listOfInterns.Add(intern);
                    }
                }
                return listOfInterns;
            }
            catch (Exception)
            {
                Console.WriteLine("Error: Cannot process the file.");
                return new List<Intern>();
            }
}
    }
}
