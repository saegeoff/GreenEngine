using System;
using GreenEngine.Model;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace GreenEngine.ElementMatrices
{
    class TrussElementMatrix2d : ElementMatrix
    {
        protected double[,] m_Matrix = new double[4, 4];
        protected int m_NodeId1 = -1;
        protected int m_NodeId2 = -1;
        
        public TrussElementMatrix2d(TrussElement element) :
            base()
        {
            m_DegreesOfFreedom = 4;

            m_NodeId1 = element.Node1.NodeId;
            m_NodeId2 = element.Node2.NodeId;

            double a = element.Area;
            double e = element.Material.ElasticModulus;
            double dX = element.Node2.X - element.Node1.X;
            double dY = element.Node2.Y - element.Node1.Y;
            double l = Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2));

            double ael = (a * e) / l;

            double angle = Math.Atan2(dY, dX);

            double c = Math.Cos(angle);
            double c2 = c * c;
            double s = Math.Sin(angle);
            double s2 = s * s;
            double cs = c * s;

            m_Matrix [0, 0] = m_Matrix [2, 2] = ael * c2;
            m_Matrix [1, 0] = m_Matrix [3, 2] = ael * cs;
            m_Matrix [0, 1] = m_Matrix [2, 3] = ael * cs;
            m_Matrix [1, 1] = m_Matrix [3, 3] = ael * s2;

            m_Matrix [2, 0] = m_Matrix [0, 2] = ael * -c2;
            m_Matrix [3, 0] = m_Matrix [1, 2] = ael * -cs;
            m_Matrix [2, 1] = m_Matrix [0, 3] = ael * -cs;
            m_Matrix [3, 1] = m_Matrix [1, 3] = ael * -s2;
        }

        public override SortedSet<Tuple<int, DegreeType>> GetDegreesOfFreedomSet()
        {
            SortedSet<Tuple<int, DegreeType>> degreeSet = new SortedSet<Tuple<int, DegreeType>>();

            degreeSet.Add(new Tuple<int, DegreeType>(m_NodeId1, DegreeType.X));
            degreeSet.Add(new Tuple<int, DegreeType>(m_NodeId1, DegreeType.Y));
            degreeSet.Add(new Tuple<int, DegreeType>(m_NodeId2, DegreeType.X));
            degreeSet.Add(new Tuple<int, DegreeType>(m_NodeId2, DegreeType.Y));

            return degreeSet;
        }

        public override void CopyToGlobal(Matrix<double> globalMatrix, SortedSet<Tuple<int, DegreeType>> globalDofSet)
        {
            bool n1x = globalDofSet.Contains(new Tuple<int, DegreeType>(m_NodeId1, DegreeType.X));
            bool n1y = globalDofSet.Contains(new Tuple<int, DegreeType>(m_NodeId1, DegreeType.Y));
            bool n2x = globalDofSet.Contains(new Tuple<int, DegreeType>(m_NodeId2, DegreeType.X));
            bool n2y = globalDofSet.Contains(new Tuple<int, DegreeType>(m_NodeId2, DegreeType.Y));


        }
    }
}

