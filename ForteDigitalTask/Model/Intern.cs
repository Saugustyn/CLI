using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForteDigitalTask.Model
{
    public class Intern
    {
        public int id { get; set; }
        public int age { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTime internshipStart { get; set; }
        public string internshipStartString { get; set; }
        public DateTime internshipEnd { get; set; }
        public string internshipEndString { get; set; }

        public Intern()
        {

        }
        public Intern (string[] values)
        {
            id = int.Parse(values[0]);
            age = int.Parse(values[1]);
            name = values[2];
            email = values[3];
            internshipStart = DateTime.ParseExact(values[4], "yyyy-MM-ddTHH:mm+00Z", CultureInfo.InvariantCulture);
            internshipEnd = DateTime.ParseExact(values[5], "yyyy-MM-ddTHH:mm+00Z", CultureInfo.InvariantCulture);
        }
    }
}
