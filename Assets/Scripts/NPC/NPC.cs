using UnityEngine;
using System;

namespace MySampleEx
{
    public enum NpcType
    {
        None = -1,
        Merchant,
        BlackSmith,
        SkillMasterm,
        QuestGiver,

    }

    [Serializable]
    public class NPC
    {
        // npc 타입
        public NpcType npcType;
        // npc 고유번호
        public int number;
        // npc 이름
        public string name;
    }
}