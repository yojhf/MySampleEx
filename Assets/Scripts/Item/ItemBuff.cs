using UnityEngine;
using System;

namespace MySampleEx
{
    // 아이템 능력치 관리 클래스
    [Serializable]
    public class ItemBuff : IModifier
    {
        public CharacterAttributes stat;
        public int value;

        [SerializeField] private int min;
        [SerializeField] private int max;

        public int Min => min;
        public int Max => max;

        // 생성자
        public ItemBuff(int _min, int _max) 
        {
            min = _min;
            max = _max;
            GenerateValue();
        }

        // 능력치 랜덤값 생성
        public void GenerateValue()
        {
            value = UnityEngine.Random.Range(min, max);
        }

        // 매개변수로 입력받은 변수에 value값을 누적
        public void AddValue(ref int baseValue)
        {
            baseValue += value;
        }
    }
}