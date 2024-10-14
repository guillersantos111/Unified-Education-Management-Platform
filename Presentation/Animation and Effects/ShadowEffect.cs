using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace UnifiedEducationManagementSystem.Presentation.Animation_and_Effects
{
    public class ShadowEffect
    {

        public static void ApplyShadow(System.Windows.Forms.Control control)
        {
            if (control == null) return;

            control.Paint += (sender, e) =>
            {
                Color shadowColor = Color.FromArgb(50, Color.Black);

                Rectangle shadowRect = new Rectangle(control.Location.X + 5, control.Location.Y + 5, control.Width, control.Height);

                using (SolidBrush brush = new SolidBrush(shadowColor))
                {
                    e.Graphics.FillRectangle(brush, shadowRect);
                }
            };

            control.Invalidate();
        }
    }
}
