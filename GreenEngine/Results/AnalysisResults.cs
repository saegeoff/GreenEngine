using System;

namespace GreenEngine.Results
{
    public class AnalysisResults
    {
        protected NodalDisplacementCollection m_NodalDisplacementCollection = new NodalDisplacementCollection();
        protected ElementActionCollection m_ElementStressCollection = new ElementActionCollection();
        protected SupportReactionCollection m_SupportReactionCollection = new SupportReactionCollection();

        public AnalysisResults()
        {
        }

        public NodalDisplacementCollection NodalDisplacements
        {
            get { return m_NodalDisplacementCollection; }
        }

        public ElementActionCollection ElementActions
        {
            get { return m_ElementStressCollection; }
        }

        public SupportReactionCollection SupportReactions
        {
            get { return m_SupportReactionCollection; }
        }
    }
}

