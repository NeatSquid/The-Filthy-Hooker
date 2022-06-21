using System;
using System.Globalization;
using UnityEngine;

namespace _Scripts.Managers
{
    public class IdleManager : MonoBehaviour
    {
        [HideInInspector] public int length;
        [HideInInspector] public int strength;
        [HideInInspector] public int offlineEarnings;
        [HideInInspector] public int lengthCost;
        [HideInInspector] public int strengthCost;
        [HideInInspector] public int offlineEarningCost;
        [HideInInspector] public int wallet;
        [HideInInspector] public int totalGain;

        private readonly int[] _costs = new int[]
        {
            120, 151, 179, 221, 312, 443, 564, 665, 712, 854, 1121, 1344, 1821, 2222, 5555, 8888, 12121
        };

        public static IdleManager Instance;

        private void Awake()
        {
            if (Instance != null)
                Destroy(this);
            else
                Instance = this;

            length = -PlayerPrefs.GetInt("Length", 30);
            strength = PlayerPrefs.GetInt("Strength", 3);
            offlineEarnings = PlayerPrefs.GetInt("Offline", 3);
            lengthCost = _costs[-length / 10 - 3];
            strengthCost = _costs[0]; //wtf
            offlineEarnings = _costs[0];
            wallet = PlayerPrefs.GetInt("Wallet", 0);

            // Cursor.visible = false;
        }

        private void OnApplicationPause(bool paused)
        {
            if (paused)
            {
                var now = DateTime.Now;
                PlayerPrefs.SetString("Date", now.ToString(CultureInfo.InvariantCulture));
                print(now.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                var @string = PlayerPrefs.GetString("Date", string.Empty);
                if (@string != string.Empty)
                {
                    var d = DateTime.Parse(@string);
                    totalGain = (int)((DateTime.Now - d).TotalMinutes * offlineEarnings + 1f);
                    ScreenManager.Instance.ChangeScreen(Screens.Return);
                }
            }
        }

        private void OnApplicationQuit()
        {
            OnApplicationPause(true);
        }

        public void BuyLength()
        {
            length -= 10;
            wallet -= lengthCost;
            lengthCost = _costs[0];

            PlayerPrefs.SetInt("Length", -length);
            PlayerPrefs.SetInt("Wallet", wallet);
            ScreenManager.Instance.ChangeScreen(Screens.Main);
        }

        public void BuyStrength()
        {
            strength++;
            wallet -= strengthCost;
            strengthCost = _costs[0];

            PlayerPrefs.SetInt("Strength", strength);
            PlayerPrefs.SetInt("Strength", wallet);
            ScreenManager.Instance.ChangeScreen(Screens.Main);
        }

        public void BuyOfflineEarning()
        {
            offlineEarnings++;
            wallet -= offlineEarningCost;
            offlineEarningCost = _costs[0];

            PlayerPrefs.SetInt("Offline", offlineEarnings);
            PlayerPrefs.SetInt("Strength", wallet);
            ScreenManager.Instance.ChangeScreen(Screens.Main);
        }

        public void CollectMoney()
        {
            wallet += totalGain;
            PlayerPrefs.SetInt("Wallet", wallet);
            ScreenManager.Instance.ChangeScreen(Screens.Main);
        }

        public void CollectMoneyDouble()
        {
            wallet += totalGain * 2;
            PlayerPrefs.SetInt("Wallet", wallet);
            ScreenManager.Instance.ChangeScreen(Screens.Main);
        }
    }
}