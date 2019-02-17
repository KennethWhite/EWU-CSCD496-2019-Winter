using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Domain
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> collection, IRandom random)
        {
            if (random == null) throw new ArgumentNullException(nameof(random));
            
            List<T> items = collection.ToList();
            while (items.Any())
            {
                int randomIndex = random.Next(items.Count);
                T item = items[randomIndex];
                items.RemoveAt(randomIndex);
                yield return item;
            }
        }
    }
}