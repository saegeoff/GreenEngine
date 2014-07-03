using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace GreenEngine.ElementMatrices
{
    abstract class ElementMatrix
    {
        protected int m_DegreesOfFreedom = 0;
        protected double[,] m_Matrix;

        public ElementMatrix()
        {

        }

        public int DegreesOfFreedom
        {
            get { return m_DegreesOfFreedom; }
        }

        public abstract IEnumerable<Tuple<int, DegreeType>> GetDegreesOfFreedom();

        public void CopyToGlobalMatrix(Matrix<double> matrix, IDictionary<Tuple<int, DegreeType>, int> globalIndexDictionary)
        {
            CopyToMatrix(matrix, globalIndexDictionary, globalIndexDictionary);
        }

        public void CopyToSupportMatrix(Matrix<double> matrix, IDictionary<Tuple<int, DegreeType>, int> allGlobalIndexDictionary, IDictionary<Tuple<int, DegreeType>, int> supportGlobalIndexDictionary)
        {
            CopyToMatrix(matrix, allGlobalIndexDictionary, supportGlobalIndexDictionary);
        }
    
        protected abstract void CopyToMatrix(Matrix<double> matrix, IDictionary<Tuple<int, DegreeType>, int> rowDictionary, IDictionary<Tuple<int, DegreeType>, int> colDictionary);

        protected void AddToMatrix( Matrix<double> matrix, int lRow, int lCol, int gRow, int gCol)
        {
            if (gRow < 0 || gCol < 0)
                return;

            matrix[gRow, gCol] += m_Matrix[lRow, lCol]; 
        }
    }
}
