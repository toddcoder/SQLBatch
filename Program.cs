using System.IO;
using SQLBatch.Library;
using Standard.Applications;
using Standard.Computer;
using static System.Console;

namespace SQLBatch
{
   internal class Program : CommandLine
   {
      FileStream fileStream;
      StreamWriter writer;

      static void Main(string[] args)
      {
         var program = new Program { Test = false };
         program.Run(args);
      }

      public override void Execute(Arguments arguments)
      {
         try
         {
            arguments.AssertMinimumCount(1);
            if (arguments.Exists(1) && arguments[1].Text == "/out")
            {
               FileName outfile = arguments[2].Text;
               if (outfile.Exists())
                  outfile.Delete();
               fileStream = outfile.WritingStream();
               writer = new StreamWriter(fileStream) { AutoFlush = true };
               SetOut(writer);
               SetError(writer);
            }
            var file = arguments[0].FileName.Required($"File {arguments[0].Text} not found");
            var chooser = new Chooser(file);
            chooser.Choose();
            WriteLine("Done");
         }
         finally
         {
            writer?.Close();
            fileStream?.Close();
         }
      }
   }
}