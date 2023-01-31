using UnityEngine;

namespace _Scripts.SO
{
    [CreateAssetMenu]
    public class Level : ScriptableObject
    {
        public int timeLimit;
        public int gridSizeX;
        public int gridSizeY;
    }
}