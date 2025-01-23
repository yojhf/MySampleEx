using UnityEngine;
using System;

namespace MySampleEx
{
    // ����Ʈ ���� ������ ���� Ŭ����
    [Serializable]
    public class QuestObject
    {
        // ����Ʈ �ε���
        public int number { get; set; }
        // ����Ʈ ����
        public QuestState questState;
        // ����Ʈ ��ǥ
        public QuestGoal questGoal;

        // ������
        public QuestObject(Quest _quest) 
        {
            number = _quest.number;
            questState = QuestState.Ready;

            questGoal = new QuestGoal();

            questGoal.questType = _quest.questType;
            questGoal.goalIndex = _quest.goalIndex;
            questGoal.goalAmount = _quest.goalAmount;
            questGoal.currentAmount = 0;
        }

        // ����Ʈ �̼� �޼� - Kill
        public void EnemyKill(int enemyId)
        {
            if (questGoal.questType == QuestType.Kill)
            {
                // Enemy Id üũ
                //if (questGoal.goalIndex == enemyId)
                {
                    questGoal.currentAmount++;
                }
            }
        }

        // ����Ʈ �̼� �޼� - Collect
        public void ItemCollect(int itemId)
        {
            if (questGoal.questType == QuestType.Collect)
            {
                // Item Id üũ
                //if (questGoal.goalIndex == itemId)
                {
                    questGoal.currentAmount++;
                }
            }
        }

    }
}