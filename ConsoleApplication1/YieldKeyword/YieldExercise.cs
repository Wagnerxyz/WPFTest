using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyExercise.YieldKeyword
{
    class IterationSample : IEnumerable
    {
        object[] values;
        int startingPoint;

        public IterationSample(object[] values, int startingPoint)
        {
            this.values = values;
            this.startingPoint = startingPoint;
        }

        public IEnumerator GetEnumerator()
        {
            yield return 6;
            yield return 7;
            yield return 8;
            //return new IterationSampleIterator(this);
        }

        class IterationSampleIterator : IEnumerator
        {
            IterationSample parent;
            int position;

            internal IterationSampleIterator(IterationSample parent)
            {
                this.parent = parent;
                position = -1;
            }

            public bool MoveNext()
            {
                if (position != parent.values.Length)
                {
                    position++;
                }
                return position < parent.values.Length;
            }

            public object Current
            {
                get
                {
                    if (position == -1 ||
                        position == parent.values.Length)
                    {
                        throw new InvalidOperationException();
                    }
                    int index = (position + parent.startingPoint);
                    index = index % parent.values.Length;
                    return parent.values[index];
                }
            }

            public void Reset()
            {
                position = -1;
            }
        }

        static void Run()
        {
            object[] values = { "a", "b", "c", "d", "e" };
            IterationSample collection = new IterationSample(values, 3);
            foreach (object x in collection)
            {
                Console.WriteLine(x);
            }
        }
    }

    class IteratorBlockIterationSample : IEnumerable
    {
        object[] values;
        int startingPoint;

        public IteratorBlockIterationSample(object[] values, int startingPoint)
        {
            this.values = values;
            this.startingPoint = startingPoint;
        }

        public IEnumerator GetEnumerator()
        {
            for (int index = 0; index < values.Length; index++)
            {
                yield return values[(index + startingPoint) % values.Length];
            }
        }
    }
    class YieldBreak
    {
        static IEnumerable<int> CountWithTimeLimit(DateTime limit)
        {
            for (int i = 1; i <= 100; i++)
            {
                if (DateTime.Now >= limit)
                {
                    yield break;
                }
                yield return i;
            }
        }

        static void Run()
        {
            DateTime stop = DateTime.Now.AddSeconds(2);
            foreach (int i in CountWithTimeLimit(stop))
            {
                Console.WriteLine("Received {0}", i);
                Thread.Sleep(300);
            }
        }
    }
    class LineReader
    {
        //TODO:.net 4 File.Readlines方法比这个更方便
        public static IEnumerable<string> ReadLines(string filename)
        {
            using (TextReader reader = File.OpenText(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        static void Run()
        {
            foreach (string line in ReadLines("../../LineReader.cs"))
            {
                Console.WriteLine(line);
            }
        }
    }
}
