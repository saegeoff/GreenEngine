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

        public void CopyToGlobalMatrix(Matrix<double> globalMatrix, IDictionary<Tuple<int, DegreeType>, int> globalIndexDictionary)
        {
            CopyToMatrix(globalMatrix, globalIndexDictionary, globalIndexDictionary);
        }

        public void CopyToSupportMatrix(Matrix<double> supportMatrix, IDictionary<Tuple<int, DegreeType>, int> allGlobalIndexDictionary, IDictionary<Tuple<int, DegreeType>, int> supportGlobalIndexDictionary)
        {
            CopyToMatrix(supportMatrix, allGlobalIndexDictionary, supportGlobalIndexDictionary);
        }
    
        protected abstract void CopyToMatrix(Matrix<double> matrix, IDictionary<Tuple<int, DegreeType>, int> rowDictionary, IDictionary<Tuple<int, DegreeType>, int> colDictionary);

        protected void AddToGlobalMatrix(int lRow, int lCol, int gRow, int gCol, Matrix<double> globalMatrix)
        {
            if (gRow < 0 || gCol < 0)
                return;

            globalMatrix [gRow, gCol] += m_Matrix [lRow, lCol]; 
        }
    }
}
