// /*******************************************************
//  * Copyright (C) 2014 Geoffrey Trees gptrees@gmail.com
//  * 
//  * This file is part of GreenEngine.
//  * 
//  * GreenEngine can not be copied and/or distributed without the express
//  * permission of Geoffrey Trees
//  *******************************************************/
using System;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using GreenEngine.ElementMatrices;
using MathNet.Numerics.LinearAlgebra.Double;

namespace GreenEngine
{
    class AnalysisDomain
    {
        protected List<ElementMatrix> m_ElementMatrixList = new List<ElementMatrix>();
        protected SortedSet<Tuple<int, DegreeType>> m_AllDegreeOfFreedomSet = new SortedSet<Tuple<int, DegreeType>>();
        protected SortedSet<Tuple<int, DegreeType>> m_SupportDegreeOfFreedomSet = new SortedSet<Tuple<int, DegreeType>>();
        protected Dictionary<Tuple<int, DegreeType>, int> m_AllGlobalIndexDictionary = new Dictionary<Tuple<int, DegreeType>, int>();
        protected Dictionary<Tuple<int, DegreeType>, int> m_SupportGlobalIndexDictionary = new Dictionary<Tuple<int, DegreeType>, int>();
        protected Dictionary<Tuple<int, DegreeType>, int> m_GlobalIndexDictionary = new Dictionary<Tuple<int, DegreeType>, int>();

        protected Matrix<double> m_GlobalStiffnessMatrix;
        protected Vector<double> m_LoadsVector;
        protected Vector<double> m_DisplacementsVector;
        protected Vector<double> m_SupportReactionsVector;

        public AnalysisDomain()
        {
        }

        public List<ElementMatrix> ElementMatrixList
        {
            get { return m_ElementMatrixList; }
        }

        public SortedSet<Tuple<int, DegreeType>> AllDegreeOfFreedomSet
        {
            get { return m_AllDegreeOfFreedomSet; }
        }

        public SortedSet<Tuple<int, DegreeType>> SupportDegreeOfFreedomSet
        {
            get { return m_SupportDegreeOfFreedomSet; }
        }

        public Dictionary<Tuple<int, DegreeType>, int> AllGlobalIndexDictionary
        {
            get { return m_AllGlobalIndexDictionary; }
        }

        public Dictionary<Tuple<int, DegreeType>, int> SupportGlobalIndexDictionary
        {
            get { return m_SupportGlobalIndexDictionary; }
        }

        public Dictionary<Tuple<int, DegreeType>, int> GlobalIndexDictionary
        {
            get { return m_GlobalIndexDictionary; }
        }

        public Matrix<double> GlobalStiffnessMatrix
        {
            get { return m_GlobalStiffnessMatrix; }
        }

        public Vector<double> LoadsVector
        {
            get { return m_LoadsVector; }
        }

        public Vector<double> DisplacementsVector
        {
            get { return m_DisplacementsVector; }
        }

        public Vector<double> SupportReactionsVector
        {
            get { return m_SupportReactionsVector; }
            set { m_SupportReactionsVector = value; } // this should probably be removed with a refactoring
        }

        public void SetDataSize(int size)
        {
            m_GlobalStiffnessMatrix = new SparseMatrix(size, size);
            m_LoadsVector = new SparseVector(size);
        }

        public void Solve()
        {
            m_DisplacementsVector = m_GlobalStiffnessMatrix.Solve(m_LoadsVector);
        }
    }
}

