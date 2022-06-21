using System;
using System.Collections.Generic;
using _Scripts.Managers;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Hook
{
    public class Hook : MonoBehaviour
    {
        [SerializeField] private Transform _hookTrans;

        private Camera _camera;
        private Collider2D _collider;

        private int _length;
        private int _strength;
        private int _fishCount;

        private bool _canMove;

        private List<Fish.Fish> _hookedFishes;

        private Tweener _camTween;

        private void Awake()
        {
            _camera = Camera.main;
            _collider = GetComponent<Collider2D>();
            _hookedFishes = new List<Fish.Fish>();
        }

        private void Update()
        {
            if (_canMove && Input.GetMouseButton(0))
            {
                var hookTrans = transform;

                var worldPoint = _camera.ScreenToWorldPoint(Input.mousePosition);

                var pos = hookTrans.position;
                pos.x = worldPoint.x;

                hookTrans.position = pos;
            }
        }

        public void StartFishing()
        {
            _length = IdleManager.Instance.length - 20;
            _strength = IdleManager.Instance.strength;
            _fishCount = 0;
            var time = -_length * .1f;

            _camTween = _camera.transform.DOMoveY(_length, 1f + time * .25f).OnUpdate(() =>
            {
                if (_camera.transform.position.y <= -11f)
                    transform.SetParent(_camera.transform);
            }).OnComplete(() =>
            {
                _collider.enabled = true;
                _camTween = _camera.transform.DOMoveY(0f, time * 5f, false).OnUpdate(() =>
                {
                    if (_camera.transform.position.y >= -25f)
                        StopFishing();
                });
            });

            ScreenManager.Instance.ChangeScreen(Screens.Game);
            _collider.enabled = false;
            _canMove = true;
            _hookedFishes.Clear();
        }

        private void StopFishing()
        {
            _canMove = false;
            _camTween.Kill();
            _camTween = _camera.transform.DOMoveY(0f, 2f).OnUpdate(() =>
            {
                if (_camera.transform.position.y >= -11f)
                {
                    transform.SetParent(null);
                    transform.position = new Vector2(transform.position.x, -6f);
                }
            }).OnComplete(() =>
            {
                transform.position = Vector2.down * 6f;
                _collider.enabled = true;

                var num = 0;
                foreach (var fish in _hookedFishes)
                {
                    fish.transform.SetParent(null);
                    fish.ResetFish();
                    num += fish.Type.price;
                }

                IdleManager.Instance.totalGain = num;
                ScreenManager.Instance.ChangeScreen(Screens.End);
            });
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Fish") && _fishCount != _strength)
            {
                _fishCount++;
                var component = col.GetComponent<Fish.Fish>();
                component.Hooked();
                _hookedFishes.Add(component);
                col.transform.SetParent(transform);

                col.transform.position = _hookTrans.position;
                col.transform.rotation = _hookTrans.rotation;
                col.transform.localScale = Vector3.one;

                col.transform.DOShakeRotation(5f, Vector3.forward * 45f, 10, 90).SetLoops(1, LoopType.Yoyo).OnComplete(
                    delegate { col.transform.rotation = Quaternion.identity; });

                if (_fishCount == _strength)
                    StartFishing();
            }
        }
    }
}