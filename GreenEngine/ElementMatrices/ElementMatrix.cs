using System;

namespace GreenEngine.ElementMatrix
{
    abstract class ElementMatrix
    {
        protected int m_DegreesOfFreedom = 0;

        public ElementMatrix()
        {

        }

        public int DegreesOfFreedom
        {
            get { return m_DegreesOfFreedom; }
        }
    }
}
