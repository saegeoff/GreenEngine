using System;
using GreenEngine.Model;
using GreenEngine.Results;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using GreenEngine.ElementMatrices;

namespace GreenEngine
{
    class ResultsBuilder
    {
        protected FiniteElementModel m_Model;
        protected List<ElementMatrix> m_ElementMatrixList;
        protected SortedSet<Tuple<int, DegreeType>> m_AllDegreeOfFreedomSet;
        protected SortedSet<Tuple<int, DegreeType>> m_SupportDegreeOfFreedomSet;
        protected Dictionary<Tuple<int, DegreeType>, int> m_AllGlobalIndexDictionary;
        protected Dictionary<Tuple<int, DegreeType>, int> m_GlobalIndexDictionary;
        protected Vector<double> m_DisplacementsVector;
        //protected Matrix<double> globalStiffnessMatrix;

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

        public Vector<double> DisplacementsVector
        { 
            get { return m_DisplacementsVector; }
            set { m_DisplacementsVector = value; }
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

                    if (degree.Item2 == DegreeType.X)
                        displacement.X = m_DisplacementsVector[index];
                    else if (degree.Item2 == DegreeType.Y)
                        displacement.Y = m_DisplacementsVector[index];
                    else
                        System.Diagnostics.Debug.Assert(false);
                }
            }
        }

        protected void PopulateStresses(AnalysisResults results)
        {
            Dictionary<int, ElementStress> stressDictionary = new Dictionary<int, ElementStress>();
            foreach (Element element in m_Model.Elements)
            {
                ElementStress stress = new ElementStress();
                stress.ElementId = element.ElementId;
                results.ElementStresses.Add(stress);
                stressDictionary.Add(element.ElementId, stress);  
            }

            foreach (ElementMatrix matrix in m_ElementMatrixList)
            {
                if (matrix is TrussElementMatrix2d)
                {
                    TrussElementMatrix2d trussMatrix = (TrussElementMatrix2d)matrix;
                    ElementStress stress = stressDictionary [trussMatrix.ElementId];

                    double q1 = 0.0;
                    double q2 = 0.0;
                    double q3 = 0.0;
                    double q4 = 0.0;

                    Tuple<int, DegreeType> x1Tuple = new Tuple<int, DegreeType>(trussMatrix.NodeId1, DegreeType.X);
                    Tuple<int, DegreeType> y1Tuple = new Tuple<int, DegreeType>(trussMatrix.NodeId1, DegreeType.Y);
                    Tuple<int, DegreeType> x2Tuple = new Tuple<int, DegreeType>(trussMatrix.NodeId2, DegreeType.X);
                    Tuple<int, DegreeType> y2Tuple = new Tuple<int, DegreeType>(trussMatrix.NodeId2, DegreeType.Y);

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
            int iNumSupportConstraints = 0;
            Dictionary<int, SupportReaction> supportReactionDictionary = new Dictionary<int, SupportReaction>();
            foreach (Support support in m_Model.Supports)
            {
                SupportReaction supportReaction = new SupportReaction();
                supportReaction.NodeId = support.Node.NodeId;
                results.SupportReactions.Add(supportReaction);
                supportReactionDictionary.Add(support.Node.NodeId, supportReaction);

                iNumSupportConstraints += support.GetNumberOfConstraints();
            }

            //            Matrix<double> supportStiffnessMatrix = new SparseMatrix(fem.Nodes.Count * 2 /* nodes * xy(2) */, iNumSupportConstraints);
            //
            //
            //            Vector<double> supportStiffnessDisplacements = new SparseVector(fem.Nodes.Count * 2 /* nodes * xy(2) */);
            //
            //            for (int i = 0; i < fem.Nodes.Count; i++)
            //            {
            //                Node node = fem.Nodes [i];
            //
            //                int xIndex = elementDegreeSolveList.FindIndex(x => x.Item1 == node.NodeId && x.Item2 == DegreeType.X);
            //                int yIndex = elementDegreeSolveList.FindIndex(x => x.Item1 == node.NodeId && x.Item2 == DegreeType.Y);
            //
            //                if (xIndex >= 0)
            //                    supportStiffnessDisplacements [(i * 2)] = displacements [xIndex];
            //                else
            //                    supportStiffnessDisplacements [(i * 2)] = 0.0;
            //
            //                if (yIndex >= 0)
            //                    supportStiffnessDisplacements [(i * 2) + 1] = displacements [yIndex];
            //                else
            //                    supportStiffnessDisplacements [(i * 2) + 1] = 0.0;
            //            }

            //            SortedSet<Tuple<int, DegreeType>> elementSupportSolveSet = new SortedSet<Tuple<int, DegreeType>>(elementDegreeSet);
            //
            //
            //            // Remove supports from degrees of freedom solve set
            //            foreach (Support support in fem.Supports)
            //            {
            //                if (support.Tx == Support.TranslationType.Constrained)
            //                    elementSupportSolveSet.Remove(new Tuple<int, DegreeType>(support.Node.NodeId, DegreeType.X));
            //
            //                if (support.Ty == Support.TranslationType.Constrained)
            //                    elementSupportSolveSet.Remove(new Tuple<int, DegreeType>(support.Node.NodeId, DegreeType.Y));
            //            }
            //
            //            List<Tuple<int, DegreeType>> elementSupportSolveList = new List<Tuple<int, DegreeType>>(elementDegreeSolveSet);

        }
    }
}

