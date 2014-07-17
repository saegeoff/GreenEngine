/*******************************************************
 * Copyright (C) 2014 Geoffrey Trees gptrees@gmail.com
 * 
 * This file is part of GreenEngine.
 * 
 * GreenEngine can not be copied and/or distributed without the express
 * permission of Geoffrey Trees
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
        protected List<ElementMatrix> m_ElementMatrixList;
        protected SortedSet<Tuple<int, DegreeType>> m_AllDegreeOfFreedomSet;
        protected SortedSet<Tuple<int, DegreeType>> m_SupportDegreeOfFreedomSet;
        protected Dictionary<Tuple<int, DegreeType>, int> m_AllGlobalIndexDictionary;
        protected Dictionary<Tuple<int, DegreeType>, int> m_SupportGlobalIndexDictionary;
        protected Dictionary<Tuple<int, DegreeType>, int> m_GlobalIndexDictionary;
        protected Vector<double> m_DisplacementsVector;
        //protected Matrix<double> globalStiffnessMatrix;
        protected Vector<double> m_SupportReactionsVector;

        public ResultsBuilder()
        {
        }

        public FiniteElementModel Model
        { 
            get { return m_Model; }
            set { m_Model = value; }
        }

        public List<ElementMatrix> ElementMatrixList
        { 
            get { return m_ElementMatrixList; }
            set { m_ElementMatrixList = value; }
        }

        public SortedSet<Tuple<int, DegreeType>>  AllDegreeOfFreedomSet
        { 
            get { return m_AllDegreeOfFreedomSet; }
            set { m_AllDegreeOfFreedomSet = value; }
        }

        public SortedSet<Tuple<int, DegreeType>>  SupportDegreeOfFreedomSet
        { 
            get { return m_SupportDegreeOfFreedomSet; }
            set { m_SupportDegreeOfFreedomSet = value; }
        }

        public Dictionary<Tuple<int, DegreeType>, int> AllGlobalIndexDictionary
        { 
            get { return m_AllGlobalIndexDictionary; }
            set { m_AllGlobalIndexDictionary = value; }
        }

        public Dictionary<Tuple<int, DegreeType>, int> GlobalIndexDictionary
        { 
            get { return m_GlobalIndexDictionary; }
            set { m_GlobalIndexDictionary = value; }
        }

        public Dictionary<Tuple<int, DegreeType>, int> SupportGlobalIndexDictionary
        { 
            get { return m_SupportGlobalIndexDictionary; }
            set { m_SupportGlobalIndexDictionary = value; }
        }

        public Vector<double> DisplacementsVector
        { 
            get { return m_DisplacementsVector; }
            set { m_DisplacementsVector = value; }
        }

        public Vector<double> SupportReactionsVector
        { 
            get { return m_SupportReactionsVector; }
            set { m_SupportReactionsVector = value; }
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

            foreach (Tuple<int, DegreeType> degree in m_AllDegreeOfFreedomSet)
            {
                int index = m_GlobalIndexDictionary[degree];

                if (index >= 0)
                {
                    NodalDisplacement displacement = displacementDictionary[degree.Item1];

                    if (degree.Item2 == DegreeType.Fx)
                        displacement.Tx = m_DisplacementsVector[index];
                    else if (degree.Item2 == DegreeType.Fy)
                        displacement.Ty = m_DisplacementsVector[index];
                    else if (degree.Item2 == DegreeType.Mz)
                        displacement.Rz = m_DisplacementsVector[index];
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

            foreach (ElementMatrix matrix in m_ElementMatrixList)
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

                    int q1Index = m_GlobalIndexDictionary[x1Tuple];
                    int q2Index = m_GlobalIndexDictionary[y1Tuple];
                    int q3Index = m_GlobalIndexDictionary[x2Tuple];
                    int q4Index = m_GlobalIndexDictionary[y2Tuple];

                    if (q1Index >= 0)
                        q1 = m_DisplacementsVector [q1Index];

                    if (q2Index >= 0)
                        q2 = m_DisplacementsVector [q2Index];

                    if (q3Index >= 0)
                        q3 = m_DisplacementsVector [q3Index];

                    if (q4Index >= 0)
                        q4 = m_DisplacementsVector [q4Index];

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

            foreach (Tuple<int, DegreeType> supportDegree in m_SupportDegreeOfFreedomSet)
            {
                if (!m_SupportGlobalIndexDictionary.ContainsKey(supportDegree))
                    continue;

                int supportIndex = m_SupportGlobalIndexDictionary[supportDegree];
                double reactionValue = m_SupportReactionsVector[supportIndex];

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

