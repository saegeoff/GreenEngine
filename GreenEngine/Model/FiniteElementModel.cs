using System;
using System.Collections.Generic;

namespace GreenEngine.Model
{
	public class FiniteElementModel
	{
		public FiniteElementModel ()
		{
		}

		public List<Material> MaterialList = new List<Material>();
		public List<Node> NodeList = new List<Node>();
		public List<Element> ElementList = new List<Element>();
        public List<Load> LoadList = new List<Load>();
	}
}

