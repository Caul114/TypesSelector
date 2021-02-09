using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypesSelector
{
    public class ColorsDrawing
    {
        public static List<string> GetColors()
        {
            string[] listColors = Enum.GetNames(typeof(KnownColor));
            List<string> listColorsTrimmed = new List<string>();

            bool copy = false;
            bool esc = true;
            int count = 1;

            while(esc)
            {
                foreach (var color in listColors)
                {
                    if (color == "Beige")
                    {
                        copy = true;
                    }

                    if (color == "White")
                    {
                        copy = false;
                    }

                    if (color == "FloralWhite"
                       || color == "GhostWhite"
                       || color == "Honeydew"
                       || color == "Ivory"
                       || color == "LavenderBlush"
                       || color == "MintCream"
                       || color == "SeaShell"
                       || color == "Snow"
                       )
                    {
                    }
                    else if (copy)
                    {
                        listColorsTrimmed.Add(color);
                    }

                    if(count == 20)
                    {
                        esc = false;
                    }
                }
                count++;
            }

            return listColorsTrimmed;
        }
    }
}
