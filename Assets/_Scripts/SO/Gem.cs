using _Scripts.Enums;
using UnityEngine;

namespace _Scripts.SO
{
    [CreateAssetMenu()]
    public class Gem : ScriptableObject
    {
        public GemType gemType;
        public Sprite sprite;
    }
}