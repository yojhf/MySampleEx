using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MySampleEx
{
    public class PlayerInGameUI : MonoBehaviour
    {
        public StatsObject statsObject;

        public Image healthBar;
        public Image manaBar;

        public TMP_Text levelText;
        public TMP_Text expText;
        public TMP_Text goldText;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            OnChangedStats(statsObject);
        }

        private void OnEnable()
        {
            statsObject.OnChangedStats += OnChangedStats;
        }

        private void OnDisable()
        {
            statsObject.OnChangedStats -= OnChangedStats;
        }

        private void OnChangedStats(StatsObject _object)
        {
            healthBar.fillAmount = statsObject.HealthPercentage;
            manaBar.fillAmount = statsObject.ManaPercentage;

            levelText.text = statsObject.Level.ToString();
            expText.text = statsObject.Exp.ToString() + " / " + statsObject.GetExpForLevelUp(statsObject.Level).ToString();
            goldText.text = statsObject.Gold.ToString();
        }
    }
}