using System;
using GreenEngine.Model;
using GreenEngine;

namespace GreenEngineConsole
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			FiniteElementModel fem = GetModel ();

            TestEngine engine = new TestEngine();
            engine.Analyze(fem);


		}

		public static FiniteElementModel GetModel()
		{
			FiniteElementModel fem = new FiniteElementModel();
			
			Node node1 = new Node ();
            node1.NodeId = 1;
			node1.X = 0.0;
			node1.Y = 0.0;
            node1.Fx = true;
            node1.Fy = true;
			
			Node node2 = new Node ();
            node2.NodeId = 2;
			node2.X = 10.0;
			node2.Y = 0.0;
            node2.Fy = true;
			
			Node node3 = new Node ();
            node3.NodeId = 3;
			node3.X = 10.0;
			node3.Y = 10.0;
			
			fem.NodeList.Add (node1);
			fem.NodeList.Add (node2);
			fem.NodeList.Add (node3);
			
			Material material = new Material ();
			material.ElasticModulus = 10000000.0;
			
			fem.MaterialList.Add (material);

            TrussElement truss1 = new TrussElement();
            truss1.ElementId = 1;
            truss1.Node1 = node1;
            truss1.Node2 = node2;
            truss1.Material = material;
            truss1.Area = 10.0;

            TrussElement truss2 = new TrussElement();
            truss1.ElementId = 2;
            truss2.Node1 = node2;
            truss2.Node2 = node3;
            truss2.Material = material;
            truss2.Area = 10.0;

            TrussElement truss3 = new TrussElement();
            truss1.ElementId = 3;
            truss3.Node1 = node3;
            truss3.Node2 = node1;
            truss3.Material = material;
            truss3.Area = 10.0;

            fem.ElementList.Add(truss1);
            fem.ElementList.Add(truss2);
            fem.ElementList.Add(truss3);


            ConcentratedLoad load1 = new ConcentratedLoad();
            load1.Load = 1000.0;
            load1.Node = node3;
            load1.X = 1.0;
            load1.Y = 1.0;

			return fem;
		}
	}
}
