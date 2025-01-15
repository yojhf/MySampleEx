using UnityEngine;
using System;

namespace MySampleEx
{
    // ������ �ɷ�ġ ���� Ŭ����
    [Serializable]
    public class ItemBuff : IModifier
    {
        public CharacterAttributes stat;
        public int value;

        [SerializeField] private int min;
        [SerializeField] private int max;

        public int Min => min;
        public int Max => max;

        // ������
        public ItemBuff(int _min, int _max) 
        {
            min = _min;
            max = _max;
            GenerateValue();
        }

        // �ɷ�ġ ������ ����
        public void GenerateValue()
        {
            value = UnityEngine.Random.Range(min, max);
        }

        // �Ű������� �Է¹��� ������ value���� ����
        public void AddValue(ref int baseValue)
        {
            baseValue += value;
        }
    }
}