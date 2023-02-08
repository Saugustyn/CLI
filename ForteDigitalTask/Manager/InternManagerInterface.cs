using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForteDigitalTask.Manager
{
    public interface InternManagerInterface
    {
        public int CountInterns(string url, int? ageThreshold, bool greaterThan);
        public int MaxAge(string url);
    }
}
