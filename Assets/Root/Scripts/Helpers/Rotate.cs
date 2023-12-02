using System;
using System.Collections;
using UnityEngine;

namespace Helpers
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField] public float speed = 1f;
        [SerializeField] public SnapAxis targetAxis = SnapAxis.Y;

        public virtual void Update()
        {
            transform.Rotate(GetAxis(), speed * Time.deltaTime);
        }

        public virtual Vector3 GetAxis() =>
            targetAxis switch
            {
                SnapAxis.X => Vector3.right,
                SnapAxis.Y => Vector3.up,
                SnapAxis.Z => Vector3.forward,
                SnapAxis.None => Vector3.zero,
                SnapAxis.All => Vector3.one,
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}