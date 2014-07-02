using System;
using GreenEngine.Model;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace GreenEngine.ElementMatrices
{
    class TrussElementMatrix2d : ElementMatrix
    {
        protected int m_NodeId1 = -1;
        protected int m_NodeId2 = -1;
        protected int m_ElementId = -1;

        protected double m_E = 0.0;
        protected double m_L = 0.0;
        protected double m_S = 0.0;
        protected double m_C = 0.0;
        
        public TrussElementMatrix2d(TrussElement element) :
            base()
        {
            m_DegreesOfFreedom = 4;
            m_Matrix = new double[m_DegreesOfFreedom, m_DegreesOfFreedom];

            m_NodeId1 = element.Node1.NodeId;
            m_NodeId2 = element.Node2.NodeId;
            m_ElementId = element.ElementId;

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

            m_E = e;
            m_L = l;
            m_S = s;
            m_C = c;
        }

        public double GetStress(double q1, double q2, double q3, double q4)
        {
            double dRet = 0.0;

            dRet = (m_E / m_L) * ((-m_C * q1) + (-m_S * q2) + (m_C * q3) + (m_S * q4));

            return dRet;
        }

        public int NodeId1
        {
            get { return m_NodeId1; }
        }

        public int NodeId2
        {
            get { return m_NodeId2; }
        }

        public int ElementId
        {
            get { return m_ElementId; }
        }

        public override IEnumerable<Tuple<int, DegreeType>> GetDegreesOfFreedom()
        {
            List<Tuple<int, DegreeType>> degreesOfFreedomList = new List<Tuple<int, DegreeType>>();

            degreesOfFreedomList.Add(new Tuple<int, DegreeType>(m_NodeId1, DegreeType.X));
            degreesOfFreedomList.Add(new Tuple<int, DegreeType>(m_NodeId1, DegreeType.Y));
            degreesOfFreedomList.Add(new Tuple<int, DegreeType>(m_NodeId2, DegreeType.X));
            degreesOfFreedomList.Add(new Tuple<int, DegreeType>(m_NodeId2, DegreeType.Y));

            return degreesOfFreedomList;
        }

        public override void CopyToGlobal(Matrix<double> globalMatrix, IDictionary<Tuple<int, DegreeType>, int> globalIndexDictionary)
        {
            Tuple<int, DegreeType> x1Tuple = new Tuple<int, DegreeType>(m_NodeId1, DegreeType.X);
            Tuple<int, DegreeType> y1Tuple = new Tuple<int, DegreeType>(m_NodeId1, DegreeType.Y);
            Tuple<int, DegreeType> x2Tuple = new Tuple<int, DegreeType>(m_NodeId2, DegreeType.X);
            Tuple<int, DegreeType> y2Tuple = new Tuple<int, DegreeType>(m_NodeId2, DegreeType.Y);

            int x1Index = globalIndexDictionary[x1Tuple];
            int y1Index = globalIndexDictionary[y1Tuple];
            int x2Index = globalIndexDictionary[x2Tuple];
            int y2Index = globalIndexDictionary[y2Tuple];

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
    }
}

