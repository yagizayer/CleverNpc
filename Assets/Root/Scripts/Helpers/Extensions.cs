// Extensions.cs

using System.Collections.Generic;
using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;

namespace YagizAyer.Root.Scripts.Helpers
{
    public static class Extensions
    {
        public static List<T> Clone<T>(this IEnumerable<T> list) => new(list);
        
        public static IPassableData ToPassableData<T>(this T value) => new PassableDataBase<T>(value);

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
    }
}