using System;
using UnityEngine;

namespace MySampleEx
{
    // 캐릭터 속성
    public enum CharacterAttributes
    {
        Agility,
        Intellect,
        Stamina,
        Strength,
        Health,
        Mana
    }

    // 캐릭터 속성 타입, 값
    [Serializable]
    public class Attributes
    {
        public CharacterAttributes type;
        public ModifiableInt value;
    }
}