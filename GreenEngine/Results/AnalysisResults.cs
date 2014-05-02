using System;

namespace GreenEngine.Results
{
    public class AnalysisResults
    {
        protected NodalDisplacementCollection m_NodalDisplacementCollection = new NodalDisplacementCollection();

        public AnalysisResults()
        {
        }

        public NodalDisplacementCollection NodalDisplacements
        {
            get { return m_NodalDisplacementCollection; }
        }
    }
}

