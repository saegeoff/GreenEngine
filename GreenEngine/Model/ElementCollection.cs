/*******************************************************
 * Copyright (C) 2014 Geoffrey Trees gptrees@gmail.com
 * 
 * This file is part of GreenEngine.
 * 
 * GreenEngine can not be copied and/or distributed without the express
 * permission of Geoffrey Trees
 *******************************************************/
using System;
using System.Collections.Generic;

namespace GreenEngine.Model
{
    public class ElementCollection : IList<Element>
    {
        IList<Element> m_Data = new List<Element>();
        
        public ElementCollection()
        {
        }

        public int IndexOf(Element item)
        {
            return m_Data.IndexOf(item);
        }

        public void Insert(int index, Element item)
        {
            m_Data.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            m_Data.RemoveAt(index);
        }

        public Element this[int index]
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

        public void Add(Element item)
        {
            m_Data.Add(item);
        }

        public void Clear()
        {
            m_Data.Clear();
        }

        public bool Contains(Element item)
        {
            return m_Data.Contains(item);
        }

        public void CopyTo(Element[] array, int arrayIndex)
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

        public bool Remove(Element item)
        {
            return m_Data.Remove(item);
        }

        public IEnumerator<Element> GetEnumerator()
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

