using System;
using System.IO;

namespace SecretSanta.Library
{
    public class FileUtility
    {
        public static FileStream OpenFile(string fileName)
        {
            return new FileStream(fileName, FileMode.Open, FileAccess.Read);
        }
    }
}
