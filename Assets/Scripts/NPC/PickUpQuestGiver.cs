using System.Collections.Generic;
using UnityEngine;

namespace MySampleEx
{
    // ����Ʈ�� ���� NPC
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

        // npc�� �ش� �ε����� ������ ����Ʈ ��� ��������
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
                // �����ϰ� ��ȭ ����
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
        
        // ����Ʈ �Ϸ� ó��
        public void CompletedQuest()
        {
            // ����Ʈ ���� �ޱ�
            questManager.RewardQuest();

            // NPC ����Ʈ ����Ʈ ����
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