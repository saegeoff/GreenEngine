using System;

namespace GreenEngine.Results
{
    public class AnalysisResults
    {
        protected NodalDisplacementCollection m_NodalDisplacementCollection = new NodalDisplacementCollection();
        protected ElementStressCollection m_ElementStressCollection = new ElementStressCollection();
        protected SupportReactionCollection m_SupportReactionCollection = new SupportReactionCollection();

        public AnalysisResults()
        {
        }

        public NodalDisplacementCollection NodalDisplacements
        {
            get { return m_NodalDisplacementCollection; }
        }

        public ElementStressCollection ElementStresses
        {
            get { return m_ElementStressCollection; }
        }

        public SupportReactionCollection SupportReactions
        {
            get { return m_SupportReactionCollection; }
        }
    }
}

