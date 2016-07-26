/*******************************************************
 * Copyright (C) 2016 Geoffrey Trees
 * 
 * This file is part of GreenEngine.
 * 
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 *******************************************************/
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

        protected double m_E = 0.0;
        protected double m_L = 0.0;
        protected double m_S = 0.0;
        protected double m_C = 0.0;
        
        public TrussElementMatrix2d(TrussElement element)
            : base(element)
        {
            m_NodeId1 = element.Node1.NodeId;
            m_NodeId2 = element.Node2.NodeId;

            m_DegreesOfFreedomList.Add(new Tuple<int, DegreeType>(m_NodeId1, DegreeType.Fx));
            m_DegreesOfFreedomList.Add(new Tuple<int, DegreeType>(m_NodeId1, DegreeType.Fy));
            m_DegreesOfFreedomList.Add(new Tuple<int, DegreeType>(m_NodeId2, DegreeType.Fx));
            m_DegreesOfFreedomList.Add(new Tuple<int, DegreeType>(m_NodeId2, DegreeType.Fy));

            m_Matrix = new double[m_DegreesOfFreedomList.Count, m_DegreesOfFreedomList.Count];

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
    }
}

