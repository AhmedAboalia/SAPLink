using System.Media;

namespace SAPLink.Utilities
{
    public static class PlaySound
    {
        /// <summary>
        /// Plays a click sound.
        /// </summary>
        public static void Click() => PlaySoundByName("click");

        /// <summary>
        /// Plays a keypress sound.
        /// </summary>
        public static void KeyPress() => PlaySoundByName("key");

        /// <summary>
        /// Plays a hover sound.
        /// </summary>
        public static void Hover() => PlaySoundByName("hover");

        /// <summary>
        /// Plays a sound by its file name without the file extension.
        /// </summary>
        /// <param name="soundName">The name of the sound file without the extension.</param>
        private static void PlaySoundByName(string soundName)
        {
            string filePath = GetSoundFilePath(soundName);

            if (File.Exists(filePath))
            {
                using (var player = new SoundPlayer(filePath))
                {
                    try
                    {
                        player.Play();
                    }
                    catch (Exception ex)
                    {
                        // Optionally, handle the exception or log it for diagnostics.
                    }
                }
            }
        }

        /// <summary>
        /// Gets the full file path of a sound by its name.
        /// </summary>
        /// <param name="soundName">The name of the sound file without the extension.</param>
        /// <returns>The full file path to the sound.</returns>
        private static string GetSoundFilePath(string soundName)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(baseDirectory, $"Resources\\Sounds\\{soundName}.wav");
        }

        //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Sounds\\{name}.wav");
        //new SoundPlayer(path)?.Play();
    }
}
