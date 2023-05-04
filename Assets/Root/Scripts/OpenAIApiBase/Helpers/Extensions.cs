// Extensions.cs

using System;
using YagizAyer.Root.Scripts.OpenAIApiBase.Presets;

namespace YagizAyer.Root.Scripts.OpenAIApiBase.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Converts the CompletionEngines enum to the string that the OpenAI API expects.
        /// </summary>
        /// <param name="name"> The CompletionEngines enum to convert. </param>
        /// <returns> The string that the OpenAI API expects. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> Thrown when the name parameter is not a valid CompletionEngines enum. </exception>
        public static string ToEngineString(this CompletionEngines name) =>
            name switch
            {
                CompletionEngines.Ada => "ada",
                CompletionEngines.Babbage => "babbage",
                CompletionEngines.ContentFilterAlphaC4 => "content-filter-alpha-c4",
                CompletionEngines.ContentFilterDev => "content-filter-dev",
                CompletionEngines.Curie => "curie",
                CompletionEngines.CursingFilterV6 => "cursing-filter-v6",
                CompletionEngines.Davinci => "text-davinci-003",
                CompletionEngines.InstructCurieBeta => "curie-instruct-beta",
                CompletionEngines.InstructDavinciBeta => "davinci-instruct-beta",
                _ => throw new ArgumentOutOfRangeException(nameof(name), name, null)
            };

        /// <summary>
        ///   Converts the AudioEngines enum to the string that the OpenAI API expects.
        /// </summary>
        /// <param name="name"> The AudioEngines enum to convert. </param>
        /// <returns> The string that the OpenAI API expects. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> Thrown when the name parameter is not a valid AudioEngines enum. </exception>
        public static string ToEngineString(this AudioEngines name) =>
            name switch
            {
                AudioEngines.Whisper1 => "whisper-1",
                _ => throw new ArgumentOutOfRangeException(nameof(name), name, null)
            };

        /// <summary>
        /// Converts the ContentType enum to the string that the web request expects.
        /// </summary>
        /// <param name="name"> The ContentType enum to convert. </param>
        /// <returns> The string that the web request expects. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> Thrown when the name parameter is not a valid ContentType enum. </exception>
        public static string ToContentString(this RequestPreset.ContentType name) =>
            name switch
            {
                RequestPreset.ContentType.ApplicationJson => "application/json",
                RequestPreset.ContentType.TextPlain => "text/plain",
                RequestPreset.ContentType.MultipartFormData => "multipart/form-data",
                RequestPreset.ContentType.ApplicationXWwwFormUrlencoded => "application/x-www-form-urlencoded",
                _ => throw new ArgumentOutOfRangeException(nameof(name), name, null)
            };
    }
}