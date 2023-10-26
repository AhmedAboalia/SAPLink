using System.Text.Json;
using Guna.UI2.WinForms;
using SAPLink.Core.Utilities;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SAPLink.Utilities
{
    public static class Controls
    {
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
