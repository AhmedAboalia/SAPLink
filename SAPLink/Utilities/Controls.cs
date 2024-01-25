namespace SAPLink.Utilities
{
    public static class Controls
    {
        /// <summary>
        /// Brings the given control to the front, using Invoke if needed.
        /// </summary>
        /// <param name="control">The control to bring to the front.</param>
        public static void InvokeBringToFront(this Control control)
        {
            try
            {
                if (control.InvokeRequired)
                    control.Invoke(new MethodInvoker(control.BringToFront));
                else
                    control.BringToFront();
            }
            catch (Exception e)
            {
                //
            }
        }

        /// <summary>
        /// Uncheck the given Guna2ToggleSwitch control, using Invoke if needed.
        /// </summary>
        /// <param name="control">The control to uncheck.</param>
        public static void InvokeUnCheck(this Guna2ToggleSwitch control)
        {
            try
            {
                if (control.InvokeRequired)
                    control.Invoke(new MethodInvoker(control.UnCheck));
                else
                    control.UnCheck();
            }
            catch (Exception e)
            {
                //
            }
        }

        /// <summary>
        /// Unchecks the given Guna2ToggleSwitch control if it's checked.
        /// </summary>
        /// <param name="control">The control to uncheck.</param>
        public static void UnCheck(this Guna2ToggleSwitch control)
        {
            if (control.Checked)
                control.Checked = false;
        }

        /// <summary>
        /// Selects and scrolls to the last row of a Guna2DataGridView.
        /// </summary>
        /// <param name="dataGridView">The DataGridView on which the operation is performed.</param>
        public static void SelectLastRow(this Guna2DataGridView dataGridView)
        {
            if (dataGridView.Rows.Count > 0)
            {
                // Select the last row.
                dataGridView.ClearSelection();
                dataGridView.Rows[dataGridView.Rows.Count - 1].Selected = true;

                // Scroll to the last row.
                dataGridView.FirstDisplayedScrollingRowIndex = dataGridView.Rows.Count - 1;
            }
        }

        /// <summary>
        /// Checks if the numeric value in a Guna2TextBox is within a specified range.
        /// </summary>
        /// <param name="textBox">The TextBox containing the value to check.</param>
        /// <param name="min">The minimum value of the range.</param>
        /// <param name="max">The maximum value of the range.</param>
        /// <returns>True if the value is within the range, otherwise false.</returns>
        public static bool IsWithinRange(this Guna2TextBox textBox, int min, int max)
        {
            if (textBox.Text.IsDigit())
            {
                var number = int.Parse(textBox.Text);

                if (Enumerable.Range(min, max).Contains(number))
                    return true; //true
            }
            return false;


            //int val = 0;
            //bool res = Int32.TryParse(txtbox1.Text, out val);
            //if (res == true && val > 0 && val <= 60)
            //{
            //    // add record
            //}
            //else
            //{
            //    MessageBox.Show("Please input 0 to 100 only.");
            //    return;
            //}
        }

        /// <summary>
        /// Sets the text value of a control, using Invoke if required.
        /// </summary>
        /// <param name="control">The control to set the text value for.</param>
        /// <param name="input">The text value to set.</param>
        public static void InvokeValue(this Control control, string input)
        {
            try
            {
                if (control.InvokeRequired)
                    control.Invoke(new Action(() => { control.Text = input; }));
                else
                    control.Text = input;
            }
            catch (Exception e)
            {
                //
            }
        }

        /// <summary>
        /// Clears the image in a PictureBox, using Invoke if needed.
        /// </summary>
        /// <param name="control">The PictureBox control to clear.</param>
        public static void InvokeClear(this PictureBox control)
        {
            if (!control.InvokeRequired)
                control.Invoke(new Action(() => { control = null; }));
            //else
            //    control.Image = Properties.Resources.NoImage;
        }
        /// <summary>
        /// Adds an item with integer value to a ComboBox.
        /// </summary>
        /// <param name="combo">The ComboBox to which the item is added.</param>
        /// <param name="Value">The integer value of the item.</param>
        /// <param name="Text">The display text of the item.</param>
        public static void AddItem(this ComboBox combo, int Value, string Text)
        {
            if (Value.ToString().IsHasValue())
            {
                var item = new ComboboxItem();
                item.Text = Text;
                item.Value = Value;

                combo.Items.Add(item);

                //combo.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Adds an item with string value to a ComboBox.
        /// </summary>
        /// <param name="combo">The ComboBox to which the item is added.</param>
        /// <param name="Value">The string value of the item.</param>
        /// <param name="Text">The display text of the item.</param>
        public static void AddItem(this ComboBox combo, string Value, string Text)
        {
            if (Value.IsHasValue())
            {
                var item = new ComboboxItem();
                item.Text = Text;
                item.Value = Value;

                combo.Items.Add(item);

                //combo.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Represents an item in a ComboBox with a display text and an associated value.
        /// </summary>
        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}
