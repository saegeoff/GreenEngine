using System;
using System.Collections.Generic;
using GreenEngine.Model;
using GreenEngine.ElementMatrices;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using GreenEngine.Results;


namespace GreenEngine
{
    public class LinearEngine2d
	{
        protected FiniteElementModel m_Model;

        List<ElementMatrix> m_ElementMatrixList = new List<ElementMatrix>();
        SortedSet<Tuple<int, DegreeType>> m_AllDegreeOfFreedomSet = new SortedSet<Tuple<int, DegreeType>>();
        SortedSet<Tuple<int, DegreeType>> m_SupportDegreeOfFreedomSet = new SortedSet<Tuple<int, DegreeType>>();
        Dictionary<Tuple<int, DegreeType>, int> m_AllGlobalIndexDictionary = new Dictionary<Tuple<int, DegreeType>, int>();
        Dictionary<Tuple<int, DegreeType>, int> m_SupportGlobalIndexDictionary = new Dictionary<Tuple<int, DegreeType>, int>();
        Dictionary<Tuple<int, DegreeType>, int> m_GlobalIndexDictionary = new Dictionary<Tuple<int, DegreeType>, int>();

        Matrix<double> m_GlobalStiffnessMatrix;
        Vector<double> m_LoadsVector;
        Vector<double> m_DisplacementsVector;
        Vector<double> m_SupportReactionsVector;

        public LinearEngine2d()
		{
		}

        public AnalysisResults Analyze(FiniteElementModel model)
		{
            m_Model = model;

            m_ElementMatrixList.Clear();
            m_AllDegreeOfFreedomSet.Clear();
            m_SupportDegreeOfFreedomSet.Clear();
            m_AllGlobalIndexDictionary.Clear();
            m_SupportGlobalIndexDictionary.Clear();
            m_GlobalIndexDictionary.Clear();

            // Build local stiffness matrix for each element
            BuildLocalStiffnessMatricies();
                
            // Build support degrees of freedom set
            BuildSupportDegreesOfFreedomSet();

            // Build global index dictionaries
            BuildGlobalIndexDictionaries();

            // Build Global Stiffness Matrix
            BuildGlobalStiffnessMatrix();

            // Build Loads Vector
            BuildLoadsVector();

            // Solve problem
            m_DisplacementsVector = m_GlobalStiffnessMatrix.Solve(m_LoadsVector);

            // Compute Stresses -- Need to pull this out of results gen

            // Compute Support Reactions
            BuildSupportReactionsVector();

            // debugging
            //Console.WriteLine(m_GlobalStiffnessMatrix.ToString());
            //Console.WriteLine(m_LoadsVector.ToString());
            //Console.WriteLine(m_DisplacementsVector.ToString());

            // Build Results
            return BuildResults();
        }

        protected void BuildLocalStiffnessMatricies()
        {
            // Build local stiffness matrices
            foreach (Element element in m_Model.Elements)
            {
                ElementMatrix elementMatrix;

                if (element is TrussElement)
                {
                    elementMatrix = new TrussElementMatrix2d((TrussElement)element);
                }
                else if (element is BeamElement)
                {
                    elementMatrix = new BeamElementMatrix2d((BeamElement)element);
                }
                else
                {
                    System.Diagnostics.Debug.Assert(false);
                    continue;
                }

                m_AllDegreeOfFreedomSet.UnionWith(elementMatrix.GetDegreesOfFreedomTupleList());
                m_ElementMatrixList.Add(elementMatrix);
            }
        }

        protected void BuildSupportDegreesOfFreedomSet()
        {
            foreach (Support support in m_Model.Supports)
            {
                if (support.Tx == Support.TranslationType.Constrained)
                    m_SupportDegreeOfFreedomSet.Add(new Tuple<int, DegreeType>(support.Node.NodeId, DegreeType.Fx));

                if (support.Ty == Support.TranslationType.Constrained)
                    m_SupportDegreeOfFreedomSet.Add(new Tuple<int, DegreeType>(support.Node.NodeId, DegreeType.Fy));

                if (support.Rz == Support.RotationType.Constrained)
                    m_SupportDegreeOfFreedomSet.Add(new Tuple<int, DegreeType>(support.Node.NodeId, DegreeType.Mz));
            }
        }

        protected void BuildGlobalIndexDictionaries()
        {
            int iGlobalPosition = 0;
            int iSupportGlobalPosition = 0;
            int iAllGlobalPosition = 0;
            foreach (Tuple<int, DegreeType> dof in m_AllDegreeOfFreedomSet)
            {
                if (!m_SupportDegreeOfFreedomSet.Contains(dof))
                {
                    m_GlobalIndexDictionary.Add(dof, iGlobalPosition++);
                    m_SupportGlobalIndexDictionary.Add(dof, -1);
                } 
                else
                {
                    m_GlobalIndexDictionary.Add(dof, -1);
                    m_SupportGlobalIndexDictionary.Add(dof, iSupportGlobalPosition++);
                }

                m_AllGlobalIndexDictionary.Add(dof, iAllGlobalPosition++);
            }
        }

        protected void BuildGlobalStiffnessMatrix()
        {
            int iMatrixDimensionSize = m_AllDegreeOfFreedomSet.Count;

            foreach (Tuple<int, DegreeType> tuple in m_SupportDegreeOfFreedomSet)
            {
                if (m_AllDegreeOfFreedomSet.Contains(tuple))
                    --iMatrixDimensionSize;
            }

            m_GlobalStiffnessMatrix = new SparseMatrix(iMatrixDimensionSize, iMatrixDimensionSize);
            foreach (ElementMatrix matrix in m_ElementMatrixList)
            {
                matrix.CopyToGlobalMatrix(m_GlobalStiffnessMatrix, m_GlobalIndexDictionary);
            }
        }

        protected void BuildLoadsVector()
        {
            int iVectorDimensionSize = m_AllDegreeOfFreedomSet.Count;

            foreach (Tuple<int, DegreeType> tuple in m_SupportDegreeOfFreedomSet)
            {
                if (m_AllDegreeOfFreedomSet.Contains(tuple))
                    --iVectorDimensionSize;
            }

            m_LoadsVector = new SparseVector(iVectorDimensionSize);
            foreach (Load load in m_Model.Loads)
            {
                if (load is ConcentratedNodalLoad)
                {
                    ConcentratedNodalLoad conLoad = (ConcentratedNodalLoad)load;

                    Tuple<int, DegreeType> xTuple = new Tuple<int, DegreeType>(conLoad.Node.NodeId, DegreeType.Fx);
                    Tuple<int, DegreeType> yTuple = new Tuple<int, DegreeType>(conLoad.Node.NodeId, DegreeType.Fy);

                    int xIndex = -1;
                    int yIndex = -1;

                    if (m_GlobalIndexDictionary.ContainsKey(xTuple))
                        xIndex = m_GlobalIndexDictionary[xTuple];

                    if (m_GlobalIndexDictionary.ContainsKey(yTuple))
                        yIndex = m_GlobalIndexDictionary[yTuple];

                    if (xIndex >= 0)
                    {
                        m_LoadsVector[xIndex] += conLoad.X;
                    }

                    if (yIndex >= 0)
                    {
                        m_LoadsVector[yIndex] += conLoad.Y;
                    }
                }
            }
        }

        protected void BuildSupportReactionsVector()
        {
            Matrix<double> supportGlobalStiffnessMatrix = new SparseMatrix(m_SupportDegreeOfFreedomSet.Count, m_AllDegreeOfFreedomSet.Count);

            foreach (ElementMatrix matrix in m_ElementMatrixList)
            {
                matrix.CopyToSupportMatrix(supportGlobalStiffnessMatrix, m_SupportGlobalIndexDictionary, m_AllGlobalIndexDictionary);
            }

            Vector<double> displacementVector = new SparseVector(m_AllDegreeOfFreedomSet.Count);
            foreach (Tuple<int, DegreeType> dof in m_AllDegreeOfFreedomSet)
            {
                int globalIndex = m_GlobalIndexDictionary[dof];
                int allGlobalIndex = m_AllGlobalIndexDictionary[dof];

                if (globalIndex >= 0)
                    displacementVector[allGlobalIndex] = m_DisplacementsVector[globalIndex];
            }

            m_SupportReactionsVector = supportGlobalStiffnessMatrix.Multiply(displacementVector);
        }

        protected AnalysisResults BuildResults()
        {
            ResultsBuilder resultsBuilder = new ResultsBuilder();

            resultsBuilder.Model = m_Model;
            resultsBuilder.ElementMatrixList = m_ElementMatrixList;
            resultsBuilder.AllDegreeOfFreedomSet = m_AllDegreeOfFreedomSet;
            resultsBuilder.SupportDegreeOfFreedomSet = m_SupportDegreeOfFreedomSet;
            resultsBuilder.AllGlobalIndexDictionary = m_AllGlobalIndexDictionary;
            resultsBuilder.SupportGlobalIndexDictionary = m_SupportGlobalIndexDictionary;
            resultsBuilder.GlobalIndexDictionary = m_GlobalIndexDictionary;
            resultsBuilder.DisplacementsVector = m_DisplacementsVector;
            resultsBuilder.SupportReactionsVector = m_SupportReactionsVector;

            return resultsBuilder.BuildResults();
        }
	}
}

