using System.Collections.Generic;
using UnityEngine;

namespace MySampleEx
{
    // 퀘스트를 가진 NPC
    public class PickUpQuestGiver : PickUpNpc
    {
        public List<QuestObject> quests;

        QuestManager questManager;
        protected override void Start()
        {
            base.Start();

            quests = GetNpcQuest(npc.number);

            questManager = QuestManager.Instance;

            questManager.OnAcceptQuest += OnAcceptQuest;
            questManager.OnGiveupQuest += OnGiveupQuest;
            questManager.OnCompletedQuest += OnCompletedQuest;
        }

        private void OnEnable()
        {


        }

        // npc의 해당 인덱스에 지정된 퀘스트 목록 가져오기
        public List<QuestObject> GetNpcQuest(int npcNum)
        {
            List<QuestObject> questObjects = new List<QuestObject>();

            foreach (var quest in DataManager.GetQuestData().questList.quests)
            {
                if(quest.npcNumber == npcNum)
                {
                    QuestObject newQuest = new QuestObject(quest);

                    questObjects.Add(newQuest);    
                }
            }

            return questObjects;
        }

        public override void DoAction()
        {
            if(quests.Count == 0)
            {
                // 랜덤하게 대화 진행
                int randomNum = Random.Range(0, 3);

                UIManager.Instance.OpenDialogUI(randomNum);
                return;
            }

            questManager.currentQuest = quests[0];

            int dialogIndex = DataManager.GetQuestData().questList.quests[quests[0].number].dialogIndex;

            switch (quests[0].questState)
            {
                case QuestState.Ready:
                    UIManager.Instance.OpenDialogUI(dialogIndex, npc.npcType);
                    break;
                case QuestState.Accept:
                    UIManager.Instance.OpenDialogUI(dialogIndex + 1);
                    break;
                case QuestState.Complete:
                    UIManager.Instance.OpenDialogUI(dialogIndex + 2, npc.npcType);
                    CompletedQuest();
                    break;
            }
        }
        
        // 퀘스트 완료 처리
        public void CompletedQuest()
        {
            // 퀘스트 보상 받기
            questManager.RewardQuest();

            // NPC 퀘스트 리스트 제거
            quests.Remove(quests[0]);
        }

        public void OnAcceptQuest(QuestObject _questObject)
        {
            foreach(var quest in quests)
            {
                if(quest.number == _questObject.number)
                {
                    quest.questState = QuestState.Accept;
                }
            }
        }

        public void OnGiveupQuest(QuestObject _questObject)
        {
            foreach (var quest in quests)
            {
                if (quest.number == _questObject.number)
                {
                    quest.questState = QuestState.Ready;
                }
            }
        }

        public void OnCompletedQuest(QuestObject _questObject)
        {
            foreach (var quest in quests)
            {
                if (quest.number == _questObject.number)
                {
                    quest.questState = QuestState.Complete;
                }
            }
        }

    }
}