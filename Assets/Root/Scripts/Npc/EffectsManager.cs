// EffectsManager.cs

using UnityEngine;

namespace YagizAyer.Root.Scripts.Npc
{
    public class EffectsManager : MonoBehaviour
    {
        public void ShowEffect(ParticleSystem effect) => effect.Play();
    }
}