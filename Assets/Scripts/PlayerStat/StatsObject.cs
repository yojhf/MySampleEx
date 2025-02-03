using System;
using UnityEngine;

namespace MySampleEx
{
    // Ä³¸¯ÅÍ ½ºÅÈ µ¥ÀÌÅÍ¸¦ °¡Áö°í ÀÖ´Â ½ºÅ©¸³ÅÍºí ¿ÀºêÁ§Æ®
    [CreateAssetMenu(fileName = "New StatsObject", menuName = "Stats System/New Character Stats")]
    public class StatsObject : ScriptableObject
    {
        // Ä³¸¯ÅÍ ¼Ó¼º °ªµé
        public Attributes[] attributes;

        [SerializeField] private int level;
        [SerializeField] private int exp;
        [SerializeField] private int gold;

        // ½ºÅÈ º¯°æ ½Ã µî·ÏµÈ ÇÔ¼ö È£Ãâ
        public Action<StatsObject> OnChangedStats;

        // ÃÖÃÊ 1È¸ ÃÊ±âÈ­ Ã¼Å©
        [NonSerialized]
        private bool isStart = false;

        public int Health {  get; set; }
        public int Mana { get; set; }
        public int Level => level;
        public int Exp => exp;
        public int Gold => gold;


        public float HealthPercentage 
        {
            get 
            { 
                int health = Health;
                int maxHealth = health;

                foreach (var attribute in attributes)
                {
                    if (attribute.type == CharacterAttributes.Health)
                    {
                        maxHealth = attribute.value.ModifedValue;
                    }
                }

                return (maxHealth > 0) ? (float)health / (float)maxHealth : 0f;
            } 
        }

        public float ManaPercentage
        {
            get
            {
                int mana = Mana;
                int maxMana = mana;

                foreach (var attribute in attributes)
                {
                    if (attribute.type == CharacterAttributes.Mana)
                    {
                        maxMana = attribute.value.ModifedValue;
                    }
                }

                return (maxMana > 0) ? (float)mana / (float)maxMana : 0f;
            }
        }

        private void OnEnable()
        {
            InitializeAttributes();
        }

        void InitializeAttributes()
        {
            if (isStart)
                return;

            isStart = true;
            Debug.Log("ÃÊ±âÈ­");

            // attributesÀÇ value °´Ã¼ »ý¼º
            foreach(var attribute in attributes)
            {
                attribute.value = new ModifiableInt(OnModifiedValue);
            }

            level = 1;
            exp = 0;
            gold = 100;

            SetBaseValue(CharacterAttributes.Agility, 100);
            SetBaseValue(CharacterAttributes.Intellect, 100);
            SetBaseValue(CharacterAttributes.Stamina, 100);
            SetBaseValue(CharacterAttributes.Strength, 100);
            SetBaseValue(CharacterAttributes.Health, 100);
            SetBaseValue(CharacterAttributes.Mana, 100);

            // Current Health, Mana ÃÊ±âÈ­
            Health = GetModifiredValue(CharacterAttributes.Health);
            Mana = GetModifiredValue(CharacterAttributes.Mana);

        }
        
        // ¼Ó¼º°ª ÃÊ±âÈ­
        void SetBaseValue(CharacterAttributes _type, int _value)
        {
            foreach (var attribute in attributes)
            {
                if (attribute.type == _type)
                {
                    attribute.value.BaseValue = _value;
                }
            }
        }

        // BaseValue °ª °¡Á®¿À±â
        public int GetBaseValue(CharacterAttributes _type)
        {
            foreach (var attribute in attributes)
            {
                if (attribute.type == _type)
                {
                    return attribute.value.BaseValue;
                }
            }

            return -1;
        }

        // ÃÖÁ¾ ¼Ó¼º°ª °¡Á®¿À±â
        public int GetModifiredValue(CharacterAttributes _type)
        {
            foreach (var attribute in attributes)
            {
                if (attribute.type == _type)
                {
                    return attribute.value.ModifedValue;
                }
            }

            return -1;
        }

        // ¸ðµç attributesÀÇ value °ªÀÌ º¯°æµÇ¸é È£­„µÇ´Â ÇÔ¼ö
        void OnModifiedValue(ModifiableInt _value)
        {
            OnChangedStats?.Invoke(this);
        }

        public void AddGold(int _amount)
        {
            gold += _amount;

            // ½ºÅÈº¯°æ ½Ã ÀúÀåµÈ ÀÌº¥Æ® ÇÔ¼ö È£Ãâ
            OnChangedStats?.Invoke(this);
        }

        public bool UseGold(int _amount)
        {
            if (gold < _amount)
            {
                return false;
            }

            gold -= _amount;

            OnChangedStats?.Invoke(this);

            return true;
        }

        public bool AddExp(int _amount)
        {
            bool isLevelUp = false;

            exp += _amount;

            // ·¹º§¾÷ Ã¼Å©
            while (exp >= GetExpForLevelUp(level))
            {
                exp -= GetExpForLevelUp(level);
                level++;

                // ·¹º§¾÷ º¸»ó


                isLevelUp = true;
            }

            OnChangedStats?.Invoke(this);

            return isLevelUp;
        }

        // ÁöÁ¤ÇÑ ·¹º§¿¡¼­ ´ÙÀ½ ·¹º§·Î °¡´Âµ¥ ÇÊ¿äÇÑ °æÇèÄ¡
        public int GetExpForLevelUp(int nowLevel)
        {
            return level * 100;   
        }
    }
}