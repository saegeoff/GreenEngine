using System;
using GreenEngine.Model;
using GreenEngine;
using GreenEngine.Results;

namespace GreenEngineConsole.Tests
{
    public class TrussTest3 : Test
    {
        public TrussTest3()
        {
            m_TestName = "Truss Test 3";
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
            node2.X = 155.8845727;
            node2.Y = -90.0;

            Node node3 = new Node ();
            node3.NodeId = 3;
            node3.X = 311.7691454;
            node3.Y = 0.0;

            m_Model.Nodes.Add(node1);
            m_Model.Nodes.Add(node2);
            m_Model.Nodes.Add(node3);

            Support support1 = new Support();
            support1.Tx = TranslationType.Constrained;
            support1.Ty = TranslationType.Constrained;
            support1.Node = node1;

            Support support2 = new Support();
            support2.Tx = TranslationType.Constrained;
            support2.Ty = TranslationType.Constrained;
            support2.Node = node3;

            m_Model.Supports.Add(support1);
            m_Model.Supports.Add(support2);

            Material material = new Material ();
            material.ElasticModulus = 30.0e6;

            m_Model.Materials.Add (material);

            TrussElement truss1 = new TrussElement();
            truss1.ElementId = 1;
            truss1.Node1 = node1;
            truss1.Node2 = node2;
            truss1.Material = material;
            truss1.Area = 0.5;

            TrussElement truss2 = new TrussElement();
            truss2.ElementId = 2;
            truss2.Node1 = node2;
            truss2.Node2 = node3;
            truss2.Material = material;
            truss2.Area = 0.5;

            m_Model.Elements.Add(truss1);
            m_Model.Elements.Add(truss2);


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
            foreach (NodalDisplacement disp in m_Results.NodalDisplacements)
            {
                if (disp.NodeId == 2)
                {
                    if (!IsEqualTo(disp.Tx, 0.0))
                        return false;

                    if (!IsEqualTo(disp.Ty, -0.120))
                        return false;
                }
            }

            foreach (ElementAction action in m_Results.ElementActions)
            {
                if (action.ElementId == 1)
                {
                    if (!IsEqualTo(action.Stress, 10000.0))
                        return false;
                }

                if (action.ElementId == 2)
                {
                    if (!IsEqualTo(action.Stress, 10000.0))
                        return false;
                }
            }

            foreach (SupportReaction reaction in m_Results.SupportReactions)
            {
                if (reaction.NodeId == 1)
                {
                    if (!IsEqualTo(reaction.Fx, -4330.12702))
                        return false;

                    if (!IsEqualTo(reaction.Fy, 2500.0))
                        return false;
                }

                if (reaction.NodeId == 3)
                {
                    if (!IsEqualTo(reaction.Fx, 4330.12702))
                        return false;

                    if (!IsEqualTo(reaction.Fy, 2500.0))
                        return false;
                }
            }

            return true;
        }
    }
}

