using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class WeakRefernceTest
    {
        public void Test()
        {
            // Create the cache.
            int cacheSize = 500;
            Random r = new Random();
            Cache c = new Cache(cacheSize);

            string DataName = "";

            // Randomly access objects in the cache.
            for (int i = 0; i < c.Count; i++)
            {
                int index = r.Next(c.Count);

                // Access the object by
                // getting a property value.
                DataName = c[index].Name;
            }
            // Show results.
            double regenPercent = c.RegenerationCount * 100 / c.Count;
            Console.WriteLine("Cache size: {0}, Regenerated: {1}%", c.Count.ToString(), regenPercent.ToString());
            Console.ReadKey();
        }
    }
    public class Cache
    {
        // Dictionary to contain the cache.
        static Dictionary<int, System.WeakReference> _cache;

        // Track the number of times an 
        // object is regenerated.
        int regenCount = 0;

        public Cache(int count)
        {

            _cache = new Dictionary<int, System.WeakReference>();

            // Add data objects with a 
            // short weak reference to the cache.
            for (int i = 0; i < count; i++)
            {
                _cache.Add(i, new System.WeakReference(new Data(i), false));
            }

        }

        // Returns the number of items in the cache.
        public int Count
        {
            get
            {
                return _cache.Count;
            }
        }

        // Returns the number of times an 
        // object had to be regenerated.
        public int RegenerationCount
        {
            get
            {
                return regenCount;
            }
        }

        // Accesses a data object from the cache.
        // If the object was reclaimed for garbage collection,
        // create a new data object at that index location.
        public Data this[int index]
        {
            get
            {
                // Obtain an instance of a data
                // object from the cache of
                // of weak reference objects.
                Data d = _cache[index].Target as Data;
                if (d == null)
                {
                    // Object was reclaimed, so generate a new one.
                    Console.WriteLine("Regenerate object at {0}: Yes", index.ToString());
                    d = new Data(index);
                    regenCount++;
                }
                else
                {
                    // Object was obtained with the weak reference.
                    Console.WriteLine("Regenerate object at {0}: No", index.ToString());
                }

                return d;
            }

        }

    }


    // This class creates byte arrays to simulate data.
    public class Data
    {
        private byte[] _data;
        private string _name;

        public Data(int size)
        {
            _data = new byte[size * 1024];
            _name = size.ToString();
        }

        // Simple property.
        public string Name
        {
            get
            {
                return _name;
            }
        }

    }
}
