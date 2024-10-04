using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UnifiedEducationManagementSystem.Presentation.Animation
{
    public class AnimationCollapsed
    {
        private List<FlowLayoutPanel> containers;
        private bool collapsed;
        private System.Windows.Forms.Timer timer;
        private int stepSize;

        public AnimationCollapsed(int stepSize = 10, params FlowLayoutPanel[] containers)
        {
            this.containers = new List <FlowLayoutPanel>(containers);
            collapsed = true;
            this.stepSize = stepSize;
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            foreach (var container in containers)
            {
                if (collapsed)
                {
                    container.Height += stepSize;
                    if (container.Height >= container.MaximumSize.Height)
                    {
                        collapsed = false;
                        timer.Stop();
                    }
                }
                else
                {
                    container.Height -= stepSize;
                    if (container.Height <= container.MinimumSize.Height)
                    {
                        collapsed = true;
                        timer.Stop();
                    }

                }
            }
        }

        public void Reset()
        {
            timer.Stop();
            foreach (var container in containers)
            {
                container.Height = container.MinimumSize.Height;
            }
            collapsed = true;
        }

        public void Toggle()
        {
            timer.Start();
        }
    }
}
