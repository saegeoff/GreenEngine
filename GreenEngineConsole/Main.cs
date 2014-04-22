using System;
using GreenEngine.Model;

namespace GreenEngineConsole
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			FiniteElementModel fem = GetModel ();




		}

		public static FiniteElementModel GetModel()
		{
			FiniteElementModel fem = new FiniteElementModel();
			
			Node node1 = new Node ();
			node1.X = 0.0;
			node1.Y = 0.0;
			
			Node node2 = new Node ();
			node2.X = 10.0;
			node2.Y = 0.0;
			
			Node node3 = new Node ();
			node3.X = 10.0;
			node3.Y = 10.0;
			
			fem.NodeList.Add (node1);
			fem.NodeList.Add (node2);
			fem.NodeList.Add (node3);
			
			Material material = new Material ();
			material.ElasticModulus = 10000000.0;
			
			fem.MaterialList.Add (material);

            TrussElement truss1 = new TrussElement();
            truss1.Node1 = node1;
            truss1.Node2 = node2;
            truss1.Material = material;

            TrussElement truss2 = new TrussElement();
            truss2.Node1 = node2;
            truss2.Node2 = node3;
            truss2.Material = material;

            TrussElement truss3 = new TrussElement();
            truss3.Node1 = node3;
            truss3.Node2 = node1;
            truss3.Material = material;

            fem.ElementList.Add(truss1);
            fem.ElementList.Add(truss2);
            fem.ElementList.Add(truss3);

			return fem;
		}
	}
}
