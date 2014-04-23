using System;
using System.Collections.Generic;
using GreenEngine.Model;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;


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

            // Determine degrees
			// for now just do X and Y for truss * nodes
            foreach (Node node in fem.NodeList)
            {
                degreeSet.Add(Tuple.Create<int, DegreeType>(node.NodeId, DegreeType.X));
                degreeSet.Add(Tuple.Create<int, DegreeType>(node.NodeId, DegreeType.Y));
            }

            //Matrix<double> m;
            //m.Solve(m);

		}


	}
}

