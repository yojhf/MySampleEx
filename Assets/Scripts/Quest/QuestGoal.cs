using System;
using UnityEngine;

namespace MySampleEx
{
    [Serializable]
    public class QuestGoal
    {
        // ����Ʈ Ÿ��
        public QuestType questType { get; set; }
        // ����Ʈ ��ǥ �� ������ Ÿ��, Npc Ÿ��
        public int goalIndex { get; set; }
        // ����Ʈ ��ǥ ����
        public int goalAmount { get; set; }
        // ����Ʈ ���� �޼���
        [NonSerialized]
        public int currentAmount;
        // ����Ʈ �޼� ����
        public bool IsReached => (currentAmount >= goalAmount);

    }
}