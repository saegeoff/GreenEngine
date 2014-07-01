using System;
using System.Collections.Generic;

namespace GreenEngine.Results
{
    public class ElementStressCollection : IList<ElementStress>
    {
        IList<ElementStress> m_Data = new List<ElementStress>();
        
        public ElementStressCollection()
        {
        }
        
        public int IndexOf(ElementStress item)
        {
            return m_Data.IndexOf(item);
        }
        
        public void Insert(int index, ElementStress item)
        {
            m_Data.Insert(index, item);
        }
        
        public void RemoveAt(int index)
        {
            m_Data.RemoveAt(index);
        }
        
        public ElementStress this[int index]
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
        
        public void Add(ElementStress item)
        {
            m_Data.Add(item);
        }
        
        public void Clear()
        {
            m_Data.Clear();
        }
        
        public bool Contains(ElementStress item)
        {
            return m_Data.Contains(item);
        }
        
        public void CopyTo(ElementStress[] array, int arrayIndex)
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
        
        public bool Remove(ElementStress item)
        {
            return m_Data.Remove(item);
        }
        
        public IEnumerator<ElementStress> GetEnumerator()
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

