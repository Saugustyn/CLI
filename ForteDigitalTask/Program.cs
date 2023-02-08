using ForteDigitalTask.Manager;
using ForteDigitalTask.Parser;
using System.Net;

while (true)
{
    InternManager manager = new InternManager(new InternParser(new WebClient()));
    InternConsoleManager consoleManager = new InternConsoleManager(manager);
    consoleManager.Start();
}
