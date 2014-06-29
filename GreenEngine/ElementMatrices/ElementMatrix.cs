using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace GreenEngine.ElementMatrices
{
    abstract class ElementMatrix
    {
        protected int m_DegreesOfFreedom = 0;

        public ElementMatrix()
        {

        }

        public int DegreesOfFreedom
        {
            get { return m_DegreesOfFreedom; }
        }

        public abstract void CopyDegreesOfFreedomToSet(SortedSet<Tuple<int, DegreeType>> degreeSet);

        public abstract void CopyToGlobal(Matrix<double> globalMatrix, List<Tuple<int, DegreeType>> elementDegreeSolveList);
    }
}
