using System;
using System.Collections.Generic;
using GreenEngine.Model;
using GreenEngine.ElementMatrices;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using GreenEngine.Results;


namespace GreenEngine
{
    public class LinearEngine2d
	{
		public LinearEngine2d()
		{
		}

		public void Analyze(FiniteElementModel fem)
		{
            List<ElementMatrix> elementMatrixList = new List<ElementMatrix>();
            SortedSet<Tuple<int, DegreeType>> elementDegreeSet = new SortedSet<Tuple<int, DegreeType>>();


            // Build local stiffness matrices
            foreach (Element element in fem.Elements)
            {
                ElementMatrix elementMatrix;

                if (element is TrussElement)
                {
                    elementMatrix = new TrussElementMatrix2d((TrussElement)element);
                }
                else
                {
                    System.Diagnostics.Debug.Assert(false);
                    continue;
                }

                elementMatrix.CopyDegreesOfFreedomToSet(elementDegreeSet);
                elementMatrixList.Add(elementMatrix);
            }

            SortedSet<Tuple<int, DegreeType>> elementDegreeSolveSet = new SortedSet<Tuple<int, DegreeType>>(elementDegreeSet);


            // Remove supports from degrees of freedom solve set
            foreach (Support support in fem.Supports)
            {
                if (support.Tx == Support.TranslationType.Constrained)
                    elementDegreeSolveSet.Remove(new Tuple<int, DegreeType>(support.Node.NodeId, DegreeType.X));

                if (support.Ty == Support.TranslationType.Constrained)
                    elementDegreeSolveSet.Remove(new Tuple<int, DegreeType>(support.Node.NodeId, DegreeType.Y));
            }

            List<Tuple<int, DegreeType>> elementDegreeSolveList = new List<Tuple<int, DegreeType>>(elementDegreeSolveSet);

            Matrix<double> globalStiffnessMatrix = new SparseMatrix(elementDegreeSolveSet.Count, elementDegreeSolveSet.Count);

            // Build Global Stiffness Matrix
            foreach (ElementMatrix matrix in elementMatrixList)
            {
                matrix.CopyToGlobal(globalStiffnessMatrix, elementDegreeSolveList);
            }

            Vector<double> loads = new SparseVector(elementDegreeSolveSet.Count);

            foreach (Load load in fem.Loads)
            {
                if (load is ConcentratedLoad)
                {
                    ConcentratedLoad conLoad = (ConcentratedLoad)load;

                    int x1Index = elementDegreeSolveList.FindIndex(x => x.Item1 == conLoad.Node.NodeId && x.Item2 == DegreeType.X);
                    int y1Index = elementDegreeSolveList.FindIndex(x => x.Item1 == conLoad.Node.NodeId && x.Item2 == DegreeType.Y);

                    if (x1Index >= 0)
                        loads[x1Index] += conLoad.X;

                    if (y1Index >= 0)
                        loads[y1Index] += conLoad.Y;
                }
            }

            AnalysisResults results = new AnalysisResults();

            foreach (Tuple<int, DegreeType> degree in elementDegreeSet)
            {
                NodalDisplacement displacement = new NodalDisplacement();
                displacement.NodeId = degree.Item1;
                displacement.Degree = degree.Item2;
                results.NodalDisplacements.Add(displacement);
            }

            Vector<double> displacements = globalStiffnessMatrix.Solve(loads);

            Console.WriteLine(globalStiffnessMatrix.ToString());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(loads.ToString());
            Console.WriteLine();
            Console.WriteLine();

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

            int x1Index = list.FindIndex(x => x.Item1 == element.Node1.NodeId && x.Item2 == DegreeType.X);
            int y1Index = list.FindIndex(x => x.Item1 == element.Node1.NodeId && x.Item2 == DegreeType.Y);
            int x2Index = list.FindIndex(x => x.Item1 == element.Node2.NodeId && x.Item2 == DegreeType.X);
            int y2Index = list.FindIndex(x => x.Item1 == element.Node2.NodeId && x.Item2 == DegreeType.Y);


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

