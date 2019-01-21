using System;
using System.IO;

namespace SecretSanta.Library
{
    public class FileUtility
    {
        public static bool OpenFile(string fileName, out FileStream fout)
        {
            fout = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            return !(fout is null);
        }
    }
}
