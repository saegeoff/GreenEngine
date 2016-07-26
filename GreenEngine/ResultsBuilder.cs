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
using GreenEngine.Results;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using GreenEngine.ElementMatrices;
using System.Diagnostics;

namespace GreenEngine
{
    class ResultsBuilder
    {
        protected FiniteElementModel m_Model;
        protected AnalysisDomain m_Domain;

        public ResultsBuilder()
        {
        }

        public FiniteElementModel Model
        { 
            get { return m_Model; }
            set { m_Model = value; }
        }

        public AnalysisDomain Domain
        {
            get { return m_Domain; }
            set { m_Domain = value; }
        }

        public AnalysisResults BuildResults()
        {
            AnalysisResults results = new AnalysisResults();

            // Displacements
            PopulateDisplacements(results);

            // Element Stresses
            PopulateStresses(results);

            // Support Reactions
            PopulateSupportReactions(results);

            return results;
        }

        protected void PopulateDisplacements(AnalysisResults results)
        {
            Dictionary<int, NodalDisplacement> displacementDictionary = new Dictionary<int, NodalDisplacement>();
            foreach (Node node in m_Model.Nodes)
            {
                NodalDisplacement displacement = new NodalDisplacement();
                displacement.NodeId = node.NodeId;
                results.NodalDisplacements.Add(displacement);
                displacementDictionary.Add(node.NodeId, displacement);  
            }

            foreach (Tuple<int, DegreeType> degree in m_Domain.AllDegreeOfFreedomSet)
            {
                int index = m_Domain.GlobalIndexDictionary[degree];

                if (index >= 0)
                {
                    NodalDisplacement displacement = displacementDictionary[degree.Item1];

                    if (degree.Item2 == DegreeType.Fx)
                        displacement.Tx = m_Domain.DisplacementsVector[index];
                    else if (degree.Item2 == DegreeType.Fy)
                        displacement.Ty = m_Domain.DisplacementsVector[index];
                    else if (degree.Item2 == DegreeType.Mz)
                        displacement.Rz = m_Domain.DisplacementsVector[index];
                    else
                        System.Diagnostics.Debug.Assert(false);
                }
            }
        }

        protected void PopulateStresses(AnalysisResults results)
        {
            Dictionary<int, ElementAction> stressDictionary = new Dictionary<int, ElementAction>();
            foreach (Element element in m_Model.Elements)
            {
                ElementAction stress = new ElementAction();
                stress.ElementId = element.ElementId;
                results.ElementActions.Add(stress);
                stressDictionary.Add(element.ElementId, stress);  
            }

            foreach (ElementMatrix matrix in m_Domain.ElementMatrixList)
            {
                if (matrix is TrussElementMatrix2d)
                {
                    TrussElementMatrix2d trussMatrix = (TrussElementMatrix2d)matrix;
                    ElementAction stress = stressDictionary [trussMatrix.ElementId];

                    double q1 = 0.0;
                    double q2 = 0.0;
                    double q3 = 0.0;
                    double q4 = 0.0;

                    Tuple<int, DegreeType> x1Tuple = new Tuple<int, DegreeType>(trussMatrix.NodeId1, DegreeType.Fx);
                    Tuple<int, DegreeType> y1Tuple = new Tuple<int, DegreeType>(trussMatrix.NodeId1, DegreeType.Fy);
                    Tuple<int, DegreeType> x2Tuple = new Tuple<int, DegreeType>(trussMatrix.NodeId2, DegreeType.Fx);
                    Tuple<int, DegreeType> y2Tuple = new Tuple<int, DegreeType>(trussMatrix.NodeId2, DegreeType.Fy);

                    int q1Index = m_Domain.GlobalIndexDictionary[x1Tuple];
                    int q2Index = m_Domain.GlobalIndexDictionary[y1Tuple];
                    int q3Index = m_Domain.GlobalIndexDictionary[x2Tuple];
                    int q4Index = m_Domain.GlobalIndexDictionary[y2Tuple];

                    if (q1Index >= 0)
                        q1 = m_Domain.DisplacementsVector [q1Index];

                    if (q2Index >= 0)
                        q2 = m_Domain.DisplacementsVector [q2Index];

                    if (q3Index >= 0)
                        q3 = m_Domain.DisplacementsVector [q3Index];

                    if (q4Index >= 0)
                        q4 = m_Domain.DisplacementsVector [q4Index];

                    stress.Stress = trussMatrix.GetStress(q1, q2, q3, q4);
                }
            }
        }

        protected void PopulateSupportReactions(AnalysisResults results)
        {
            Dictionary<int, SupportReaction> supportReactionDictionary = new Dictionary<int, SupportReaction>();
            foreach (Support support in m_Model.Supports)
            {
                SupportReaction supportReaction = new SupportReaction();
                supportReaction.NodeId = support.Node.NodeId;
                results.SupportReactions.Add(supportReaction);
                supportReactionDictionary.Add(support.Node.NodeId, supportReaction);
            }

            foreach (Tuple<int, DegreeType> supportDegree in m_Domain.SupportDegreeOfFreedomSet)
            {
                if (!m_Domain.SupportGlobalIndexDictionary.ContainsKey(supportDegree))
                    continue;

                int supportIndex = m_Domain.SupportGlobalIndexDictionary[supportDegree];
                double reactionValue = m_Domain.SupportReactionsVector[supportIndex];

                SupportReaction supportReaction = supportReactionDictionary[supportDegree.Item1];

                if (supportDegree.Item2 == DegreeType.Fx)
                {
                    supportReaction.Fx = reactionValue;
                }
                else if (supportDegree.Item2 == DegreeType.Fy)
                {
                    supportReaction.Fy = reactionValue;
                }
                else if (supportDegree.Item2 == DegreeType.Mz)
                {
                    supportReaction.Mz = reactionValue;
                }
                else
                {
                    Debug.Assert(false);
                    continue;
                }
            }
        }
    }
}

