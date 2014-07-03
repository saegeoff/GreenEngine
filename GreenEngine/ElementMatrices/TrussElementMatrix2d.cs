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
            m_Matrix [0, 1] = m_Matrix [2, 3] = ael * cs;
            m_Matrix [0, 2] = m_Matrix [2, 0] = ael * -c2;
            m_Matrix [0, 3] = m_Matrix [2, 1] = ael * -cs;

            m_Matrix [1, 0] = m_Matrix [3, 2] = ael * cs;
            m_Matrix [1, 1] = m_Matrix [3, 3] = ael * s2;
            m_Matrix [1, 2] = m_Matrix [3, 0] = ael * -cs;
            m_Matrix [1, 3] = m_Matrix [3, 1] = ael * -s2;

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

        protected override void CopyToMatrix(Matrix<double> globalMatrix, IDictionary<Tuple<int, DegreeType>, int> rowDictionary, IDictionary<Tuple<int, DegreeType>, int> colDictionary)
        {
            Tuple<int, DegreeType> x1Tuple = new Tuple<int, DegreeType>(m_NodeId1, DegreeType.X);
            Tuple<int, DegreeType> y1Tuple = new Tuple<int, DegreeType>(m_NodeId1, DegreeType.Y);
            Tuple<int, DegreeType> x2Tuple = new Tuple<int, DegreeType>(m_NodeId2, DegreeType.X);
            Tuple<int, DegreeType> y2Tuple = new Tuple<int, DegreeType>(m_NodeId2, DegreeType.Y);

            int x1RowIndex = rowDictionary[x1Tuple];
            int x1ColIndex = colDictionary[x1Tuple];

            int y1RowIndex = rowDictionary[y1Tuple];
            int y1ColIndex = colDictionary[y1Tuple];

            int x2RowIndex = rowDictionary[x2Tuple];
            int x2ColIndex = colDictionary[x2Tuple];

            int y2RowIndex = rowDictionary[y2Tuple];
            int y2ColIndex = colDictionary[y2Tuple];

            AddToGlobalMatrix(0, 0, x1RowIndex, x1ColIndex, globalMatrix);
            AddToGlobalMatrix(0, 1, x1RowIndex, y1ColIndex, globalMatrix);
            AddToGlobalMatrix(0, 2, x1RowIndex, x2ColIndex, globalMatrix);
            AddToGlobalMatrix(0, 3, x1RowIndex, y2ColIndex, globalMatrix);

            AddToGlobalMatrix(1, 0, y1RowIndex, x1ColIndex, globalMatrix);
            AddToGlobalMatrix(1, 1, y1RowIndex, y1ColIndex, globalMatrix);
            AddToGlobalMatrix(1, 2, y1RowIndex, x2ColIndex, globalMatrix);
            AddToGlobalMatrix(1, 3, y1RowIndex, y2ColIndex, globalMatrix);

            AddToGlobalMatrix(2, 0, x2RowIndex, x1ColIndex, globalMatrix);
            AddToGlobalMatrix(2, 1, x2RowIndex, y1ColIndex, globalMatrix);
            AddToGlobalMatrix(2, 2, x2RowIndex, x2ColIndex, globalMatrix);
            AddToGlobalMatrix(2, 3, x2RowIndex, y2ColIndex, globalMatrix);

            AddToGlobalMatrix(3, 0, y2RowIndex, x1ColIndex, globalMatrix);
            AddToGlobalMatrix(3, 1, y2RowIndex, y1ColIndex, globalMatrix);
            AddToGlobalMatrix(3, 2, y2RowIndex, x2ColIndex, globalMatrix);
            AddToGlobalMatrix(3, 3, y2RowIndex, y2ColIndex, globalMatrix);
        }
    }
}

