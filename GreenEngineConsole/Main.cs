using System;
using GreenEngine.Model;
using GreenEngine;

namespace GreenEngineConsole
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			FiniteElementModel fem = GetModel2 ();

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
            truss2.ElementId = 2;
            truss2.Node1 = node2;
            truss2.Node2 = node3;
            truss2.Material = material;
            truss2.Area = 10.0;

            TrussElement truss3 = new TrussElement();
            truss3.ElementId = 3;
            truss3.Node1 = node3;
            truss3.Node2 = node1;
            truss3.Material = material;
            truss3.Area = 10.0;

            fem.ElementList.Add(truss1);
            fem.ElementList.Add(truss2);
            fem.ElementList.Add(truss3);


            ConcentratedLoad load1 = new ConcentratedLoad();
            load1.Node = node3;
            load1.X = 707.1;
            load1.Y = 707.1;

            fem.LoadList.Add(load1);

			return fem;
		}

        public static FiniteElementModel GetModel2()
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
            node2.X = 40.0;
            node2.Y = 0.0;
            node2.Fy = true;
            
            Node node3 = new Node ();
            node3.NodeId = 3;
            node3.X = 40.0;
            node3.Y = 30.0;

            Node node4 = new Node ();
            node4.NodeId = 4;
            node4.X = 0.0;
            node4.Y = 30.0;
            node4.Fx = true;
            node4.Fy = true;
            
            fem.NodeList.Add (node1);
            fem.NodeList.Add (node2);
            fem.NodeList.Add (node3);
            fem.NodeList.Add (node4);
            
            Material material = new Material ();
            material.ElasticModulus = 29.5 * Math.Pow(10.0, 6.0);
            
            fem.MaterialList.Add (material);
            
            TrussElement truss1 = new TrussElement();
            truss1.ElementId = 1;
            truss1.Node1 = node1;
            truss1.Node2 = node2;
            truss1.Material = material;
            truss1.Area = 1.0;
            
            TrussElement truss2 = new TrussElement();
            truss2.ElementId = 2;
            truss2.Node1 = node2;
            truss2.Node2 = node3;
            truss2.Material = material;
            truss2.Area = 1.0;
            
            TrussElement truss3 = new TrussElement();
            truss3.ElementId = 3;
            truss3.Node1 = node1;
            truss3.Node2 = node3;
            truss3.Material = material;
            truss3.Area = 1.0;

            TrussElement truss4 = new TrussElement();
            truss4.ElementId = 4;
            truss4.Node1 = node3;
            truss4.Node2 = node4;
            truss4.Material = material;
            truss4.Area = 1.0;
            
            fem.ElementList.Add(truss1);
            fem.ElementList.Add(truss2);
            fem.ElementList.Add(truss3);
            fem.ElementList.Add(truss4);
            
            
            ConcentratedLoad load1 = new ConcentratedLoad();
            load1.Node = node2;
            load1.X = 20000.0;

            ConcentratedLoad load2 = new ConcentratedLoad();
            load2.Node = node3;
            load2.Y = -25000.0;
            
            fem.LoadList.Add(load1);
            fem.LoadList.Add(load2);

            return fem;
        }
	}
}
