using System;
using System.Collections.Generic;

namespace GreenEngine.Model
{
	public class FiniteElementModel
	{
        protected MaterialCollection m_MaterialCollection = new MaterialCollection();
        protected NodeCollection m_NodeCollection = new NodeCollection();
        protected ElementCollection m_ElementCollection = new ElementCollection();
        protected SupportCollection m_SupportCollection = new SupportCollection();
        protected LoadCollection m_LoadCollection = new LoadCollection();
        
        public FiniteElementModel ()
		{
		}

        public MaterialCollection Materials
        {
            get { return m_MaterialCollection; }
        }

        public NodeCollection Nodes
        {
            get { return m_NodeCollection; }
        }

        public ElementCollection Elements
        {
            get { return m_ElementCollection; }
        }

        public SupportCollection Supports
        {
            get { return m_SupportCollection; }
        }

        public LoadCollection Loads
        {
            get { return m_LoadCollection; }
        }
	}
}

