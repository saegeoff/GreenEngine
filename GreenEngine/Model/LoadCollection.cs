using System;
using System.Collections.Generic;

namespace GreenEngine.Model
{
    public class LoadCollection : IList<Load>
    {
        IList<Load> m_Data = new List<Load>();
        
        public LoadCollection()
        {
        }

        public int IndexOf(Load item)
        {
            return m_Data.IndexOf(item);
        }

        public void Insert(int index, Load item)
        {
            m_Data.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            m_Data.RemoveAt(index);
        }

        public Load this[int index]
        {
            get
            {
                return m_Data[index];
            }
            set
            {
                m_Data[index] = value;
            }
        }

        public void Add(Load item)
        {
            m_Data.Add(item);
        }

        public void Clear()
        {
            m_Data.Clear();
        }

        public bool Contains(Load item)
        {
            return m_Data.Contains(item);
        }

        public void CopyTo(Load[] array, int arrayIndex)
        {
            m_Data.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return m_Data.Count;  }
        }

        public bool IsReadOnly
        {
            get { return m_Data.IsReadOnly; }
        }

        public bool Remove(Load item)
        {
            return m_Data.Remove(item);
        }

        public IEnumerator<Load> GetEnumerator()
        {
            return m_Data.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            System.Collections.IEnumerable e = (System.Collections.IEnumerable)m_Data;
            return e.GetEnumerator();
        }
    }
}

