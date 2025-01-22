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
        // npc Ÿ��
        public NpcType npcType;
        // npc ������ȣ
        public int number;
        // npc �̸�
        public string name;
    }
}