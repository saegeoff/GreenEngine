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

        public abstract void CopyToGlobal(Matrix<double> globalMatrix, IDictionary<Tuple<int, DegreeType>, int> globalIndexDictionary);
    
        protected void AddToGlobalMatrix(int lX, int lY, int gX, int gY, Matrix<double> globalMatrix)
        {
            if (gX < 0 || gY < 0)
                return;

            globalMatrix [gX, gY] += m_Matrix [lX, lY]; 
        }
    }
}
