using static SAPLink.Utilities.Logger;

namespace SAPLink.Utilities
{
    public static class LabelExtensions
    {
        /// <summary>
        /// Logs a message to the label with a specified message type and duration.
        /// </summary>
        /// <param name="label">The label to display the message on.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="messageType">The type of the message (affects the label's color).</param>
        /// <param name="messageDuration">The duration for which the message should be displayed.</param>
        public static async void Log(this Label label, string message, MessageTypes messageType, MessageTime messageDuration,bool resetText = true)
        {
            var backColor = label.BackColor;
            var foreColor = label.ForeColor;
            if (string.IsNullOrEmpty(message))
                return;

            SetLabelMessage(label, message, messageType);
            await DisplayMessageForDuration(messageDuration);
            if (resetText)
                ResetLabel(label);
        }

        private static void SetLabelMessage(Label label, string message, MessageTypes messageType)
        {
            label.Text = message;
            label.ForeColor = messageType == MessageTypes.Error ? Color.AliceBlue : label.ForeColor;

            label.BackColor = messageType switch
            {
                MessageTypes.Info => Color.LightGreen,
                MessageTypes.Error => Color.DarkRed,
                MessageTypes.Warning => Color.Goldenrod,
                _ => label.BackColor
            };
        }

        private static async Task DisplayMessageForDuration(MessageTime messageDuration)
        {
            var delayDuration = messageDuration == MessageTime.Short ? 5000 : 2015000;
            await Task.Delay(delayDuration);
        }

        private static void ResetLabel(Label label)
        {
            label.Text = "Status:";
            label.BackColor = Color.Wheat;
        }
    }

    public static class Logger
    {
        /// <summary>
        /// Logs a message array to a RichTextBox.
        /// </summary>
        public static void Log(this RichTextBox textBox, string[] message, MessageTime messageTime)
        {
            if (message != null && message.Length > 0)
                textBox.Lines = message;

            //if (messageTime == MessageTime.Short)
            //    await Task.Delay(10000);
            //else
            //    await Task.Delay(60000);

            //tetxbox.Text = null;
        }

        /// <summary>
        /// Logs a message to a RichTextBox with specific message type and display duration.
        /// </summary>
        public static async Task LogAsync(this RichTextBox textBox, string message, MessageTypes messageType, MessageTime messageDuration)
        {
            SetRichTextBoxProperties(textBox, message, messageType);
            await DisplayMessageForDuration(messageDuration);
            ResetRichTextBox(textBox);
        }

        /// <summary>
        /// Logs a message array to a RichTextBox with specific message type and display duration.
        /// </summary>
        public static async Task LogAsync(this RichTextBox textBox, string[] message, MessageTypes messageType, MessageTime messageDuration)
        {
            textBox.Lines = message;
            SetRichTextBoxProperties(textBox, messageType);
            await DisplayMessageForDuration(messageDuration);
            ResetRichTextBox(textBox);
        }

        private static void SetRichTextBoxProperties(RichTextBox textBox, string message, MessageTypes messageType)
        {
            textBox.Text = message;
            SetRichTextBoxProperties(textBox, messageType);
        }

        private static void SetRichTextBoxProperties(RichTextBox textBox, MessageTypes messageType)
        {
            switch (messageType)
            {
                case MessageTypes.Error:
                    textBox.ForeColor = Color.AliceBlue;
                    textBox.BackColor = Color.DarkRed;
                    break;
                case MessageTypes.Info:
                    textBox.BackColor = Color.Green;
                    break;
                case MessageTypes.Warning:
                    textBox.BackColor = Color.Goldenrod;
                    break;
            }
        }

        private static async Task DisplayMessageForDuration(MessageTime messageDuration)
        {
            int delayDuration = messageDuration == MessageTime.Short ? 3000 : 5000;
            await Task.Delay(delayDuration);
        }

        private static void ResetRichTextBox(RichTextBox textBox)
        {
            textBox.Text = null;
            textBox.BackColor = Color.Linen;
        }
        public enum MessageTypes
        {
            Error,
            Info,
            Warning
        }
        public enum MessageTime
        {
            Short,
            Long
        }
    }
}
