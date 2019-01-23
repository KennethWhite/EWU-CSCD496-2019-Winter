using System;
using System.IO;

namespace SecretSanta.Library
{
    public class FileUtility
    {
        //private static string RelativePath { get; set; } =
        //    Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
        //        @"..\..\..\..\..\test\TestData\");

        public static FileStream OpenFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException($"Parameter fileName empty or null on call to OpenFile.", fileName);
            }
            var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            return new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        }
    }
}
