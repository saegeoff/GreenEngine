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
        SortedSet<Tuple<int, DegreeType>> m_DegreeOfFreedomSet = new SortedSet<Tuple<int, DegreeType>>();
        SortedSet<Tuple<int, DegreeType>> m_DegreeOfFreedomSupportSet = new SortedSet<Tuple<int, DegreeType>>();
        Dictionary<Tuple<int, DegreeType>, int> m_DegreeOfFreedomSolveDictionary = new Dictionary<Tuple<int, DegreeType>, int>();
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
            m_DegreeOfFreedomSet.Clear();
            m_DegreeOfFreedomSupportSet.Clear();
            m_DegreeOfFreedomSolveDictionary.Clear();

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

                m_DegreeOfFreedomSet.UnionWith(elementMatrix.GetDegreesOfFreedom());
                m_ElementMatrixList.Add(elementMatrix);
            }
        }

        protected void BuildSupportDegreesOfFreedomSet()
        {
            foreach (Support support in m_Model.Supports)
            {
                if (support.Tx == Support.TranslationType.Constrained)
                    m_DegreeOfFreedomSupportSet.Add(new Tuple<int, DegreeType>(support.Node.NodeId, DegreeType.X));

                if (support.Ty == Support.TranslationType.Constrained)
                    m_DegreeOfFreedomSupportSet.Add(new Tuple<int, DegreeType>(support.Node.NodeId, DegreeType.Y));
            }
        }

        protected void BuildDegreeOfFreedomSolveMap()
        {
            int iDofPosition = 0;
            foreach (Tuple<int, DegreeType> dof in m_DegreeOfFreedomSet)
            {
                if (m_DegreeOfFreedomSupportSet.Contains(dof))
                    continue;

                m_DegreeOfFreedomSolveDictionary.Add(dof, iDofPosition++);
            }
        }

        protected void BuildGlobalStiffnessMatrix()
        {
            m_GlobalStiffnessMatrix = new SparseMatrix(m_DegreeOfFreedomSolveDictionary.Count, m_DegreeOfFreedomSolveDictionary.Count);
            foreach (ElementMatrix matrix in m_ElementMatrixList)
            {
                matrix.CopyToGlobal(m_GlobalStiffnessMatrix, m_DegreeOfFreedomSolveDictionary);
            }
        }

        protected void BuildLoadsVector()
        {
            m_LoadsVector = new SparseVector(m_DegreeOfFreedomSolveDictionary.Count);
            foreach (Load load in m_Model.Loads)
            {
                if (load is ConcentratedNodalLoad)
                {
                    ConcentratedNodalLoad conLoad = (ConcentratedNodalLoad)load;
                    int xIndex = -1;
                    int yIndex = -1;
                    Tuple<int, DegreeType> xTuple = new Tuple<int, DegreeType>(conLoad.Node.NodeId, DegreeType.X);
                    Tuple<int, DegreeType> yTuple = new Tuple<int, DegreeType>(conLoad.Node.NodeId, DegreeType.Y);
                    if (m_DegreeOfFreedomSolveDictionary.ContainsKey(xTuple))
                    {
                        xIndex = m_DegreeOfFreedomSolveDictionary[xTuple];
                        m_LoadsVector[xIndex] += conLoad.X;
                    }
                    if (m_DegreeOfFreedomSolveDictionary.ContainsKey(yTuple))
                    {
                        yIndex = m_DegreeOfFreedomSolveDictionary[yTuple];
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
            resultsBuilder.DegreeOfFreedomSet = m_DegreeOfFreedomSet;
            resultsBuilder.DegreeOfFreedomSupportSet = m_DegreeOfFreedomSupportSet;
            resultsBuilder.DegreeOfFreedomSolveDictionary = m_DegreeOfFreedomSolveDictionary;
            resultsBuilder.DisplacementsVector = m_DisplacementsVector;

            return resultsBuilder.BuildResults();
        }
	}
}

