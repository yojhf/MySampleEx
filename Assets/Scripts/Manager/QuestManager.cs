using System.Collections.Generic;
using UnityEngine;
using System;

namespace MySampleEx
{
    // �÷��̾� ����Ʈ ���� Ŭ����
    public class QuestManager : Singleton<QuestManager>
    {
        // �÷��̾ ���� �������� ����Ʈ ����Ʈ
        public List<QuestObject> playerQuests;
        // ����Ʈ UI�� ���޵Ǵ� ����Ʈ
        public QuestObject currentQuest;
        
        // ����Ʈ ���� �� ��ϵ� �Լ��� ȣ��
        public Action<QuestObject> OnAcceptQuest;
        // ����Ʈ ���� �� ��ϵ� �Լ��� ȣ��
        public Action<QuestObject> OnGiveupQuest;
        // ����Ʈ �Ϸ� �� ��ε� �Լ��� ȣ��
        public Action<QuestObject> OnCompletedQuest;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            playerQuests = new List<QuestObject>();
        }
        
        // ����ƮUI�� ���� ���õ� ����Ʈ ����
        public void SetCurrentQuest(QuestObject _quest)
        {
            currentQuest = _quest;
        }

        // �÷��̾� ����Ʈ ����Ʈ�� ���õ� ����Ʈ �߰�
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
            Debug.Log("����Ʈ ���� ����");

            // �÷��̾� ����Ʈ ����Ʈ���� ����
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