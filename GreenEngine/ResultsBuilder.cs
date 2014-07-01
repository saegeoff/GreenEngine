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
        protected SortedSet<Tuple<int, DegreeType>> m_DegreeOfFreedomSet;
        protected SortedSet<Tuple<int, DegreeType>> m_DegreeOfFreedomSupportSet;
        protected Dictionary<Tuple<int, DegreeType>, int> m_DegreeOfFreedomSolveDictionary;
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

        public SortedSet<Tuple<int, DegreeType>>  DegreeOfFreedomSet
        { 
            get { return m_DegreeOfFreedomSet; }
            set { m_DegreeOfFreedomSet = value; }
        }

        public SortedSet<Tuple<int, DegreeType>>  DegreeOfFreedomSupportSet
        { 
            get { return m_DegreeOfFreedomSupportSet; }
            set { m_DegreeOfFreedomSupportSet = value; }
        }

        public Dictionary<Tuple<int, DegreeType>, int> DegreeOfFreedomSolveDictionary
        { 
            get { return m_DegreeOfFreedomSolveDictionary; }
            set { m_DegreeOfFreedomSolveDictionary = value; }
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

            foreach (Tuple<int, DegreeType> degree in m_DegreeOfFreedomSet)
            {
                if (m_DegreeOfFreedomSolveDictionary.ContainsKey(degree))
                {
                    NodalDisplacement displacement = displacementDictionary[degree.Item1];

                    int index = m_DegreeOfFreedomSolveDictionary[degree];

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

                    if (m_DegreeOfFreedomSolveDictionary.ContainsKey(x1Tuple))
                        q1 = m_DisplacementsVector [m_DegreeOfFreedomSolveDictionary[x1Tuple]];

                    if (m_DegreeOfFreedomSolveDictionary.ContainsKey(y1Tuple))
                        q2 = m_DisplacementsVector [m_DegreeOfFreedomSolveDictionary[y1Tuple]];

                    if (m_DegreeOfFreedomSolveDictionary.ContainsKey(x2Tuple))
                        q3 = m_DisplacementsVector [m_DegreeOfFreedomSolveDictionary[x2Tuple]];

                    if (m_DegreeOfFreedomSolveDictionary.ContainsKey(y2Tuple))
                        q4 = m_DisplacementsVector [m_DegreeOfFreedomSolveDictionary[y2Tuple]];

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

