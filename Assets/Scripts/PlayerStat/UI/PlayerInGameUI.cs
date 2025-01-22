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

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            statsObject.OnChangedStats += OnChangedStats;
        }

        private void OnDisable()
        {
            statsObject.OnChangedStats -= OnChangedStats;
        }

        void Init()
        {
            healthBar.fillAmount = statsObject.HealthPercentage;
            manaBar.fillAmount = statsObject.ManaPercentage;
            levelText.text = statsObject.level.ToString();
            expText.text = statsObject.exp.ToString();
        }

        private void OnChangedStats(StatsObject _object)
        {
            healthBar.fillAmount = _object.HealthPercentage;
            manaBar.fillAmount = _object.ManaPercentage;
        }
    }
}