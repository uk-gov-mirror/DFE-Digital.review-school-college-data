using System;

namespace Dfe.CspdAlpha.Admin
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("ERROR: Please specify 3 arguments: filepath to schools reference CSV and filepath to " +
                    "schools performance CSV and filepath to GIAS csv");

                return;
            }

            SchoolsLoader.Load(Console.WriteLine, args[0], args[1], args[2]);
        }

        
    }
}
