using ForteDigitalTask.Manager;
using ForteDigitalTask.Parser;
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



class Program
{
    static void Main(string[] args)
    {
        InternManager manager = new InternManager(new InternParser(new WebClient()));
        InternConsoleManager consoleManager = new InternConsoleManager(manager);
        consoleManager.Start();
    }
}