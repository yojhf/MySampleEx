using System;
using UnityEngine;

namespace MySampleEx
{
    // ĳ���� ���� �����͸� ������ �ִ� ��ũ���ͺ� ������Ʈ
    [CreateAssetMenu(fileName = "New StatsObject", menuName = "Stats System/New Character Stats")]
    public class StatsObject : ScriptableObject
    {
        // ĳ���� �Ӽ� ����
        public Attributes[] attributes;

        [SerializeField] private int level;
        [SerializeField] private int exp;
        [SerializeField] private int gold;

        // ���� ���� �� ��ϵ� �Լ� ȣ��
        public Action<StatsObject> OnChangedStats;

        // ���� 1ȸ �ʱ�ȭ üũ
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
            Debug.Log("�ʱ�ȭ");

            // attributes�� value ��ü ����
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

            // Current Health, Mana �ʱ�ȭ
            Health = GetModifiredValue(CharacterAttributes.Health);
            Mana = GetModifiredValue(CharacterAttributes.Mana);

        }
        
        // �Ӽ��� �ʱ�ȭ
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

        // BaseValue �� ��������
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

        // ���� �Ӽ��� ��������
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

        // ��� attributes�� value ���� ����Ǹ� ȣ���Ǵ� �Լ�
        void OnModifiedValue(ModifiableInt _value)
        {
            OnChangedStats?.Invoke(this);
        }

        public void AddGold(int _amount)
        {
            gold += _amount;

            // ���Ⱥ��� �� ����� �̺�Ʈ �Լ� ȣ��
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

            // ������ üũ
            while (exp >= GetExpForLevelUp(level))
            {
                exp -= GetExpForLevelUp(level);
                level++;

                // ������ ����


                isLevelUp = true;
            }

            OnChangedStats?.Invoke(this);

            return isLevelUp;
        }

        // ������ �������� ���� ������ ���µ� �ʿ��� ����ġ
        public int GetExpForLevelUp(int nowLevel)
        {
            return level * 100;   
        }
    }
}