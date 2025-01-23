using System.Collections.Generic;
using UnityEngine;
using System;

namespace MySampleEx
{
    // 플레이어 퀘스트 관리 클래스
    public class QuestManager : Singleton<QuestManager>
    {
        // 플레이어가 현재 진행중인 퀘스트 리스트
        public List<QuestObject> playerQuests;
        // 퀘스트 UI에 전달되는 퀘스트
        public QuestObject currentQuest;
        
        // 퀘스트 수락 시 등록된 함수들 호출
        public Action<QuestObject> OnAcceptQuest;
        // 퀘스트 포기 시 등록된 함수들 호출
        public Action<QuestObject> OnGiveupQuest;
        // 퀘스트 완료 시 등로된 함수들 호출
        public Action<QuestObject> OnCompletedQuest;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            playerQuests = new List<QuestObject>();
        }
        
        // 퀘스트UI에 현재 선택된 퀘스트 셋팅
        public void SetCurrentQuest(QuestObject _quest)
        {
            currentQuest = _quest;
        }

        // 플레이어 퀘스트 리스트에 선택된 퀘스트 추가
        public void AddPlayerQuest()
        {
            if (currentQuest == null)
                return;

            OnAcceptQuest?.Invoke(currentQuest);

            Quest quest = DataManager.GetQuestData().questList.quests[currentQuest.number];
            QuestObject newQuest = new QuestObject(quest);

            newQuest.questState = QuestState.Accept;

            playerQuests.Add(newQuest);
            
        }

        public void GiveupPlayerQuest()
        {
            if (currentQuest == null)
                return;

            OnGiveupQuest?.Invoke(currentQuest);

            playerQuests.Remove(currentQuest);
        }

        public void UpdateQuest(QuestType _type, int _goalIndex)
        {
            switch(_type)
            {
                case QuestType.Kill:
                    foreach(var quest in playerQuests)
                    {
                        quest.EnemyKill(_goalIndex);

                        if(quest.questGoal.IsReached)
                        {
                            quest.questState = QuestState.Complete;
                            OnCompletedQuest?.Invoke(quest);
                        }
                    }
                    break;
                case QuestType.Collect:
                    foreach (var quest in playerQuests)
                    {
                        quest.ItemCollect(_goalIndex);

                        if (quest.questGoal.IsReached)
                        {
                            quest.questState = QuestState.Complete;
                            OnCompletedQuest?.Invoke(quest);
                        }
                    }
                    break;
            }
        }

        public void RewardQuest()
        {
            //
            Debug.Log("퀘스트 보상 지급");

            // 플레이어 퀘스트 리스트에서 삭제
            RemovePlayerQuest(currentQuest);
        }

        public void RemovePlayerQuest(QuestObject _questObject)
        {
            foreach (var quest in playerQuests)
            {
                if (quest.number == _questObject.number)
                {
                    playerQuests.Remove(quest);
                    break;
                }
            }
        }

    }
}