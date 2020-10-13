using System;

namespace Dfe.CspdAlpha.Admin
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("ERROR: No arguments specified. Please specify at least task type to run");

                return;
            }

            var task = args[0];

            switch (task)
            {
                case nameof(SchoolsLoader):

                    if (args.Length < 4)
                    {
                        Console.WriteLine("ERROR: Please specify: [schoolsRefCsvFilePath] [schoolsPerfCsvFilePath] [giasCsvFilePath]");

                        return;
                    }
                    SchoolsLoader.Load(Console.WriteLine, args[1], args[2], args[3]);
                    break;

                case nameof(PupilsLoader):

                    if (args.Length < 4 || args.Length > 5)
                    {
                        Console.WriteLine("ERROR: Please specify: [pupilsCsvFilePath] [pupilsPerfCsvFilePath] [giasCsvFilePath]  ([amendmentsCsvFilePath] optional)");

                        return;
                    }

                    if (args.Length == 4)
                    {
                        PupilsLoader.Load(Console.WriteLine, args[1], args[2], args[3]);
                    }
                    else
                    {
                        PupilsLoader.Load(Console.WriteLine, args[1], args[2], args[3], args[4]);
                    }
                    break;

                default:

                    Console.WriteLine($"ERROR: Specified task '{task}' is not valid");

                    break;
            }
        }
    }
}
