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
        Dictionary<Tuple<int, DegreeType>, int> m_GlobalIndexDictionary = new Dictionary<Tuple<int, DegreeType>, int>();
        Matrix<double> m_GlobalStiffnessMatrix;
        Vector<double> m_LoadsVector;
        Vector<double> m_DisplacementsVector;

        public LinearEngine2d()
		{
		}

        public AnalysisResults Analyze(FiniteElementModel model)
		{
            m_Model = model;

            m_ElementMatrixList.Clear();
            m_AllDegreeOfFreedomSet.Clear();
            m_SupportDegreeOfFreedomSet.Clear();
            m_GlobalIndexDictionary.Clear();

            // Build local stiffness matrix for each element
            BuildLocalStiffnessMatricies();
                
            // Build support degrees of freedom set
            BuildSupportDegreesOfFreedomSet();

            // Build degree of freedom solve map
            BuildDegreeOfFreedomSolveMap();

            // Build Global Stiffness Matrix
            BuildGlobalStiffnessMatrix();

            // Build Loads Vector
            BuildLoadsVector();

            // Solve problem
            m_DisplacementsVector = m_GlobalStiffnessMatrix.Solve(m_LoadsVector);

            // debugging
            Console.WriteLine(m_GlobalStiffnessMatrix.ToString());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(m_LoadsVector.ToString());
            Console.WriteLine();
            Console.WriteLine();

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
                else
                {
                    System.Diagnostics.Debug.Assert(false);
                    continue;
                }

                m_AllDegreeOfFreedomSet.UnionWith(elementMatrix.GetDegreesOfFreedom());
                m_ElementMatrixList.Add(elementMatrix);
            }
        }

        protected void BuildSupportDegreesOfFreedomSet()
        {
            foreach (Support support in m_Model.Supports)
            {
                if (support.Tx == Support.TranslationType.Constrained)
                    m_SupportDegreeOfFreedomSet.Add(new Tuple<int, DegreeType>(support.Node.NodeId, DegreeType.X));

                if (support.Ty == Support.TranslationType.Constrained)
                    m_SupportDegreeOfFreedomSet.Add(new Tuple<int, DegreeType>(support.Node.NodeId, DegreeType.Y));
            }
        }

        protected void BuildDegreeOfFreedomSolveMap()
        {
            int iDofPosition = 0;
            foreach (Tuple<int, DegreeType> dof in m_AllDegreeOfFreedomSet)
            {
                if (!m_SupportDegreeOfFreedomSet.Contains(dof))
                {
                    m_GlobalIndexDictionary.Add(dof, iDofPosition++);
                } 
                else
                {
                    m_GlobalIndexDictionary.Add(dof, -1);
                }
            }
        }

        protected void BuildGlobalStiffnessMatrix()
        {
            int iMatrixDimensionSize = m_AllDegreeOfFreedomSet.Count - m_SupportDegreeOfFreedomSet.Count;
            m_GlobalStiffnessMatrix = new SparseMatrix(iMatrixDimensionSize, iMatrixDimensionSize);
            foreach (ElementMatrix matrix in m_ElementMatrixList)
            {
                matrix.CopyToGlobal(m_GlobalStiffnessMatrix, m_GlobalIndexDictionary);
            }
        }

        protected void BuildLoadsVector()
        {
            int iVectorDimensionSize = m_AllDegreeOfFreedomSet.Count - m_SupportDegreeOfFreedomSet.Count;
            m_LoadsVector = new SparseVector(iVectorDimensionSize);
            foreach (Load load in m_Model.Loads)
            {
                if (load is ConcentratedNodalLoad)
                {
                    ConcentratedNodalLoad conLoad = (ConcentratedNodalLoad)load;

                    Tuple<int, DegreeType> xTuple = new Tuple<int, DegreeType>(conLoad.Node.NodeId, DegreeType.X);
                    Tuple<int, DegreeType> yTuple = new Tuple<int, DegreeType>(conLoad.Node.NodeId, DegreeType.Y);

                    int xIndex = m_GlobalIndexDictionary[xTuple];
                    int yIndex = m_GlobalIndexDictionary[yTuple];

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

        protected AnalysisResults BuildResults()
        {
            ResultsBuilder resultsBuilder = new ResultsBuilder();

            resultsBuilder.Model = m_Model;
            resultsBuilder.ElementMatrixList = m_ElementMatrixList;
            resultsBuilder.AllDegreeOfFreedomSet = m_AllDegreeOfFreedomSet;
            resultsBuilder.SupportDegreeOfFreedomSet = m_SupportDegreeOfFreedomSet;
            resultsBuilder.GlobalIndexDictionary = m_GlobalIndexDictionary;
            resultsBuilder.DisplacementsVector = m_DisplacementsVector;

            return resultsBuilder.BuildResults();
        }
	}
}

