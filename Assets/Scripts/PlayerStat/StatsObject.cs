using System;
using UnityEngine;

namespace MySampleEx
{
    // 캐릭터 스탯 데이터를 가지고 있는 스크립터블 오브젝트
    [CreateAssetMenu(fileName = "New StatsObject", menuName = "Stats System/New Character Stats")]
    public class StatsObject : ScriptableObject
    {
        // 캐릭터 속성 값들
        public Attributes[] attributes;

        public int level;
        public int exp;

        // 스탯 변경 시 등록된 함수 호출
        public Action<StatsObject> OnChangedStats;

        // 최초 1회 초기화 체크
        [NonSerialized]
        private bool isStart = false;

        public int Health {  get; set; }
        public int Mana { get; set; }

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
            Debug.Log("초기화");

            // attributes의 value 객체 생성
            foreach(var attribute in attributes)
            {
                attribute.value = new ModifiableInt(OnModifiedValue);
            }

            level = 1;
            exp = 0;

            SetBaseValue(CharacterAttributes.Agility, 100);
            SetBaseValue(CharacterAttributes.Intellect, 100);
            SetBaseValue(CharacterAttributes.Stamina, 100);
            SetBaseValue(CharacterAttributes.Strength, 100);
            SetBaseValue(CharacterAttributes.Health, 100);
            SetBaseValue(CharacterAttributes.Mana, 100);

            // Current Health, Mana 초기화
            Health = GetModifiredValue(CharacterAttributes.Health);
            Mana = GetModifiredValue(CharacterAttributes.Mana);

        }
        
        // 속성값 초기화
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

        // BaseValue 값 가져오기
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

        // 최종 속성값 가져오기
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

        // 모든 attributes의 value 값이 변경되면 호춣되는 함수
        void OnModifiedValue(ModifiableInt _value)
        {
            OnChangedStats?.Invoke(this);
        }
    }
}