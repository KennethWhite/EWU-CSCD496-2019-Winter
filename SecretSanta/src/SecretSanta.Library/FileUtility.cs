using System;
using System.IO;

namespace SecretSanta.Library
{
    public class FileUtility
    {
        public static FileStream OpenFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException($"Parameter fileName empty or null on call to OpenFile.", fileName);
            }
            return new FileStream(fileName, FileMode.Open, FileAccess.Read);
        }
    }
}
