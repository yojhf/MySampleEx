using System;
using UnityEngine;

namespace MySampleEx
{
    [Serializable]
    public class QuestGoal
    {
        // ����Ʈ Ÿ��
        public QuestType questType;
        // ����Ʈ ��ǥ �� ������ Ÿ��, Npc Ÿ��
        public int goalIndex;
        // ����Ʈ ��ǥ ����
        public int goalAmount;
        // ����Ʈ ���� �޼���
        public int currentAmount;
        // ����Ʈ �޼� ����
        public bool IsReached => (currentAmount >= goalAmount);
    }
}