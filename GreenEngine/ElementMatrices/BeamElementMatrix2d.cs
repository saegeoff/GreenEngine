/*******************************************************
 * Copyright (C) 2014 Geoffrey Trees gptrees@gmail.com
 * 
 * This file is part of GreenEngine.
 * 
 * GreenEngine can not be copied and/or distributed without the express
 * permission of Geoffrey Trees
 *******************************************************/
using System;
using GreenEngine.Model;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace GreenEngine.ElementMatrices
{
    class BeamElementMatrix2d : ElementMatrix
    {
        protected int m_NodeId1 = -1;
        protected int m_NodeId2 = -1;

        public BeamElementMatrix2d(BeamElement element)
            : base(element)
        {
            m_NodeId1 = element.Node1.NodeId;
            m_NodeId2 = element.Node2.NodeId;

            m_DegreesOfFreedomList.Add(new Tuple<int, DegreeType>(m_NodeId1, DegreeType.Fy));
            m_DegreesOfFreedomList.Add(new Tuple<int, DegreeType>(m_NodeId1, DegreeType.Mz));
            m_DegreesOfFreedomList.Add(new Tuple<int, DegreeType>(m_NodeId2, DegreeType.Fy));
            m_DegreesOfFreedomList.Add(new Tuple<int, DegreeType>(m_NodeId2, DegreeType.Mz));

            m_Matrix = new double[m_DegreesOfFreedomList.Count, m_DegreesOfFreedomList.Count];

            double e = element.Material.ElasticModulus;
            double i = element.MomentOfInertia;
            double dX = element.Node2.X - element.Node1.X;
            double dY = element.Node2.Y - element.Node1.Y;
            double l = Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2));

            double eil3 = (e * i) / Math.Pow(l, 3);
           

            m_Matrix[0, 0] = 12.0 * eil3;
            m_Matrix[0, 1] = 6.0 * l * eil3;
            m_Matrix[0, 2] = -12.0 * eil3;
            m_Matrix[0, 3] = 6.0 * l * eil3;

            m_Matrix[1, 0] = 6.0 * l * eil3;
            m_Matrix[1, 1] = 4.0 * Math.Pow(l, 2) * eil3;
            m_Matrix[1, 2] = -6.0 * l * eil3;
            m_Matrix[1, 3] = 2.0 * Math.Pow(l, 2) * eil3;

            m_Matrix[2, 0] = -12.0 * eil3;
            m_Matrix[2, 1] = -6.0 * l * eil3;
            m_Matrix[2, 2] = 12.0 * eil3;
            m_Matrix[2, 3] = -6.0 * l * eil3;

            m_Matrix[3, 0] = 6.0 * l * eil3;
            m_Matrix[3, 1] = 2.0 * Math.Pow(l, 2) * eil3;
            m_Matrix[3, 2] = -6.0 * l * eil3;
            m_Matrix[3, 3] = 4.0 * Math.Pow(l, 2) * eil3;
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

