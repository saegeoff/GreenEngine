using System;
using GreenEngine.Model;

namespace GreenEngine.ElementMatrix
{
    class TrussElementMatrix : ElementMatrix
    {
        protected double m_A = 0.0;
        protected double m_E = 0.0;
        protected double m_L = 0.0;
        protected double m_AEL = 0.0;

        protected double m_c = 0.0;
        protected double m_s = 0.0;

        protected double[,] m_Matrix = new double[4, 4];
        
        public TrussElementMatrix(TrussElement element) :
            base()
        {
            m_DegreesOfFreedom = 4;

            ComputeAttributes(element);
        }

        protected void ComputeAttributes(TrussElement element)
        {
            m_A = element.Area;
            m_E = element.Material.ElasticModulus;
            double dX = element.Node2.X - element.Node1.X;
            double dY = element.Node2.Y - element.Node1.Y;
            m_L = Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2));

            m_AEL = (m_A * m_E) / m_L;

            double angle = Math.Atan2(dY, dX);

            double c = Math.Cos(angle);
            double c2 = c * c;
            double s = Math.Sin(angle);
            double s2 = s * s;
            double sc = s * c;


        }


    }
}

