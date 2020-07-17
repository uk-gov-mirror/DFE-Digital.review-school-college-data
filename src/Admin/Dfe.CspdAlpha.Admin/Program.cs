using System;

namespace Dfe.CspdAlpha.Admin
{
    class Program
    {
        

        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("ERROR: No arguments specified. Please specify at least Loader type to run");

                return;
            }

            var loader = args[0];

            switch (loader)
            {
                case nameof(SchoolsLoader):

                    if (args.Length != 4)
                    {
                        Console.WriteLine("ERROR: Please specify: [schoolsRefCsvFilePath] [schoolsPerfCsvFilePath] [giasCsvFilePath] ");

                        return;
                    }

                    SchoolsLoader.Load(Console.WriteLine, args[1], args[2], args[3]);
                    break;

                case nameof(PupilsLoader):

                    if (args.Length != 4)
                    {
                        Console.WriteLine("ERROR: Please specify: [pupilsCsvFilePath] [pupilsPerfCsvFilePath] [giasCsvFilePath] ");

                        return;
                    }

                    PupilsLoader.Load(Console.WriteLine, args[1], args[2], args[3]);
                    break;

                default:

                    Console.WriteLine($"ERROR: Specified Loader '{loader}' is not valid");

                    break;
            }
        }
    }
}
