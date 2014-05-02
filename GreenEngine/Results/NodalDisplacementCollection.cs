using System;
using System.Collections.Generic;

namespace GreenEngine.Results
{
    public class NodalDisplacementCollection : IList<NodalDisplacement>
    {
        IList<NodalDisplacement> m_Data = new List<NodalDisplacement>();
        
        public NodalDisplacementCollection()
        {
        }
        
        public int IndexOf(NodalDisplacement item)
        {
            return m_Data.IndexOf(item);
        }
        
        public void Insert(int index, NodalDisplacement item)
        {
            m_Data.Insert(index, item);
        }
        
        public void RemoveAt(int index)
        {
            m_Data.RemoveAt(index);
        }
        
        public NodalDisplacement this[int index]
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
        
        public void Add(NodalDisplacement item)
        {
            m_Data.Add(item);
        }
        
        public void Clear()
        {
            m_Data.Clear();
        }
        
        public bool Contains(NodalDisplacement item)
        {
            return m_Data.Contains(item);
        }
        
        public void CopyTo(NodalDisplacement[] array, int arrayIndex)
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
        
        public bool Remove(NodalDisplacement item)
        {
            return m_Data.Remove(item);
        }
        
        public IEnumerator<NodalDisplacement> GetEnumerator()
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

