// GetDown.cs

using UnityEngine;

namespace YagizAyer.Root.Sandbox
{
    public class GetDown : MonoBehaviour
    {
        [ContextMenu("DropEveryChildToGround")]
        public void DropEveryChildToGround()
        {
            var allChildren = GetComponentsInChildren<Transform>();
            foreach (var child in allChildren)
            {
                if (child == transform) continue;
                var ray = new Ray(child.position, Vector3.down);
                if (Physics.Raycast(ray, out var hit)) child.position = hit.point;
            }
        }

        [ContextMenu("DropMeFromAbove")]
        public void DropMeFromAbove()
        {
            var ray = new Ray(transform.position + Vector3.up * 3, Vector3.down);
            var rays = Physics.RaycastAll(ray);
            foreach (var hit in rays)
            {
                if (hit.transform == transform) continue;
                transform.position = hit.point;
                break;
            }
        }
    }
}