using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Fish
{
    public class Fish : MonoBehaviour
    {
        private FishType _type;
        private CircleCollider2D _collider;
        private SpriteRenderer _renderer;
        private float _screenLeft;
        private Tweener _tweener;

        public FishType Type
        {
            get => _type;
            set
            {
                _type = value;
                _collider.radius = Type.collRad;
                _renderer.sprite = Type.sprite;
            }
        }

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _renderer = GetComponentInChildren<SpriteRenderer>();
            if (Camera.main != null) _screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        }

        public void ResetFish()
        {
            _tweener?.Kill();

            var num = Random.Range(Type.minDepth, Type.maxDepth);
            _collider.enabled = true;

            var fishTran = transform;
            var fishPos = fishTran.position;

            fishPos.y = num;
            fishPos.x = _screenLeft;
            fishTran.position = fishPos;

            const float num1 = 1f;
            var y = Random.Range(num - num1, num + num1);
            var v = new Vector2(-fishPos.x, y);

            const float num2 = 3f;
            var delay = Random.Range(0f, 2f * num2);

            _tweener = transform.DOMove(v, num2).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetDelay(delay)
                .OnStepComplete(delegate
                {
                    var lScale = transform.localScale;
                    lScale.x = -lScale.x;
                    transform.localScale = lScale;
                });
        }

        public void Hooked()
        {
            _collider.enabled = false;
            _tweener.Kill();
        }

        [Serializable]
        public class FishType
        {
            public int price;
            public float fishAmount;
            public float minDepth;
            public float maxDepth;
            public float collRad;
            public Sprite sprite;
        }
    }
}