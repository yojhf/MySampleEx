using UnityEngine;
using System;

namespace MySampleEx
{
    // 퀘스트 진행 데이터 관리 클래스
    [Serializable]
    public class QuestObject
    {
        // 퀘스트 인덱스
        public int number { get; set; }
        // 퀘스트 상태
        public QuestState questState;
        // 퀘스트 목표
        public QuestGoal questGoal;

        // 생성자
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

        // 퀘스트 미션 달성 - Kill
        public void EnemyKill(int enemyId)
        {
            if (questGoal.questType == QuestType.Kill)
            {
                // Enemy Id 체크
                //if (questGoal.goalIndex == enemyId)
                {
                    questGoal.currentAmount++;
                }
            }
        }

        // 퀘스트 미션 달성 - Collect
        public void ItemCollect(int itemId)
        {
            if (questGoal.questType == QuestType.Collect)
            {
                // Item Id 체크
                //if (questGoal.goalIndex == itemId)
                {
                    questGoal.currentAmount++;
                }
            }
        }

    }
}