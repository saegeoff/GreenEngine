using System;
using GreenEngine.Model;
using GreenEngine.Results;
using GreenEngine;

namespace GreenEngineConsole.Tests
{
    public class BeamTest2 : Test
    {
        public BeamTest2()
        {
            m_TestName = "Beam Test 2";
        }

        public override bool Run()
        {
            BuildModel();

            PerformAnalysis();

            return CompareResults();
        }

        protected void BuildModel()
        {
            m_Model = new FiniteElementModel();

            Node node1 = new Node ();
            node1.NodeId = 1;
            node1.X = 0.0;
            node1.Y = 0.0;

            Node node2 = new Node ();
            node2.NodeId = 2;
            node2.X = 24.0;
            node2.Y = 0.0;

            Node node3 = new Node ();
            node3.NodeId = 3;
            node3.X = 48.0;
            node3.Y = 0.0;

            Node node4 = new Node ();
            node4.NodeId = 4;
            node4.X = 72.0;
            node4.Y = 0.0;

            Node node5 = new Node ();
            node5.NodeId = 5;
            node5.X = 96.0;
            node5.Y = 0.0;

            Node node6 = new Node ();
            node6.NodeId = 6;
            node6.X = 120.0;
            node6.Y = 0.0;

            m_Model.Nodes.Add(node1);
            m_Model.Nodes.Add(node2);
            m_Model.Nodes.Add(node3);
            m_Model.Nodes.Add(node4);
            m_Model.Nodes.Add(node5);
            m_Model.Nodes.Add(node6);

            Support support1 = new Support();
            support1.Tx = Support.TranslationType.Constrained;
            support1.Ty = Support.TranslationType.Constrained;
            support1.Rz = Support.RotationType.Constrained;
            support1.Node = node1;

            m_Model.Supports.Add(support1);

            Material material = new Material ();
            material.ElasticModulus = 30.0e6;

            m_Model.Materials.Add (material);

            BeamElement beam1 = new BeamElement();
            beam1.ElementId = 1;
            beam1.Node1 = node1;
            beam1.Node2 = node2;
            beam1.Material = material;
            beam1.MomentOfInertia = 31.745; // 6 x 6 hollow box with 5.5 inner wall (.25 thickness)

            BeamElement beam2 = new BeamElement();
            beam2.ElementId = 2;
            beam2.Node1 = node2;
            beam2.Node2 = node3;
            beam2.Material = material;
            beam2.MomentOfInertia = 31.745; // 6 x 6 hollow box with 5.5 inner wall (.25 thickness)

            BeamElement beam3 = new BeamElement();
            beam3.ElementId = 3;
            beam3.Node1 = node3;
            beam3.Node2 = node4;
            beam3.Material = material;
            beam3.MomentOfInertia = 31.745; // 6 x 6 hollow box with 5.5 inner wall (.25 thickness)

            BeamElement beam4 = new BeamElement();
            beam4.ElementId = 4;
            beam4.Node1 = node4;
            beam4.Node2 = node5;
            beam4.Material = material;
            beam4.MomentOfInertia = 31.745; // 6 x 6 hollow box with 5.5 inner wall (.25 thickness)

            BeamElement beam5 = new BeamElement();
            beam5.ElementId = 5;
            beam5.Node1 = node5;
            beam5.Node2 = node6;
            beam5.Material = material;
            beam5.MomentOfInertia = 31.745; // 6 x 6 hollow box with 5.5 inner wall (.25 thickness)

            m_Model.Elements.Add(beam1);
            m_Model.Elements.Add(beam2);
            m_Model.Elements.Add(beam3);
            m_Model.Elements.Add(beam4);
            m_Model.Elements.Add(beam5);

            ConcentratedNodalLoad load1 = new ConcentratedNodalLoad();
            load1.Node = node6;
            load1.Y = -5000.0;

            m_Model.Loads.Add(load1);
        }

        protected void PerformAnalysis()
        {
            LinearEngine2d engine = new LinearEngine2d();
            m_Results = engine.Analyze(m_Model);
        }

        protected bool CompareResults()
        {
            // Disp = -3.0241
            // Angle = -0.0378 radians, -2.1659 degrees

            foreach (NodalDisplacement disp in m_Results.NodalDisplacements)
            {
                if (disp.NodeId == 6)
                {
                    if (!IsEqualTo(disp.Tx, 0.0))
                        return false;

                    if (!IsEqualTo(disp.Ty, -3.02410))
                        return false;

                    if (!IsEqualTo(disp.Rz, -0.037801))
                        return false;
                }
            }

            foreach (SupportReaction reaction in m_Results.SupportReactions)
            {
                if (reaction.NodeId == 1)
                {
                    if (!IsEqualTo(reaction.Fy, 5000.0))
                        return false;

                    if (!IsEqualTo(reaction.Mz, 600000.0))
                        return false;
                }
            }

            return true;
        }
    }
}

