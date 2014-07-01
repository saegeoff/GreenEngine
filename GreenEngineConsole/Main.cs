using System;
using GreenEngine.Model;
using GreenEngine;
using GreenEngine.Results;

namespace GreenEngineConsole
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			FiniteElementModel fem = GetModel2 ();

            LinearEngine2d engine = new LinearEngine2d();
            AnalysisResults results = engine.Analyze(fem);

            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("Node Displacements:");
            foreach (NodalDisplacement disp in results.NodalDisplacements)
            {
                Console.WriteLine(disp);
            }
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Element Stresses:");
            foreach (ElementStress stress in results.ElementStresses)
            {
                Console.WriteLine(stress);
            }
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Support Reactions:");
            foreach (SupportReaction supportReaction in results.SupportReactions)
            {
                Console.WriteLine(supportReaction);
            }
            Console.WriteLine();
            Console.WriteLine();


            Console.ReadKey();
		}

		public static FiniteElementModel GetModel()
		{
			FiniteElementModel fem = new FiniteElementModel();
			
			Node node1 = new Node ();
            node1.NodeId = 1;
			node1.X = 0.0;
			node1.Y = 0.0;
			
			Node node2 = new Node ();
            node2.NodeId = 2;
			node2.X = 10.0;
			node2.Y = 0.0;
			
			Node node3 = new Node ();
            node3.NodeId = 3;
			node3.X = 10.0;
			node3.Y = 10.0;
			
			fem.Nodes.Add(node1);
            fem.Nodes.Add(node2);
            fem.Nodes.Add(node3);

            Support support1 = new Support();
            support1.Tx = Support.TranslationType.Constrained;
            support1.Ty = Support.TranslationType.Constrained;
            support1.Node = node1;

            Support support2 = new Support();
            support2.Ty = Support.TranslationType.Constrained;
            support2.Node = node2;

            fem.Supports.Add(support1);
            fem.Supports.Add(support2);
			
			Material material = new Material ();
            material.ElasticModulus = 29.5e6;
			
			fem.Materials.Add (material);

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
            truss3.Node1 = node3;
            truss3.Node2 = node1;
            truss3.Material = material;
            truss3.Area = 1.0;

            fem.Elements.Add(truss1);
            fem.Elements.Add(truss2);
            fem.Elements.Add(truss3);


            ConcentratedLoad load1 = new ConcentratedLoad();
            load1.Node = node3;
            load1.X = 707.1;
            load1.Y = 707.1;

            fem.Loads.Add(load1);

			return fem;
		}

        public static FiniteElementModel GetModel2()
        {
            FiniteElementModel fem = new FiniteElementModel();
            
            Node node1 = new Node ();
            node1.NodeId = 1;
            node1.X = 0.0;
            node1.Y = 0.0;
            
            Node node2 = new Node ();
            node2.NodeId = 2;
            node2.X = 40.0;
            node2.Y = 0.0;
            
            Node node3 = new Node ();
            node3.NodeId = 3;
            node3.X = 40.0;
            node3.Y = 30.0;

            Node node4 = new Node ();
            node4.NodeId = 4;
            node4.X = 0.0;
            node4.Y = 30.0;
            
            fem.Nodes.Add(node1);
            fem.Nodes.Add(node2);
            fem.Nodes.Add(node3);
            fem.Nodes.Add(node4);

            Support support1 = new Support();
            support1.Tx = Support.TranslationType.Constrained;
            support1.Ty = Support.TranslationType.Constrained;
            support1.Node = node1;

            Support support2 = new Support();
            support2.Ty = Support.TranslationType.Constrained;
            support2.Node = node2;

            Support support4 = new Support();
            support4.Tx = Support.TranslationType.Constrained;
            support4.Ty = Support.TranslationType.Constrained;
            support4.Node = node4;

            fem.Supports.Add(support1);
            fem.Supports.Add(support2);
            fem.Supports.Add(support4);

            Material material = new Material ();
            material.ElasticModulus = 29.5e6;
            
            fem.Materials.Add (material);
            
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
            
            fem.Elements.Add(truss1);
            fem.Elements.Add(truss2);
            fem.Elements.Add(truss3);
            fem.Elements.Add(truss4);
            
            
            ConcentratedLoad load1 = new ConcentratedLoad();
            load1.Node = node2;
            load1.X = 20000.0;

            ConcentratedLoad load2 = new ConcentratedLoad();
            load2.Node = node3;
            load2.Y = -25000.0;
            
            fem.Loads.Add(load1);
            fem.Loads.Add(load2);

            return fem;
        }
	}
}
