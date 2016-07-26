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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenEngine.Model
{
    public class SupportCollection : IList<Support>
    {
        IList<Support> m_Data = new List<Support>();
        
        public SupportCollection()
        {
        }

        public int IndexOf(Support item)
        {
            return m_Data.IndexOf(item);
        }

        public void Insert(int index, Support item)
        {
            m_Data.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            m_Data.RemoveAt(index);
        }

        public Support this[int index]
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

        public void Add(Support item)
        {
            m_Data.Add(item);
        }

        public void Clear()
        {
            m_Data.Clear();
        }

        public bool Contains(Support item)
        {
            return m_Data.Contains(item);
        }

        public void CopyTo(Support[] array, int arrayIndex)
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

        public bool Remove(Support item)
        {
            return m_Data.Remove(item);
        }

        public IEnumerator<Support> GetEnumerator()
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
