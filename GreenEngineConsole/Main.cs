using System;
using GreenEngine.Model;

namespace GreenEngineConsole
{
	class MainClass
	{
		public static void Main (string[] args)
		{



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
			
			BeamElement beam1 = new BeamElement ();
			beam1.Node1 = node1;
			beam1.Node2 = node2;
			beam1.Material = material;
			
			BeamElement beam2 = new BeamElement ();
			beam2.Node1 = node2;
			beam2.Node2 = node3;
			beam2.Material = material;
			
			BeamElement beam3 = new BeamElement ();
			beam3.Node1 = node3;
			beam3.Node2 = node1;
			beam3.Material = material;
			
			fem.ElementList.Add (beam1);
			fem.ElementList.Add (beam2);
			fem.ElementList.Add (beam3);

			return fem;
		}
	}
}
