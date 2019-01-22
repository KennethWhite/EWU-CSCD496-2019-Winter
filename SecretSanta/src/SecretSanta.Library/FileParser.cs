using System.IO;
using System.Linq;
using SecretSanta.Domain.Models;

namespace SecretSanta.Library
{
    public class FileParser
    {
        public static User ParseWishlistFile(string wishlistFileName)
        {
            User user;
            using (var fileStream = FileUtility.OpenFile(wishlistFileName))
            using (var streamReader = new StreamReader(fileStream))
            {
                user = ParseWishlistHeader(streamReader);
            }

            return user;
        }

        private static User ParseWishlistHeader(StreamReader fin)
        {
            string line;
            while (string.IsNullOrWhiteSpace(line = fin.ReadLine()))
            {
                if(fin.EndOfStream) throw new InvalidDataException("File does not contain a header");
            }
            
            if(!line.Contains("Name"))
                throw new InvalidDataException($"Header: \"{line}\" is an improper format");

            var swapNames = line.Contains(',');
            var names = line.Split();
            if(names.Length != 3)
                throw new InvalidDataException($"\"Header: {line}\" does not contain a proper user name");
            
            return swapNames ? new User {FirstName = names[2], LastName = names[1].Remove(names[1].Length-1)} : 
                new User {FirstName = names[1], LastName = names[2]};
        }
    }
}