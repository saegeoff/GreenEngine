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

namespace GreenEngine.Model
{
    public class NodeCollection : IList<Node>
    {
        IList<Node> m_Data = new List<Node>();
        
        public NodeCollection()
        {
        }

        public int IndexOf(Node item)
        {
            return m_Data.IndexOf(item);
        }

        public void Insert(int index, Node item)
        {
            m_Data.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            m_Data.RemoveAt(index);
        }

        public Node this[int index]
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

        public void Add(Node item)
        {
            m_Data.Add(item);
        }

        public void Clear()
        {
            m_Data.Clear();
        }

        public bool Contains(Node item)
        {
            return m_Data.Contains(item);
        }

        public void CopyTo(Node[] array, int arrayIndex)
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

        public bool Remove(Node item)
        {
            return m_Data.Remove(item);
        }

        public IEnumerator<Node> GetEnumerator()
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

