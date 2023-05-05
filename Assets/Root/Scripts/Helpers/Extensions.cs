// Extensions.cs

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;

namespace YagizAyer.Root.Scripts.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Clones the list.
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> Clone<T>(this IEnumerable<T> list) => new(list);

        /// <summary>
        ///  Returns the closest item to the transform.
        /// </summary>
        /// <param name="transform"> The transform to compare the distance to.</param>
        /// <param name="list"> The list to search for the closest item.</param>
        /// <typeparam name="T"> The type of the list.</typeparam>
        /// <returns> The closest item to the transform.</returns>
        public static T GetClosest<T>(this Transform transform, List<T> list) where T : Component
        {
            T closest = null;
            var closestDistance = Mathf.Infinity;
            foreach (var item in list)
            {
                var distance = Vector3.Distance(transform.position, item.transform.position);
                if (!(distance < closestDistance)) continue;
                closest = item;
                closestDistance = distance;
            }

            return closest;
        }

        /// <summary>
        /// Converts the value to IPassableData.
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IPassableData ToPassableData<T>(this T value) => new PassableDataBase<T>(value);

        /// <summary>
        ///  Logs the value of the IPassableData to the console.
        /// </summary>
        /// <param name="rawData">The IPassableData to log.</param>
        public static void ConsoleLog(this IPassableData rawData)
        {
            switch (rawData)
            {
                case PassableDataBase<int> intData:
                    Debug.Log(intData.Value);
                    break;
                case PassableDataBase<float> floatData:
                    Debug.Log(floatData.Value);
                    break;
                case PassableDataBase<string> stringData:
                    Debug.Log(stringData.Value);
                    break;
                case PassableDataBase<bool> boolData:
                    Debug.Log(boolData.Value);
                    break;
                case PassableDataBase<Vector2> vector2Data:
                    Debug.Log(vector2Data.Value);
                    break;
                case PassableDataBase<Vector3> vector3Data:
                    Debug.Log(vector3Data.Value);
                    break;
                case PassableDataBase<Vector4> vector4Data:
                    Debug.Log(vector4Data.Value);
                    break;
                case PassableDataBase<Quaternion> quaternionData:
                    Debug.Log(quaternionData.Value);
                    break;
                case PassableDataBase<Color> colorData:
                    Debug.Log(colorData.Value);
                    break;
                case PassableDataBase<object> objectData:
                    Debug.Log(objectData.Value);
                    break;
                default:
                    Debug.Log("No data");
                    break;
            }
        }

        /// <summary>
        /// Validates given IPassableData and returns true if it is valid
        /// </summary>
        /// <param name="rawData">Given IPassableData</param>
        /// <param name="data">Validated data</param>
        /// <typeparam name="T">Type of data</typeparam>
        /// <returns>True if data is valid</returns>
        public static bool Validate<T>(this IPassableData rawData, out T data) where T : class
        {
            data = rawData as T;
            return data != null;
        }

        /// <summary>
        /// Converts the string to json format.
        /// </summary>
        /// <param name="str"> The string to convert.</param>
        /// <returns> The json formatted string.</returns>
        public static string ToJson(this string str)
        {
            var indent = 0;
            var quoted = false;
            var sb = new System.Text.StringBuilder();
            foreach (var ch in str)
            {
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            sb.Append(' ', ++indent * 4);
                        }

                        break;
                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            sb.Append(' ', --indent * 4);
                        }

                        sb.Append(ch);
                        break;
                    case ',':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            sb.Append(' ', indent * 4);
                        }

                        break;
                    case '"':
                        sb.Append(ch);
                        var escaped = false;
                        var index = sb.Length - 2;
                        while (index >= 0 && sb[index] == '\\')
                        {
                            escaped = !escaped;
                            index--;
                        }

                        if (!escaped)
                            quoted = !quoted;

                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        ///  Returns true if the value is between the range.
        /// </summary>
        /// <param name="value"> The value to check.</param>
        /// <param name="range"> The range to check.</param>
        /// <returns> True if the value is between the range.</returns>
        public static bool IsBetween(this float value, Vector2 range) => value >= range.x && value <= range.y;

        /// <summary>
        /// Saves the AudioClip as a WAV file.
        /// </summary>
        /// <param name="clip"> The AudioClip to save.</param>
        /// <param name="path"> The path to save the AudioClip to.</param>
        /// <returns> True if the AudioClip was saved successfully.</returns>
        public static string SaveAsWav(this AudioClip clip, string path = "Assets/Resources/Audio/RecordedAudio.wav")
        {
            if (clip == null)
            {
                Debug.LogError("Invalid AudioClip for saving as WAV.");
                return string.Empty;
            }

            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Invalid file path for saving audio clip.");
                return string.Empty;
            }

            try
            {
                using var fileStream = new FileStream(path, FileMode.Create);
                using var writer = new BinaryWriter(fileStream);

                // Write the RIFF header
                writer.Write("RIFF".ToCharArray());
                writer.Write(0); // Placeholder for file size
                writer.Write("WAVE".ToCharArray());

                // Write the format chunk
                writer.Write("fmt ".ToCharArray());
                writer.Write(16); // Chunk size
                writer.Write((ushort)1); // Format (PCM)
                writer.Write((ushort)clip.channels); // Channels
                writer.Write(clip.frequency); // Sample rate
                writer.Write(clip.frequency * clip.channels * 2); // Byte rate
                writer.Write((ushort)(clip.channels * 2)); // Block align
                writer.Write((ushort)16); // Bits per sample

                // Write the data chunk
                writer.Write("data".ToCharArray());
                writer.Write(0); // Placeholder for data size

                // Get the audio data as a float array
                var audioData = new float[clip.samples * clip.channels];
                clip.GetData(audioData, 0);

                // Convert the audio data to a byte array
                var audioBytes = new byte[audioData.Length * 2];
                for (var i = 0; i < audioData.Length; i++)
                {
                    var sample = (short)(audioData[i] * short.MaxValue);
                    var sampleBytes = BitConverter.GetBytes(sample);
                    audioBytes[i * 2] = sampleBytes[0];
                    audioBytes[i * 2 + 1] = sampleBytes[1];
                }

                // Write the audio data to the file
                writer.Write(audioBytes.Length);
                writer.Write(audioBytes);

                // Update the file size in the RIFF header
                var fileSize = fileStream.Length - 8;
                writer.Seek(4, SeekOrigin.Begin);
                writer.Write((int)fileSize);

                // Update the data size in the data chunk
                var dataSize = fileSize - 44;
                writer.Seek(40, SeekOrigin.Begin);
                writer.Write((int)dataSize);

                return path;
            }
            catch (IOException)
            {
                return string.Empty;
            }
        }
        
        /// <summary>
        ///  Trims the silence from the start and end of the AudioClip.
        /// </summary>
        /// <param name="clip"> The AudioClip to trim.</param>
        /// <returns> The trimmed AudioClip.</returns>
        public static AudioClip Trim(this AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogError("Invalid AudioClip for trimming.");
                return null;
            }

            var samples = new float[clip.samples * clip.channels];
            clip.GetData(samples, 0);

            // Find the start of the audio data
            var startSample = 0;
            while (startSample < samples.Length && Mathf.Approximately(samples[startSample], 0f)) startSample++;

            // Find the end of the audio data
            var endSample = samples.Length - 1;
            while (endSample > startSample && Mathf.Approximately(samples[endSample], 0f)) endSample--;

            // Calculate the length of the trimmed audio clip
            var lengthSamples = endSample - startSample + 1;
            var lengthSeconds = Mathf.CeilToInt((float)lengthSamples / clip.frequency);

            // Create a new audio clip with the trimmed data
            var trimmedClip = AudioClip.Create("Trimmed Audio Clip", lengthSamples, clip.channels, clip.frequency, false);
            var trimmedData = new float[lengthSamples];
            Array.Copy(samples, startSample, trimmedData, 0, lengthSamples);
            trimmedClip.SetData(trimmedData, 0);

            return trimmedClip;
        }
    }
}