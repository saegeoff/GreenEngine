/*******************************************************
 * Copyright (C) 2016 Geoffrey Trees
 * 
 * This file is part of GreenEngine.
 * 
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 *******************************************************/
using System;
using System.Collections.Generic;

namespace GreenEngine.Results
{
    public class ElementActionCollection : IList<ElementAction>
    {
        IList<ElementAction> m_Data = new List<ElementAction>();
        
        public ElementActionCollection()
        {
        }
        
        public int IndexOf(ElementAction item)
        {
            return m_Data.IndexOf(item);
        }
        
        public void Insert(int index, ElementAction item)
        {
            m_Data.Insert(index, item);
        }
        
        public void RemoveAt(int index)
        {
            m_Data.RemoveAt(index);
        }
        
        public ElementAction this[int index]
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
        
        public void Add(ElementAction item)
        {
            m_Data.Add(item);
        }
        
        public void Clear()
        {
            m_Data.Clear();
        }
        
        public bool Contains(ElementAction item)
        {
            return m_Data.Contains(item);
        }
        
        public void CopyTo(ElementAction[] array, int arrayIndex)
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
        
        public bool Remove(ElementAction item)
        {
            return m_Data.Remove(item);
        }
        
        public IEnumerator<ElementAction> GetEnumerator()
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

