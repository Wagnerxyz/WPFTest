using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class Artist
    {
        public Artist(int id, string name)
        {
            ArtistId = id;
            Name = name;
        }
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public static ArtistCollection GetSample()
        {
            //ArtistCollection collection = new ArtistCollection() { new Artist { ArtistId = 1, Name = "aa" } };

            ArtistCollection collection = new ArtistCollection();
            collection.Add(new Artist(1, "aaa"));
            collection.Add(new Artist(2, "bbb"));
            collection.Add(new Artist(3, "cccc"));

            return collection;
        }
    }
    public class ArtistCollection : IEnumerable<Artist>
    {
        List<Artist> list = new List<Artist>();
        public void Add(Artist ar)
        {
            list.Add(ar);
        }
        public void Remove(Artist ar)
        {
            list.Remove(ar);
        }
        public Artist this[int index]
        {
            get { return list[index]; }
            set { list[index] = value; }
        }
        public int Count() 
        {
            return list.Count;
        }
        //public Artist this[string index]
        //{
        //    get { return list[index]; }
        //    set { list[index] = value; }
        //}

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ArtistEnumerator(this);
        }

        IEnumerator<Artist> IEnumerable<Artist>.GetEnumerator()
        {
            return new ArtistEnumerator(this);
        }
        public class ArtistEnumerator : IEnumerator<Artist>
        {
            public ArtistEnumerator(ArtistCollection ar)
            {
                coll = ar;
            }
            ArtistCollection coll;
            int index = -1;
            public Artist Current
            {
                get
                {
                    return coll[index];
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return coll[index];
                }
            }

            public bool MoveNext()
            {
                index++;
                if (index >= coll.Count())
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }

            public void Reset()
            {
                index = -1;
            }

            #region IDisposable Support
            private bool disposedValue = false; // 要检测冗余调用

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: 释放托管状态(托管对象)。
                    }

                    // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                    // TODO: 将大型字段设置为 null。

                    disposedValue = true;
                }
            }

            // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
            // ~ArtistEnumerator() {
            //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            //   Dispose(false);
            // }

            // 添加此代码以正确实现可处置模式。
            public void Dispose()
            {
                // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
                Dispose(true);
                // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
                // GC.SuppressFinalize(this);
            }
            #endregion

        }
    }

