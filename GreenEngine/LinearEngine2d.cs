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


            // Populate Results ---- Displacements
            Dictionary<int, NodalDisplacement> displacementDictionary = new Dictionary<int, NodalDisplacement>();
            foreach (Node node in fem.Nodes)
            {
                NodalDisplacement displacement = new NodalDisplacement();
                displacement.NodeId = node.NodeId;
                this.results.NodalDisplacements.Add(displacement);
                displacementDictionary.Add(node.NodeId, displacement);  
            }

            foreach (Tuple<int, DegreeType> degree in elementDegreeSet)
            {
                NodalDisplacement displacement = displacementDictionary[degree.Item1];
                DegreeType degreeType = degree.Item2;


                // we need to use a dictionary for this!
                int dispIndex = elementDegreeSolveList.FindIndex(x => x.Item1 == degree.Item1 && x.Item2 == degree.Item2);

                if (dispIndex >= 0)
                {
                    if (degreeType == DegreeType.X)
                        displacement.X = displacements[dispIndex];
                    else if (degreeType == DegreeType.Y)
                        displacement.Y = displacements[dispIndex];
                }
            }

            // Populate Results ---- Stresses
            Dictionary<int, ElementStress> stressDictionary = new Dictionary<int, ElementStress>();
            foreach (Element element in fem.Elements)
            {
                ElementStress stress = new ElementStress();
                stress.ElementId = element.ElementId;
                this.results.ElementStresses.Add(stress);
                stressDictionary.Add(element.ElementId, stress);  
            }

            foreach (ElementMatrix matrix in elementMatrixList)
            {
                if (matrix is TrussElementMatrix2d)
                {
                    TrussElementMatrix2d trussMatrix = (TrussElementMatrix2d)matrix;
                    ElementStress stress = stressDictionary [trussMatrix.ElementId];

                    int x1Index = elementDegreeSolveList.FindIndex(x => x.Item1 == trussMatrix.NodeId1 && x.Item2 == DegreeType.X);
                    int y1Index = elementDegreeSolveList.FindIndex(x => x.Item1 == trussMatrix.NodeId1 && x.Item2 == DegreeType.Y);
                    int x2Index = elementDegreeSolveList.FindIndex(x => x.Item1 == trussMatrix.NodeId2 && x.Item2 == DegreeType.X);
                    int y2Index = elementDegreeSolveList.FindIndex(x => x.Item1 == trussMatrix.NodeId2 && x.Item2 == DegreeType.Y);

                    double q1 = 0.0;
                    double q2 = 0.0;
                    double q3 = 0.0;
                    double q4 = 0.0;

                    if (x1Index >= 0)
                        q1 = displacements [x1Index];

                    if (y1Index >= 0)
                        q2 = displacements [y1Index];

                    if (x2Index >= 0)
                        q3 = displacements [x2Index];

                    if (y2Index >= 0)
                        q4 = displacements [y2Index];

                    stress.Stress = trussMatrix.GetStress(q1, q2, q3, q4);

                }
            }


            // Support Reactions
            int iNumSupportConstraints = 0;
            Dictionary<int, SupportReaction> supportReactionDictionary = new Dictionary<int, SupportReaction>();
            foreach (Support support in fem.Supports)
            {
                SupportReaction supportReaction = new SupportReaction();
                supportReaction.NodeId = support.Node.NodeId;
                this.results.SupportReactions.Add(supportReaction);
                supportReactionDictionary.Add(support.Node.NodeId, supportReaction);

                iNumSupportConstraints += support.GetNumberOfConstraints();
            }

            Matrix<double> supportStiffnessMatrix = new SparseMatrix(fem.Nodes.Count * 2 /* nodes * xy(2) */, iNumSupportConstraints);


            Vector<double> supportStiffnessDisplacements = new SparseVector(fem.Nodes.Count * 2 /* nodes * xy(2) */);

            for (int i = 0; i < fem.Nodes.Count; i++)
            {
                Node node = fem.Nodes [i];

                int xIndex = elementDegreeSolveList.FindIndex(x => x.Item1 == node.NodeId && x.Item2 == DegreeType.X);
                int yIndex = elementDegreeSolveList.FindIndex(x => x.Item1 == node.NodeId && x.Item2 == DegreeType.Y);

                if (xIndex >= 0)
                    supportStiffnessDisplacements [(i * 2)] = displacements [xIndex];
                else
                    supportStiffnessDisplacements [(i * 2)] = 0.0;

                if (yIndex >= 0)
                    supportStiffnessDisplacements [(i * 2) + 1] = displacements [yIndex];
                else
                    supportStiffnessDisplacements [(i * 2) + 1] = 0.0;
            }

//            SortedSet<Tuple<int, DegreeType>> elementSupportSolveSet = new SortedSet<Tuple<int, DegreeType>>(elementDegreeSet);
//
//
//            // Remove supports from degrees of freedom solve set
//            foreach (Support support in fem.Supports)
//            {
//                if (support.Tx == Support.TranslationType.Constrained)
//                    elementSupportSolveSet.Remove(new Tuple<int, DegreeType>(support.Node.NodeId, DegreeType.X));
//
//                if (support.Ty == Support.TranslationType.Constrained)
//                    elementSupportSolveSet.Remove(new Tuple<int, DegreeType>(support.Node.NodeId, DegreeType.Y));
//            }
//
//            List<Tuple<int, DegreeType>> elementSupportSolveList = new List<Tuple<int, DegreeType>>(elementDegreeSolveSet);

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

