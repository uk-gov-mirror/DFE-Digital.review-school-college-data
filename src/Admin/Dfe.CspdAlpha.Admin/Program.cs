using System;

namespace Dfe.CspdAlpha.Admin
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("ERROR: Please specify 2 arguments: filepath to schools reference CSV and filepath to " +
                    "schools performance CSV");

                return;
            }

            SchoolsLoader.Load(Console.WriteLine, args[0], args[1]);
        }

        
    }
}
