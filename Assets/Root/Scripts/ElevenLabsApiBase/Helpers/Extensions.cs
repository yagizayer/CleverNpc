// Extensions.cs

using System;

namespace YagizAyer.Root.Scripts.ElevenLabsApiBase.Helpers
{
    public static class Extensions
    {
        /// <summary>
        ///   Returns the voice ID of the given voice name.
        /// </summary>
        /// <param name="name"> The name of the voice. </param>
        /// <returns> The voice ID of the given voice name. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> name </exception>
        public static string ToVoiceID(this Voices name) =>
            name switch
            {
                Voices.Rachel => "21m00Tcm4TlvDq8ikWAM",
                Voices.Domi => "AZnzlk1XvdvUeBnXmlld",
                Voices.Bella => "EXAVITQu4vr4xnSDxMaL",
                Voices.Antoni => "ErXwobaYiN019PkySvjV",
                Voices.Elli => "MF3mGyEYCl7XYWbV9V6O",
                Voices.Josh => "TxGEqnHWrfWFTfGW9XjX",
                Voices.Arnold => "VR6AewLTigWG4xSOukaG",
                Voices.Adam => "pNInz6obpgDQGcFmaJgB",
                Voices.Sam => "yoZ06aMxZJJ28mfd3POQ",
                _ => throw new ArgumentOutOfRangeException(nameof(name), name, null)
            };
    }
}