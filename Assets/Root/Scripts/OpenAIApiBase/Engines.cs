using System;

namespace YagizAyer.Root.Scripts.OpenAIApiBase
{
    public enum Engines
    {
        Ada,
        Babbage,
        ContentFilterAlphaC4,
        ContentFilterDev,
        Curie,
        CursingFilterV6,
        Davinci,
        InstructCurieBeta,
        InstructDavinciBeta
    }

    public static class EnginesExtensions
    {
        public static string ToEngineString(this Engines name) =>
            name switch
            {
                Engines.Ada => "ada",
                Engines.Babbage => "babbage",
                Engines.ContentFilterAlphaC4 => "content-filter-alpha-c4",
                Engines.ContentFilterDev => "content-filter-dev",
                Engines.Curie => "curie",
                Engines.CursingFilterV6 => "cursing-filter-v6",
                Engines.Davinci => "text-davinci-003",
                Engines.InstructCurieBeta => "curie-instruct-beta",
                Engines.InstructDavinciBeta => "davinci-instruct-beta",
                _ => throw new ArgumentOutOfRangeException(nameof(name), name, null)
            };
    }
}