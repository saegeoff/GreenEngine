using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace GreenEngine.ElementMatrices
{
    abstract class ElementMatrix
    {
        protected int m_ElementId = -1;
        protected List<Tuple<int, DegreeType>> m_DegreesOfFreedomList = new List<Tuple<int, DegreeType>>();
        protected double[,] m_Matrix;

        public ElementMatrix(Element element)
        {
            m_ElementId = element.ElementId;
        }

        public int ElementId
        {
            get { return m_ElementId; }
        }

        public int DegreesOfFreedomCount
        {
            get { return m_DegreesOfFreedomList.Count; }
        }

        public IEnumerable<Tuple<int, DegreeType>> GetDegreesOfFreedomTupleList()
        {
            return m_DegreesOfFreedomList;
        }

        public void CopyToGlobalMatrix(Matrix<double> matrix, IDictionary<Tuple<int, DegreeType>, int> globalIndexDictionary)
        {
            CopyToMatrix(matrix, globalIndexDictionary, globalIndexDictionary);
        }

        public void CopyToSupportMatrix(Matrix<double> matrix, IDictionary<Tuple<int, DegreeType>, int> allGlobalIndexDictionary, IDictionary<Tuple<int, DegreeType>, int> supportGlobalIndexDictionary)
        {
            CopyToMatrix(matrix, allGlobalIndexDictionary, supportGlobalIndexDictionary);
        }
    
        protected void CopyToMatrix(Matrix<double> matrix, IDictionary<Tuple<int, DegreeType>, int> rowDictionary, IDictionary<Tuple<int, DegreeType>, int> colDictionary)
        {
            int[] matrixRowIndex = new int[m_DegreesOfFreedomList.Count];
            int[] matrixColIndex = new int[m_DegreesOfFreedomList.Count];

            for (int i = 0; i < m_DegreesOfFreedomList.Count; ++i)
            {
                Tuple<int, DegreeType> degreeTuple = m_DegreesOfFreedomList[i];

                matrixRowIndex[i] = rowDictionary[degreeTuple];
                matrixColIndex[i] = colDictionary[degreeTuple];
            }

            for (int iRow = 0; iRow < m_DegreesOfFreedomList.Count; ++iRow)
            {
                for (int iCol = 0; iCol < m_DegreesOfFreedomList.Count; ++iCol)
                {
                    AddToMatrix(matrix, iRow, iCol, matrixRowIndex[iRow], matrixColIndex[iCol]);
                }
            }
        }

        protected void AddToMatrix( Matrix<double> matrix, int lRow, int lCol, int gRow, int gCol)
        {
            if (gRow < 0 || gCol < 0)
                return;

            matrix[gRow, gCol] += m_Matrix[lRow, lCol]; 
        }
    }
}
