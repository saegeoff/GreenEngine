/*******************************************************
 * Copyright (C) 2016 Geoffrey Trees
 * 
 * This file is part of GreenEngine.
 * 
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 *******************************************************/

using System;
using System.Collections.Generic;
using GreenEngine.Model;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;


namespace GreenEngine
{
    public class TestEngine
	{
		public TestEngine ()
		{
		}

		public void Analyze(FiniteElementModel fem)
		{
			HashSet<Tuple<int, DegreeType>> degreeSet = new HashSet<Tuple<int, DegreeType>>();
            List<Tuple<int, DegreeType>> degreeListSolve = new List<Tuple<int, DegreeType>>();

            // Determine degrees
			// for now just do X and Y for truss * nodes
            foreach (Node node in fem.Nodes)
            {
                degreeSet.Add(Tuple.Create<int, DegreeType>(node.NodeId, DegreeType.Fx));
                degreeSet.Add(Tuple.Create<int, DegreeType>(node.NodeId, DegreeType.Fy));
                degreeListSolve.Add(Tuple.Create<int, DegreeType>(node.NodeId, DegreeType.Fx));
                degreeListSolve.Add(Tuple.Create<int, DegreeType>(node.NodeId, DegreeType.Fy));
            }

            foreach (Support support in fem.Supports)
            {
                if (support.Tx == TranslationType.Constrained)
                    degreeListSolve.Remove(Tuple.Create<int, DegreeType>(support.Node.NodeId, DegreeType.Fx));

                if (support.Ty == TranslationType.Constrained)
                    degreeListSolve.Remove(Tuple.Create<int, DegreeType>(support.Node.NodeId, DegreeType.Fy));
            }
           
            Matrix<double> m = new SparseMatrix(degreeListSolve.Count, degreeListSolve.Count);

           
            foreach (Element element in fem.Elements)
            {
                if (element is TrussElement)
                {
                    GetTrussMatrix((TrussElement)element, m, degreeListSolve);
                }
            }

            Vector<double> loads = new SparseVector(degreeListSolve.Count);

            foreach (Load load in fem.Loads)
            {
                if (load is ConcentratedNodalLoad)
                {
                    ConcentratedNodalLoad conLoad = (ConcentratedNodalLoad)load;

                    int x1Index = degreeListSolve.FindIndex(x => x.Item1 == conLoad.Node.NodeId && x.Item2 == DegreeType.Fx);
                    int y1Index = degreeListSolve.FindIndex(x => x.Item1 == conLoad.Node.NodeId && x.Item2 == DegreeType.Fy);

                    if (x1Index >= 0)
                        loads[x1Index] += conLoad.X;

                    if (y1Index >= 0)
                        loads[y1Index] += conLoad.Y;
                }
            }

            Console.WriteLine(m.ToString());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(loads.ToString());
            Console.WriteLine();
            Console.WriteLine();

            Vector<double> displacements = m.Solve(loads);

            Console.WriteLine(displacements.ToString());
        }

        protected void GetTrussMatrix(TrussElement element, Matrix<double> m, List<Tuple<int, DegreeType>> list)
        {
            double A = element.Area;
            double E = element.Material.ElasticModulus;
            double dX = element.Node2.X - element.Node1.X;
            double dY = element.Node2.Y - element.Node1.Y;
            double L = Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2));

            double AEL = (A * E) / L;

            double angle = Math.Atan2(dY, dX);

            double c = Math.Cos(angle);
            double c2 = c * c;
            double s = Math.Sin(angle);
            double s2 = s * s;
            double sc = s * c;

            int x1Index = list.FindIndex(x => x.Item1 == element.Node1.NodeId && x.Item2 == DegreeType.Fx);
            int y1Index = list.FindIndex(x => x.Item1 == element.Node1.NodeId && x.Item2 == DegreeType.Fy);
            int x2Index = list.FindIndex(x => x.Item1 == element.Node2.NodeId && x.Item2 == DegreeType.Fx);
            int y2Index = list.FindIndex(x => x.Item1 == element.Node2.NodeId && x.Item2 == DegreeType.Fy);


            // Row 1
            if (x1Index >= 0)
            {
                if (x1Index >= 0)
                    m[x1Index, x1Index] += AEL * c2;

                if (y1Index >= 0)
                    m[x1Index, y1Index] += AEL * sc;

                if (x2Index >= 0)
                    m[x1Index, x2Index] += AEL * -c2;

                if (y2Index >= 0)
                    m[x1Index, y2Index] += AEL * -sc;
            }

            // Row 2
            if (y1Index >= 0)
            {
                if (x1Index >= 0)
                    m[y1Index, x1Index] += AEL * sc;
                
                if (y1Index >= 0)
                    m[y1Index, y1Index] += AEL * s2;
                
                if (x2Index >= 0)
                    m[y1Index, x2Index] += AEL * -sc;
                
                if (y2Index >= 0)
                    m[y1Index, y2Index] += AEL * -s2;
            }

            // Row 3
            if (x2Index >= 0)
            {
                if (x1Index >= 0)
                    m[x2Index, x1Index] += AEL * -c2;
                
                if (y1Index >= 0)
                    m[x2Index, y1Index] += AEL * -sc;
                
                if (x2Index >= 0)
                    m[x2Index, x2Index] += AEL * c2;
                
                if (y2Index >= 0)
                    m[x2Index, y2Index] += AEL * sc;
            }

            // Row 4
            if (y2Index >= 0)
            {
                if (x1Index >= 0)
                    m[y2Index, x1Index] += AEL * -sc;
                
                if (y1Index >= 0)
                    m[y2Index, y1Index] += AEL * -s2;
                
                if (x2Index >= 0)
                    m[y2Index, x2Index] += AEL * sc;
                
                if (y2Index >= 0)
                    m[y2Index, y2Index] += AEL * s2;
            }
        }
	}
}

