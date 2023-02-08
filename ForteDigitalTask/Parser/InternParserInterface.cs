using ForteDigitalTask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForteDigitalTask.Parser
{
    public interface InternParserInterface
    {
        public List<Intern> ParseInternsFromFile(string url);
        public List<Intern> ParseInternsFromJson(string jsonContent);
        public List<Intern> ParseInternsFromZip(string url);
        public List<Intern> ParseInternsFromCsv(string fileContent);

    }
}
