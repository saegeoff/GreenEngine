using System;
using GreenEngine.Model;
using GreenEngine;
using GreenEngine.Results;

namespace GreenEngineConsole.Tests
{
    public class TrussTest1 : Test
    {
        public TrussTest1()
        {
            m_TestName = "Truss Test 1";
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
            node2.X = 10.0;
            node2.Y = 0.0;

            Node node3 = new Node ();
            node3.NodeId = 3;
            node3.X = 10.0;
            node3.Y = 10.0;

            m_Model.Nodes.Add(node1);
            m_Model.Nodes.Add(node2);
            m_Model.Nodes.Add(node3);

            Support support1 = new Support();
            support1.Tx = Support.TranslationType.Constrained;
            support1.Ty = Support.TranslationType.Constrained;
            support1.Node = node1;

            Support support2 = new Support();
            support2.Ty = Support.TranslationType.Constrained;
            support2.Node = node2;

            m_Model.Supports.Add(support1);
            m_Model.Supports.Add(support2);

            Material material = new Material ();
            material.ElasticModulus = 29.5e6;

            m_Model.Materials.Add (material);

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

            m_Model.Elements.Add(truss1);
            m_Model.Elements.Add(truss2);
            m_Model.Elements.Add(truss3);


            ConcentratedNodalLoad load1 = new ConcentratedNodalLoad();
            load1.Node = node3;
            load1.X = 707.1;
            load1.Y = 707.1;

            m_Model.Loads.Add(load1);
        }

        protected void PerformAnalysis()
        {
            LinearEngine2d engine = new LinearEngine2d();
            m_Results = engine.Analyze(m_Model);
        }

        protected bool CompareResults()
        {
//            foreach (NodalDisplacement disp in m_Results.NodalDisplacements)
//            {
//                if (disp.NodeId == 2)
//                {
//                    if (!IsEqualTo(disp.X, 27.12e-3))
//                        return false;
//
//                    if (!IsEqualTo(disp.Y, 0.0))
//                        return false;
//                }
//
//                if (disp.NodeId == 3)
//                {
//                    if (!IsEqualTo(disp.X, 5.65e-3))
//                        return false;
//
//                    if (!IsEqualTo(disp.Y, -22.25e-3))
//                        return false;
//                }
//            }
//
//            foreach (ElementStress stress in m_Results.ElementStresses)
//            {
//                if (stress.ElementId == 1)
//                {
//                    if (!IsEqualTo(stress.Stress, 20000.0))
//                        return false;
//                }
//
//                if (stress.ElementId == 2)
//                {
//                    if (!IsEqualTo(stress.Stress, -21875.0))
//                        return false;
//                }
//
//                if (stress.ElementId == 3)
//                {
//                    if (!IsEqualTo(stress.Stress, -5208.33333))
//                        return false;
//                }
//
//                if (stress.ElementId == 4)
//                {
//                    if (!IsEqualTo(stress.Stress, 4166.66667))
//                        return false;
//                }
//            }
//
//            foreach (SupportReaction reaction in m_Results.SupportReactions)
//            {
//                if (reaction.NodeId == 1)
//                {
//                    if (!IsEqualTo(reaction.Rx, -15833.33333))
//                        return false;
//
//                    if (!IsEqualTo(reaction.Ry, 3125.0))
//                        return false;
//                }
//
//                if (reaction.NodeId == 2)
//                {
//                    if (!IsEqualTo(reaction.Ry, 21875.0))
//                        return false;
//                }
//
//                if (reaction.NodeId == 4)
//                {
//                    if (!IsEqualTo(reaction.Rx, -4166.66667))
//                        return false;
//
//                    if (!IsEqualTo(reaction.Ry, 0.0))
//                        return false;
//                }
//            }

            return true;
        }
    }
}

