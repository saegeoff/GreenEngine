/*******************************************************
 * Copyright (C) 2016 Geoffrey Trees
 * 
 * This file is part of GreenEngine.
 * 
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 *******************************************************/

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
        private AnalysisDomain m_Domain; 

        public LinearEngine2d()
		{
		}

        public AnalysisResults Analyze(FiniteElementModel model)
		{
            m_Model = model;
            m_Domain = new AnalysisDomain();

            // Build local stiffness matrix for each element
            BuildLocalStiffnessMatricies();
                
            // Build support degrees of freedom set
            BuildSupportDegreesOfFreedomSet();

            // Build global index dictionaries
            BuildGlobalIndexDictionaries();

            // Create Matrix and Vector
            CreateDomainMatrixVector();

            // Build Global Stiffness Matrix
            BuildGlobalStiffnessMatrix();

            // Build Loads Vector
            BuildLoadsVector();

            // Solve problem
            m_Domain.Solve();

            // Compute Stresses -- Need to pull this out of results gen

            // Compute Support Reactions
            BuildSupportReactionsVector();

            // debugging
            //Console.WriteLine(m_GlobalStiffnessMatrix.ToString());
            //Console.WriteLine(m_LoadsVector.ToString());
            //Console.WriteLine(m_DisplacementsVector.ToString());

            // Build Results
            AnalysisResults results = BuildResults();

            // Dereference model and domain
            m_Model = null;
            m_Domain = null;

            // Return results
            return results;
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

                m_Domain.AllDegreeOfFreedomSet.UnionWith(elementMatrix.GetDegreesOfFreedomTupleList());
                m_Domain.ElementMatrixList.Add(elementMatrix);
            }
        }

        protected void BuildSupportDegreesOfFreedomSet()
        {
            // For 2D we just need Tx, Ty and Rz

            foreach (Support support in m_Model.Supports)
            {
                if (support.Tx == TranslationType.Constrained)
                    m_Domain.SupportDegreeOfFreedomSet.Add(new Tuple<int, DegreeType>(support.Node.NodeId, DegreeType.Fx));

                if (support.Ty == TranslationType.Constrained)
                    m_Domain.SupportDegreeOfFreedomSet.Add(new Tuple<int, DegreeType>(support.Node.NodeId, DegreeType.Fy));

                if (support.Rz == RotationType.Constrained)
                    m_Domain.SupportDegreeOfFreedomSet.Add(new Tuple<int, DegreeType>(support.Node.NodeId, DegreeType.Mz));
            }
        }

        protected void BuildGlobalIndexDictionaries()
        {
            int iGlobalPosition = 0;
            int iSupportGlobalPosition = 0;
            int iAllGlobalPosition = 0;
            foreach (Tuple<int, DegreeType> dof in m_Domain.AllDegreeOfFreedomSet)
            {
                if (!m_Domain.SupportDegreeOfFreedomSet.Contains(dof))
                {
                    m_Domain.GlobalIndexDictionary.Add(dof, iGlobalPosition++);
                    m_Domain.SupportGlobalIndexDictionary.Add(dof, -1);
                } 
                else
                {
                    m_Domain.GlobalIndexDictionary.Add(dof, -1);
                    m_Domain.SupportGlobalIndexDictionary.Add(dof, iSupportGlobalPosition++);
                }

                m_Domain.AllGlobalIndexDictionary.Add(dof, iAllGlobalPosition++);
            }
        }

        protected void CreateDomainMatrixVector()
        {
            int iSize = m_Domain.AllDegreeOfFreedomSet.Count;

            foreach (Tuple<int, DegreeType> tuple in m_Domain.SupportDegreeOfFreedomSet)
            {
                if (m_Domain.AllDegreeOfFreedomSet.Contains(tuple))
                    --iSize;
            }

            m_Domain.SetDataSize(iSize);
        }

        protected void BuildGlobalStiffnessMatrix()
        {
            foreach (ElementMatrix matrix in m_Domain.ElementMatrixList)
            {
                matrix.CopyToGlobalMatrix(m_Domain.GlobalStiffnessMatrix, m_Domain.GlobalIndexDictionary);
            }
        }

        protected void BuildLoadsVector()
        {
            foreach (Load load in m_Model.Loads)
            {
                if (load is ConcentratedNodalLoad)
                {
                    ConcentratedNodalLoad conLoad = (ConcentratedNodalLoad)load;

                    Tuple<int, DegreeType> xTuple = new Tuple<int, DegreeType>(conLoad.Node.NodeId, DegreeType.Fx);
                    Tuple<int, DegreeType> yTuple = new Tuple<int, DegreeType>(conLoad.Node.NodeId, DegreeType.Fy);

                    int xIndex = -1;
                    int yIndex = -1;

                    if (m_Domain.GlobalIndexDictionary.ContainsKey(xTuple))
                        xIndex = m_Domain.GlobalIndexDictionary[xTuple];

                    if (m_Domain.GlobalIndexDictionary.ContainsKey(yTuple))
                        yIndex = m_Domain.GlobalIndexDictionary[yTuple];

                    if (xIndex >= 0)
                    {
                        m_Domain.LoadsVector[xIndex] += conLoad.X;
                    }

                    if (yIndex >= 0)
                    {
                        m_Domain.LoadsVector[yIndex] += conLoad.Y;
                    }
                }
            }
        }

        protected void BuildSupportReactionsVector()
        {
            // NOTE: This should probably be moved to the domain eventually.

            Matrix<double> supportGlobalStiffnessMatrix = new SparseMatrix(m_Domain.SupportDegreeOfFreedomSet.Count, m_Domain.AllDegreeOfFreedomSet.Count);

            foreach (ElementMatrix matrix in m_Domain.ElementMatrixList)
            {
                matrix.CopyToSupportMatrix(supportGlobalStiffnessMatrix, m_Domain.SupportGlobalIndexDictionary, m_Domain.AllGlobalIndexDictionary);
            }

            Vector<double> displacementVector = new SparseVector(m_Domain.AllDegreeOfFreedomSet.Count);
            foreach (Tuple<int, DegreeType> dof in m_Domain.AllDegreeOfFreedomSet)
            {
                int globalIndex = m_Domain.GlobalIndexDictionary[dof];
                int allGlobalIndex = m_Domain.AllGlobalIndexDictionary[dof];

                if (globalIndex >= 0)
                    displacementVector[allGlobalIndex] = m_Domain.DisplacementsVector[globalIndex];
            }

            m_Domain.SupportReactionsVector = supportGlobalStiffnessMatrix.Multiply(displacementVector);
        }

        protected AnalysisResults BuildResults()
        {
            ResultsBuilder resultsBuilder = new ResultsBuilder();

            resultsBuilder.Model = m_Model;
            resultsBuilder.Domain = m_Domain;

            return resultsBuilder.BuildResults();
        }
	}
}

