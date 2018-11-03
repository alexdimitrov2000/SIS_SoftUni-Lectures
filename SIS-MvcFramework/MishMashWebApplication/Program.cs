using SIS.MvcFramework;
using System;
using System.Linq;

namespace MishMashWebApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost.Start(new StartUp());
        }
    }
}
