using System;
using System.Speech.Synthesis;

namespace ST10456305_ICETASK2
{
    // 1) Base Class: ShipCommand
    public class ShipCommand
    {
        public string InputText { get; set; } = string.Empty;
        public string AuthorizedUser { get; set; } = string.Empty;

        public virtual void ExecuteCommand()
        {
            Console.WriteLine("Processing command...");

            using (var synth = new SpeechSynthesizer())
            {
                synth.Rate = 0;
                synth.Speak("Command received.");
            }
        }
    }

    // 2) Child Class: VocalResponse
    public class VocalResponse : ShipCommand
    {
        // If the AlertPitch is null, the computer uses its default voice frequency.
        // If a value is provided, it adjusts the "urgency" of the response.
        public int? AlertPitch { get; set; }

        public override void ExecuteCommand()
        {
            using (var synth = new SpeechSynthesizer())
            {
                if (AlertPitch is null)
                {
                    Console.WriteLine("Mode: Standard Communication.");
                    synth.Rate = 0;
                    synth.Speak(InputText);
                    return;
                }

                synth.Rate = Math.Max(-10, Math.Min(10, AlertPitch.Value));
                Console.WriteLine($"Mode: Priority Alert (Pitch Level: {AlertPitch})");
                synth.Speak(InputText);
            }
        }
    }

    // 3) Main Program
    internal static class Program
    {
        private static void Main()
        {
            Console.Write("Enter your rank/name: ");
            var userName = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter your command: ");
            var commandText = Console.ReadLine() ?? string.Empty;

            var userPart = userName.Trim().Replace(" ", "").ToUpper();
            var commandPart = commandText.Trim().Replace(" ", "").ToUpper();
            var logCode = $"{userPart}-{commandPart}";

            Console.Write("Enter Alert Urgency (1 to 10) or leave blank for Standard Mode: ");
            var urgencyInput = Console.ReadLine();

            int? alertPitch = null;
            while (true)
            {
                if (string.IsNullOrWhiteSpace(urgencyInput))
                {
                    alertPitch = null;
                    break;
                }

                if (int.TryParse(urgencyInput, out var parsed) && parsed >= 1 && parsed <= 10)
                {
                    alertPitch = parsed;
                    break;
                }

                Console.Write("Invalid input. Enter a number 1 to 10, or leave blank: ");
                urgencyInput = Console.ReadLine();
            }

            Console.WriteLine($"LogCode: {logCode}");

            var response = new VocalResponse
            {
                AuthorizedUser = userName,
                InputText = commandText,
                AlertPitch = alertPitch
            };

            response.ExecuteCommand();
        }
    }
}

