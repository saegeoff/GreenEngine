using System;
using GreenEngine.Model;
using GreenEngine.Results;
using GreenEngine;

namespace GreenEngineConsole.Tests
{
    public class BeamTest1 : Test
    {
        public BeamTest1()
        {
            m_TestName = "Beam Test 1";
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
            node2.X = 120.0;
            node2.Y = 0.0;

            m_Model.Nodes.Add(node1);
            m_Model.Nodes.Add(node2);

            Support support1 = new Support();
            support1.Tx = TranslationType.Constrained;
            support1.Ty = TranslationType.Constrained;
            support1.Rz = RotationType.Constrained;
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

            m_Model.Elements.Add(beam1);

            ConcentratedNodalLoad load1 = new ConcentratedNodalLoad();
            load1.Node = node2;
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
                if (disp.NodeId == 2)
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

