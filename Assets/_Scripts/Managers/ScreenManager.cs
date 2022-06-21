using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Managers
{
    public class ScreenManager : MonoBehaviour
    {
        public static ScreenManager Instance;

        [HideInInspector] public GameObject currentScreen;

        public GameObject endScreen;
        public GameObject gameScreen;
        public GameObject mainScreen;
        public GameObject returnScreen;

        public Button lengthButton;
        public Button strengthButton;
        public Button offlineButton;

        public Text gameScreenMoney;
        public Text lengthCostText;
        public Text lengthValueText;
        public Text strengthCostText;
        public Text strengthValueText;
        public Text offlineCostText;
        public Text offlineValueText;
        public Text endScreenMoney;
        public Text returnScreenMoney;

        private int gameCount;

        private void Awake()
        {
            if (Instance != null)
                Destroy(this);
            else
            {
                Instance = this;
            }

            currentScreen = endScreen;
        }

        private void Start()
        {
            CheckIdles();
            UpdateTexts();
        }

        public void ChangeScreen(Screens screen)
        {
            currentScreen.SetActive(false);

            switch (screen)
            {
                case Screens.Main:
                    currentScreen = mainScreen;
                    UpdateTexts();
                    CheckIdles();
                    break;
                case Screens.End:
                    currentScreen = endScreen;
                    SetEndScreenMoney();
                    break;
                case Screens.Game:
                    currentScreen = gameScreen;
                    gameCount++;
                    break;
                case Screens.Return:
                    currentScreen = returnScreen;
                    SetReturnScreenMoney();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(screen), screen, null);
            }

            currentScreen.SetActive(true);
        }

        public void SetEndScreenMoney()
        {
            endScreenMoney.text = $"$ {IdleManager.Instance.totalGain}";
        }

        public void SetReturnScreenMoney()
        {
            returnScreenMoney.text = $"$ {IdleManager.Instance.totalGain} gained while waiting!";
        }

        private void CheckIdles()
        {
            var lengthCost = IdleManager.Instance.lengthCost;
            var strengthCost = IdleManager.Instance.strengthCost;
            var offlineEarningCost = IdleManager.Instance.offlineEarningCost;
            var wallet = IdleManager.Instance.wallet;

            lengthButton.interactable = wallet >= lengthCost;
            strengthButton.interactable = wallet >= strengthCost;
            offlineButton.interactable = wallet >= offlineEarningCost;
        }

        private void UpdateTexts()
        {
            gameScreenMoney.text = $"$ {IdleManager.Instance.wallet}";
            lengthCostText.text = $"$ {IdleManager.Instance.lengthCost}";
            lengthValueText.text = $"-{IdleManager.Instance.lengthCost} m";
            strengthCostText.text = $"$ {IdleManager.Instance.strengthCost}";
            strengthValueText.text = $"{IdleManager.Instance.strength} fishes";
            offlineCostText.text = $"$ {IdleManager.Instance.offlineEarningCost}";
            offlineValueText.text = $"$ {IdleManager.Instance.offlineEarnings} /min";
        }
    }
}