// Extensions.cs

using System.Collections.Generic;
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
        public static bool Validate<T>(this IPassableData rawData, out T data) where T :class
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
    }
}