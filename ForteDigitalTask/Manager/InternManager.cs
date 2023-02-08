using ForteDigitalTask.Model;
using ForteDigitalTask.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForteDigitalTask.Manager
{
    public class InternManager : InternManagerInterface
    {
        private InternParserInterface parser;

        public InternManager(InternParserInterface parser)
        {
            this.parser = parser;
        }

        public int CountInterns(string url, int? ageThreshold, bool greaterThan)
        {
            List<Intern> interns = parser.ParseInternsFromFile(url);
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

            return count;
        }

        public int MaxAge(string url)
        {
            List<Intern> interns = parser.ParseInternsFromFile(url);
            int maxAge = interns.Max(i => i.age);
            return maxAge;
        }
    }
}
