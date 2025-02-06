using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace ImageMeasuringWPF
{
    public static class DXF_Out
    {
        private const string section = "0\r\nSECTION\r\n  2\r\n";
        private const string entities = "ENTITIES\r\n";
        private const string dxfLine = "0\r\nLINE\r\n";
        private const string dxfCircle = "0\r\nCIRCLE\r\n";
        private const string endSec = "0\r\nENDSEC\r\n";
        private const string eof = "0\r\nEOF";

        public static string Build_DXF(SerializeElements elements)
        {
            string dxfString = section;
            dxfString += entities;
            if(elements.SaveLines != null)
            {
                foreach (SaveLine ln in elements.SaveLines)
                {
                    dxfString += dxfLine;
                    dxfString += "10\r\n";
                    dxfString += $"{ln.X1 * elements.ScaleFactor}\r\n";
                    dxfString += "20\r\n";
                    dxfString += $"{-ln.Y1 * elements.ScaleFactor}\r\n";
                    dxfString += "11\r\n";
                    dxfString += $"{ln.X2 * elements.ScaleFactor}\r\n";
                    dxfString += "21\r\n";
                    dxfString += $"{-ln.Y2 * elements.ScaleFactor}\r\n";
                }
            }
            if (elements.SaveCircles != null)
            {
                foreach(SaveCircle cir in elements.SaveCircles)
                {
                    dxfString += dxfCircle;
                    dxfString += "10\r\n";
                    dxfString += $"{cir.CenterX * elements.ScaleFactor}\r\n";
                    dxfString += "20\r\n";
                    dxfString += $"{-cir.CenterY * elements.ScaleFactor}\r\n";
                    dxfString += "40\r\n";
                    dxfString += $"{.5 *cir.Diameter * elements.ScaleFactor}\r\n";
                }
            }
            dxfString += endSec;
            dxfString += eof;
            return dxfString;
        }

    }
}
