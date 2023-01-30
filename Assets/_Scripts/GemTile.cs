using _Scripts.SO;
using UnityEngine;

namespace _Scripts
{
    public class GemTile : MonoBehaviour
    {
        private Gem _gem;
        private int _x;
        private int _y;
        public bool _isDestroyed;

        public GemTile(Gem gem, int x, int y)
        {
            _gem = gem;
            _x = x;
            _y = y;

            _isDestroyed = false;
        }

        public Gem GetGem()
        {
            return _gem;
        }

        public Vector2Int GetWorldPosition()
        {
            return new Vector2Int(_x, _y);
        }

        public void SetGemPos(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}