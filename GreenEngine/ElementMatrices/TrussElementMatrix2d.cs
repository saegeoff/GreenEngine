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

        public override void CopyDegreesOfFreedomToSet(SortedSet<Tuple<int, DegreeType>> degreeSet)
        {
            degreeSet.Add(new Tuple<int, DegreeType>(m_NodeId1, DegreeType.X));
            degreeSet.Add(new Tuple<int, DegreeType>(m_NodeId1, DegreeType.Y));
            degreeSet.Add(new Tuple<int, DegreeType>(m_NodeId2, DegreeType.X));
            degreeSet.Add(new Tuple<int, DegreeType>(m_NodeId2, DegreeType.Y));
        }

        public override void CopyToGlobal(Matrix<double> globalMatrix, List<Tuple<int, DegreeType>> elementDegreeSolveList)
        {
            int x1Index = elementDegreeSolveList.FindIndex(x => x.Item1 == m_NodeId1 && x.Item2 == DegreeType.X);
            int y1Index = elementDegreeSolveList.FindIndex(x => x.Item1 == m_NodeId1 && x.Item2 == DegreeType.Y);
            int x2Index = elementDegreeSolveList.FindIndex(x => x.Item1 == m_NodeId2 && x.Item2 == DegreeType.X);
            int y2Index = elementDegreeSolveList.FindIndex(x => x.Item1 == m_NodeId2 && x.Item2 == DegreeType.Y);

            AddToGlobalMatrix(0, 0, x1Index, x1Index, globalMatrix);
            AddToGlobalMatrix(1, 0, x1Index, y1Index, globalMatrix);
            AddToGlobalMatrix(2, 0, x1Index, x2Index, globalMatrix);
            AddToGlobalMatrix(3, 0, x1Index, y2Index, globalMatrix);

            AddToGlobalMatrix(0, 1, y1Index, x1Index, globalMatrix);
            AddToGlobalMatrix(1, 1, y1Index, y1Index, globalMatrix);
            AddToGlobalMatrix(2, 1, y1Index, x2Index, globalMatrix);
            AddToGlobalMatrix(3, 1, y1Index, y2Index, globalMatrix);

            AddToGlobalMatrix(0, 2, x2Index, x1Index, globalMatrix);
            AddToGlobalMatrix(1, 2, x2Index, y1Index, globalMatrix);
            AddToGlobalMatrix(2, 2, x2Index, x2Index, globalMatrix);
            AddToGlobalMatrix(3, 2, x2Index, y2Index, globalMatrix);

            AddToGlobalMatrix(0, 3, y2Index, x1Index, globalMatrix);
            AddToGlobalMatrix(1, 3, y2Index, y1Index, globalMatrix);
            AddToGlobalMatrix(2, 3, y2Index, x2Index, globalMatrix);
            AddToGlobalMatrix(3, 3, y2Index, y2Index, globalMatrix);
        }

        protected void AddToGlobalMatrix(int lX, int lY, int gX, int gY, Matrix<double> globalMatrix)
        {
            if (gX < 0 || gY < 0)
                return;

            globalMatrix [gX, gY] += m_Matrix [lX, lY]; 
        }
    }
}

