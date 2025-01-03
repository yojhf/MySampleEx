using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MySampleEx
{
    public class CardUI : MonoBehaviour
    {
        [SerializeField] private Card card;

        public Image artImage;
        public TMP_Text mana;
        public TMP_Text atk;
        public TMP_Text health;
        public TMP_Text description;
        public TMP_Text name;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            name.text = card.name;
            description.text = card.description;
            mana.text = card.mana.ToString();
            atk.text = card.atk.ToString();
            health.text = card.health.ToString();
            artImage.sprite = card.artImage;
        }
    }
}