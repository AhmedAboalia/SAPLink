using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace SAPLink.Utilities
{
    public class TextOffset
    {
        public int X { get; set; }
        public int Y { get; set; }

        public TextOffset(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class CustomGunaButton : Guna2Button
    {
        private string descriptionText = "";
        private TextOffset descriptionTextOffset = new TextOffset(65, -10); // Default offset value
        private Font descriptionFont = new Font("Arial", 8.25F, FontStyle.Regular); // Default font

        public string TextDescription
        {
            get => descriptionText;
            set
            {
                descriptionText = value;
                Invalidate(); // Redraw the button to reflect changes
            }
        }

        public Font TextDescriptionFont
        {
            get => descriptionFont;
            set
            {
                descriptionFont = value;
                Invalidate(); // Redraw the button to reflect changes
            }
        }

        [Browsable(true)]
        public int TextDescriptionOffsetX
        {
            get => descriptionTextOffset.X;
            set
            {
                descriptionTextOffset.X = value;
                Invalidate(); // Redraw the button to reflect changes
            }
        }

        [Browsable(true)]
        public int TextDescriptionOffsetY
        {
            get => descriptionTextOffset.Y;
            set
            {
                descriptionTextOffset.Y = value;
                Invalidate(); // Redraw the button to reflect changes
            }
        }

        public TextOffset TextDescriptionOffset
        {
            get => descriptionTextOffset;
            set
            {
                descriptionTextOffset = value;
                Invalidate(); // Redraw the button to reflect changes
            }
        }

        public CustomGunaButton()
        {
            InitializeCustomButton();
        }

        private void InitializeCustomButton()
        {
            // Initialize your custom button properties here
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            DrawText(pevent);
        }

        private void DrawText(PaintEventArgs pevent)
        {
            using (StringFormat stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })
            {
                //// Display original text at the top
                //pevent.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor),
                //    ClientRectangle, stringFormat);

                // Display description text below the original text with margin and padding
                float originalTextHeight = pevent.Graphics.MeasureString(Text, Font).Height;
                float descriptionTextY = originalTextHeight + TextDescriptionOffset.Y; // Use the Y offset property
                float leftPadding = TextDescriptionOffset.X; // Use the X offset property

                pevent.Graphics.DrawString(
                    TextDescription,
                    TextDescriptionFont, // Use the separate font for description text
                    new SolidBrush(ForeColor),
                    leftPadding,
                    originalTextHeight + descriptionTextY,
                    stringFormat
                );
            }
        }
    }
}
