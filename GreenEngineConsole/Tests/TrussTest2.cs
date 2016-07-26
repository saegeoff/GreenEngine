/*******************************************************
 * Copyright (C) 2016 Geoffrey Trees
 * 
 * This file is part of GreenEngine.
 * 
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 *******************************************************/
using System;
using GreenEngine.Model;
using GreenEngine;
using GreenEngine.Results;

namespace GreenEngineConsole.Tests
{
    public class TrussTest2 : Test
    {
        public TrussTest2()
        {
            m_TestName = "Truss Test 2";
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

            m_Model.Nodes.Add(node1);
            m_Model.Nodes.Add(node2);
            m_Model.Nodes.Add(node3);
            m_Model.Nodes.Add(node4);

            Support support1 = new Support();
            support1.Tx = TranslationType.Constrained;
            support1.Ty = TranslationType.Constrained;
            support1.Node = node1;

            Support support2 = new Support();
            support2.Ty = TranslationType.Constrained;
            support2.Node = node2;

            Support support4 = new Support();
            support4.Tx = TranslationType.Constrained;
            support4.Ty = TranslationType.Constrained;
            support4.Node = node4;

            m_Model.Supports.Add(support1);
            m_Model.Supports.Add(support2);
            m_Model.Supports.Add(support4);

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

            m_Model.Elements.Add(truss1);
            m_Model.Elements.Add(truss2);
            m_Model.Elements.Add(truss3);
            m_Model.Elements.Add(truss4);


            ConcentratedNodalLoad load1 = new ConcentratedNodalLoad();
            load1.Node = node2;
            load1.X = 20000.0;

            ConcentratedNodalLoad load2 = new ConcentratedNodalLoad();
            load2.Node = node3;
            load2.Y = -25000.0;

            m_Model.Loads.Add(load1);
            m_Model.Loads.Add(load2);
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
                    if (!IsEqualTo(disp.Tx, 27.12e-3))
                        return false;

                    if (!IsEqualTo(disp.Ty, 0.0))
                        return false;
                }

                if (disp.NodeId == 3)
                {
                    if (!IsEqualTo(disp.Tx, 5.65e-3))
                        return false;

                    if (!IsEqualTo(disp.Ty, -22.25e-3))
                        return false;
                }
            }

            foreach (ElementAction action in m_Results.ElementActions)
            {
                if (action.ElementId == 1)
                {
                    if (!IsEqualTo(action.Stress, 20000.0))
                        return false;
                }

                if (action.ElementId == 2)
                {
                    if (!IsEqualTo(action.Stress, -21875.0))
                        return false;
                }

                if (action.ElementId == 3)
                {
                    if (!IsEqualTo(action.Stress, -5208.33333))
                        return false;
                }

                if (action.ElementId == 4)
                {
                    if (!IsEqualTo(action.Stress, 4166.66667))
                        return false;
                }
            }

            foreach (SupportReaction reaction in m_Results.SupportReactions)
            {
                if (reaction.NodeId == 1)
                {
                    if (!IsEqualTo(reaction.Fx, -15833.33333))
                        return false;

                    if (!IsEqualTo(reaction.Fy, 3125.0))
                        return false;
                }

                if (reaction.NodeId == 2)
                {
                    if (!IsEqualTo(reaction.Fy, 21875.0))
                        return false;
                }

                if (reaction.NodeId == 4)
                {
                    if (!IsEqualTo(reaction.Fx, -4166.66667))
                        return false;

                    if (!IsEqualTo(reaction.Fy, 0.0))
                        return false;
                }
            }

            return true;
        }
    }
}

