using System;
using UnityEngine;

namespace MySampleEx
{
    // ĳ���� �Ӽ�
    public enum CharacterAttributes
    {
        Agility,
        Intellect,
        Stamina,
        Strength,
        Health,
        Mana
    }

    // ĳ���� �Ӽ� Ÿ��, ��
    [Serializable]
    public class Attributes
    {
        public CharacterAttributes type;
        public ModifiableInt value;
    }
}