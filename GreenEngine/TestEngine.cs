using System;
using System.Collections.Generic;
using GreenEngine.Model;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;


namespace GreenEngine
{
	// Move this once we are done... this is just for hacking
    public enum DegreeType
    {
        X,
        Y
    }

    public class TestEngine
	{
		public TestEngine ()
		{
		}

		public void Analyze(FiniteElementModel fem)
		{
			HashSet<Tuple<int, DegreeType>> degreeSet = new HashSet<Tuple<int, DegreeType>>();
            HashSet<Tuple<int, DegreeType>> degreeSetSolve = new HashSet<Tuple<int, DegreeType>>();

            // Determine degrees
			// for now just do X and Y for truss * nodes
            foreach (Node node in fem.NodeList)
            {
                degreeSet.Add(Tuple.Create<int, DegreeType>(node.NodeId, DegreeType.X));
                degreeSet.Add(Tuple.Create<int, DegreeType>(node.NodeId, DegreeType.Y));
                degreeSetSolve.Add(Tuple.Create<int, DegreeType>(node.NodeId, DegreeType.X));
                degreeSetSolve.Add(Tuple.Create<int, DegreeType>(node.NodeId, DegreeType.Y));
            }

            // This should be moved up.  I am just hacking stuff together to learn a procedure
            foreach (Node node in fem.NodeList)
            {
                if (node.Fx)
                    degreeSetSolve.Remove(Tuple.Create<int, DegreeType>(node.NodeId, DegreeType.X));

                if (node.Fy)
                    degreeSetSolve.Remove(Tuple.Create<int, DegreeType>(node.NodeId, DegreeType.Y));
            }
           
            Matrix<double> m = new SparseMatrix(degreeSetSolve.Count, degreeSetSolve.Count);
		
            foreach (Element element in fem.ElementList)
            {
                if (element is TrussElement)
                {
                    GetTrussMatrix(element as TrussElement, m, degreeSetSolve);
                }
            }
        }

        protected void GetTrussMatrix(TrussElement element, Matrix<double> m, HashSet<Tuple<int, DegreeType>> set)
        {
            double A = element.Area;
            double E = element.Material.ElasticModulus;
            double dX = element.Node2.X - element.Node1.X;
            double dY = element.Node2.Y - element.Node1.Y;
            double L = Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2));

            double AEL = (A * E) / L;

            double angle = Math.Atan2(dX, dY) * (180.0 / Math.PI);

            double c = Math.Cos(angle);
            double c2 = c * c;
            double s = Math.Sin(angle);
            double s2 = s * s;

            double sc = s * c;
        }
	}
}

