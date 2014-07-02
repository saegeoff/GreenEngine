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
    
        protected abstract void CopyToMatrix(Matrix<double> matrix, IDictionary<Tuple<int, DegreeType>, int> xDictionary, IDictionary<Tuple<int, DegreeType>, int> yDictionary);

        protected void AddToGlobalMatrix(int lX, int lY, int gX, int gY, Matrix<double> globalMatrix)
        {
            if (gX < 0 || gY < 0)
                return;

            globalMatrix [gX, gY] += m_Matrix [lX, lY]; 
        }
    }
}
