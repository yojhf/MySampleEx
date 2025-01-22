using System.Collections.Generic;
using UnityEngine;

namespace MySampleEx
{
    // ����Ʈ�� ���� NPC
    public class PickUpQuestGiver : PickUpNpc
    {
        public List<Quest> quests;

        protected override void Start()
        {
            base.Start();

            quests = GetNpcQuest(npc.number);
        }

        // npc�� �ش� �ε����� ������ ����Ʈ ��� ��������
        public List<Quest> GetNpcQuest(int npcNum)
        {
            List<Quest> result = new List<Quest>();

            foreach (var quest in DataManager.GetQuestData().questList.quests)
            {
                if(quest.npcNumber == npcNum)
                {
                    Quest newQuest = new Quest();

                    newQuest.number = quest.number;
                    newQuest.npcNumber = quest.npcNumber;
                    newQuest.name = quest.name;
                    newQuest.description = quest.description;   
                    newQuest.dialogIndex = quest.dialogIndex;
                    newQuest.level = quest.level;

                    newQuest.questGoal = new QuestGoal();
                    newQuest.questGoal.questType = quest.questType;
                    newQuest.questGoal.goalIndex = quest.goalIndex;
                    newQuest.questGoal.goalAmount = quest.goalAmount;
                    newQuest.questGoal.currentAmount = 0;

                    newQuest.rewardGold = quest.rewardGold;
                    newQuest.rewardExp = quest.rewardExp;
                    newQuest.rewardItem = quest.rewardItem;

                    newQuest.questState = QuestState.Ready;
                    

                    result.Add(newQuest);                }
            }

            return result;
        }

        public override void DoAction()
        {
            if(quests.Count == 0)
            {
                Debug.Log("��� ����Ʈ Ŭ����");
                return;
            }

            QuestManager.Instance.currentQuest = quests[0];

            switch (quests[0].questState)
            {
                case QuestState.Ready:
                    UIManager.Instance.OpenDialogUI(quests[0].dialogIndex, npc.npcType);
                    break;
                case QuestState.Accept:
                    UIManager.Instance.OpenDialogUI(quests[0].dialogIndex + 1, npc.npcType);
                    break;
                case QuestState.Complete:
                    UIManager.Instance.OpenDialogUI(quests[0].dialogIndex + 2, npc.npcType);
                    break;
            }
        }

        

    }
}