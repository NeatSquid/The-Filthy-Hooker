using System;
using UnityEngine;

namespace _Scripts.Fish
{
    public class FishSpawner : MonoBehaviour
    {
        [SerializeField] private Fish _fishPrefab;
        [SerializeField] private Fish.FishType[] _fishTypes;

        private void Awake()
        {
            foreach (var t in _fishTypes)
            {
                var num = 0;
                while (num < t.fishAmount)
                {
                    var fish = Instantiate(_fishPrefab);
                    fish.Type = t;
                    fish.ResetFish();
                    num++;
                }
            }
        }
    }
}