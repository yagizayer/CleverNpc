// Extensions.cs

using System.Collections.Generic;
using System.Linq;
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
        public static bool Validate<T>(this IPassableData rawData, out PassableDataBase<T> data)
        {
            data = rawData as PassableDataBase<T>;
            return data != null;
        }

        /// <summary>
        ///     Gives relative direction of caller vector based on cameras forward vector
        /// </summary>
        /// <param name="me">caller vector(usually movement direction)</param>
        /// <param name="currentCamera">camera to relate operation</param>
        /// <returns>relative direction based on given camera</returns>
        public static Vector3 RelativeToCamera(this Vector3 me, Transform currentCamera)
        {
            var cameraForwardNormalized = Vector3.ProjectOnPlane(currentCamera.forward, Vector3.up);
            var rotationToCamNormal = Quaternion.LookRotation(cameraForwardNormalized, Vector3.up);

            var finalMoveDir = rotationToCamNormal * me;
            return finalMoveDir;
        }

        public static Vector3 RelativeToCamera(this Vector3 me, Camera currentCamera)
        {
            return RelativeToCamera(me, currentCamera.transform);
        }

        /// <summary>
        ///     Returns given vector with projected values based on given vector normal
        /// </summary>
        /// <param name="me">given vector</param>
        /// <param name="planeNormal">plane normal</param>
        /// <returns>Projected vector</returns>
        public static Vector3 OnPlane(this Vector3 me, Vector3 planeNormal = default)
        {
            if (planeNormal == default)
                planeNormal = Vector3.up;

            planeNormal.Normalize();
            return Vector3.ProjectOnPlane(me, planeNormal);
        }

        /// <summary>
        /// Converts given vector to quaternion
        /// </summary>
        /// <param name="me"> given vector</param>
        /// <returns>converted quaternion</returns>
        public static Quaternion ToQuaternion(this Vector3 me) => Quaternion.Euler(me);

        public static bool TryGetValue<TKey, TVal, TTargetType>(this Dictionary<TKey, TVal> dict, TKey key,
            out TTargetType value) where TTargetType : TVal
        {
            if (dict.TryGetValue(key, out var val))
            {
                value = (TTargetType)val;
                return true;
            }

            value = default;
            return false;
        }
    }
}