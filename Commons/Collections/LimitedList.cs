using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bOS.Commons.Collections
{
    public class LimitedList<T>
    {
        private object oLock = new object();
        private List<T> list = new List<T>();

        private int maxitems = 10;
        public int MaxItems
        {
            get { return this.maxitems; }
            set { this.maxitems = value; }
        }

        public LimitedList()
        {

        }

        public List<T> GetList()
        {
            lock (oLock)
            {
                return list;
            }
        }

        public void Add(T item)
        {
            lock (oLock)
            {
                list.Add(item);
                if (list.Count > maxitems)
                {
                    list.RemoveAt(0);
                }
            }
        }
    }
}