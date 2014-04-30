using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenEngine.Model
{
    public class Support
    {
        public enum TranslationType
        {
            Constrained,
            Released
        }

        public enum RotationType
        {
            Constrained,
            Released
        }
        
        public Support()
		{
            Tx = TranslationType.Released;
            Ty = TranslationType.Released;
            Tz = TranslationType.Released;

            Rx = RotationType.Released;
            Ry = RotationType.Released;
            Rz = RotationType.Released;
		}

        public Node Node { get; set; }

        public TranslationType Tx { get; set; }
        public TranslationType Ty { get; set; }
        public TranslationType Tz { get; set; }

        public RotationType Rx { get; set; }
        public RotationType Ry { get; set; }
        public RotationType Rz { get; set; }
    }
}
