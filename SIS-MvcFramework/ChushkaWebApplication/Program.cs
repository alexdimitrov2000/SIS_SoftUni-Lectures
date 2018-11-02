using SIS.MvcFramework;
using System;

namespace ChushkaWebApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost.Start(new StartUp());
        }
    }
}
