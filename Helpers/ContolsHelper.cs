using ComponentFactory.Krypton.Toolkit;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UnifiedEducationManagementSystem.Helpers
{
    public static class ClearControls
    {
        private static readonly IEnumerable<Control> Controls;

        public static void Clear(Control container)
        {
            foreach (Control control in container.Controls)
            {
                switch (control)
                {
                    case TextBox textBox:
                        textBox.Clear();
                        break;
                    case ComboBox comboBox:
                        comboBox.SelectedIndex = -1; 
                        break;
                    case CheckBox checkBox:
                        checkBox.Checked = false; 
                        break;
                    case ListBox listBox:
                        listBox.SelectedIndex = -1;
                        break;
                    case Button button:
                        button.Enabled = false;
                        break;
                    case DateTimePicker dateTimePicker:
                        dateTimePicker.Value = DateTime.Now; 
                        break;
                }

                if (control.HasChildren)
                {
                    Clear(control);
                }
            }
        }



        public static void EmptyControls(object sender, EventArgs e)
        {
            var emptyControls = new List<string>();

            foreach (Control control in Controls)
            {
                if (control is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
                {
                    emptyControls.Add(textBox.Name);
                }
                else if (control is ComboBox comboBox && comboBox.SelectedIndex == -1)
                {
                    emptyControls.Add(comboBox.Name);
                }
            }

            if (emptyControls.Any())
            {
                var message = "All Fields are Required to Fill up:\n" + string.Join("\n", emptyControls);
                MessageBox.Show(message, "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
