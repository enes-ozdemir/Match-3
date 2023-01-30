using System;
using _Scripts.SO;
using UnityEngine;

namespace _Scripts
{
    public class GemTile : MonoBehaviour
    {
        [SerializeField] private Gem gem;
        private int _x;
        private int _y;
        public bool _isDestroyed;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetGemPosition(int x, int y)
        {
            transform.position = new Vector2(x, y);
            _x = x;
            _y = y;
        }

        public void InitializeGem(Gem gem, int x, int y)
        {
            this.gem = gem;
            _spriteRenderer.sprite = gem.sprite;
            SetGemPosition(x, y);
        }

        public GemTile(Gem gem, int x, int y)
        {
            gem = gem;
            _x = x;
            _y = y;

            _isDestroyed = false;
        }

        public Gem GetGem()
        {
            return gem;
        }

        public Vector2Int GetWorldPosition()
        {
            return new Vector2Int(_x, _y);
        }
    }
}