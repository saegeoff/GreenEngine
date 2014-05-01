﻿using System;
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

        public virtual SortedSet<Tuple<int, DegreeType>> GetDegreesOfFreedomSet()
        {
            return new SortedSet<Tuple<int, DegreeType>>();
        }

        public virtual void CopyToGlobal(Matrix<double> globalMatrix, SortedSet<Tuple<int, DegreeType>> globalDofSet)
        {

        }
    }
}
