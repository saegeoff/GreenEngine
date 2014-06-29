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
        private AnalysisResults results;

        public LinearEngine2d()
		{
		}

        public AnalysisResults Results
        {
            get { return this.results; }
        }

		public void Analyze(FiniteElementModel fem)
		{
            this.results = new AnalysisResults();
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

            // Solve problem
            Vector<double> displacements = globalStiffnessMatrix.Solve(loads);


            // Populate Results
            foreach (Tuple<int, DegreeType> degree in elementDegreeSet)
            {
                NodalDisplacement displacement = new NodalDisplacement();
                displacement.NodeId = degree.Item1;
                displacement.Degree = degree.Item2;
                this.results.NodalDisplacements.Add(displacement);

                // we need to use a dictionary for this!
                int dispIndex = elementDegreeSolveList.FindIndex(x => x.Item1 == degree.Item1 && x.Item2 == degree.Item2);

                if (dispIndex >= 0)
                    displacement.Displacement = displacements[dispIndex];
                   
            }


            // debugging
            Console.WriteLine(globalStiffnessMatrix.ToString());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(loads.ToString());
            Console.WriteLine();
            Console.WriteLine();


        }
	}
}

