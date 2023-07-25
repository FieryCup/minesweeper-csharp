using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MS
{
    internal class MenuRender : ToolStripProfessionalRenderer
    {
        public MenuRender() : base(new ColorTable()) { }
    }

    public class ColorTable : ProfessionalColorTable
    {
        public override Color MenuItemPressedGradientBegin => Color.FromArgb(255, 72, 72, 72);
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(255, 72, 72, 72);
        public override Color MenuItemBorder => Color.FromArgb(255, 72, 72, 72);
        public override Color MenuItemSelected => Color.DimGray;
        public override Color MenuStripGradientBegin => Color.FromArgb(255, 72, 72, 72);
        public override Color MenuStripGradientEnd => Color.FromArgb(255, 72, 72, 72);
        public override Color ToolStripDropDownBackground => Color.FromArgb(255, 72, 72, 72);
        public override Color ImageMarginGradientBegin => Color.FromArgb(255, 72, 72, 72);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(255, 72, 72, 72);
        public override Color ImageMarginGradientEnd => Color.FromArgb(255, 72, 72, 72);
    }
 }
