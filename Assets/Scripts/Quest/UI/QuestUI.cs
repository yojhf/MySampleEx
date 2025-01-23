using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace MySampleEx
{
    // (����Ʈ ���), ����Ʈ ����â, ����Ʈ ����â 
    public class QuestUI : MonoBehaviour
    {
        // ����Ʈ ����â�� ���̴� ����Ʈ
        private Quest quest;

        public TMP_Text nameText;
        public TMP_Text descriptionText;
        public TMP_Text goalText;
        public TMP_Text rewardGoldText;
        public TMP_Text rewardExpText;
        public TMP_Text rewardItemText;
        public Image itemImage;

        public GameObject acceptButton;
        public GameObject giveupButton;
        public GameObject okButton;

        // ����Ʈâ ���� �� ����� �̺�Ʈ
        public Action OnCloseQuestUI;

        QuestManager questManager;

        private void OnEnable()
        {
            if(questManager == null)
            {
                questManager = QuestManager.Instance;
            }

            OnCloseQuestUI = null;
        }

        void SetQuestUI(QuestObject _questObj)
        {
            Quest quest = DataManager.GetQuestData().questList.quests[_questObj.number];

            nameText.text = quest.name;

            if(_questObj.questState == QuestState.Complete)
            {
                descriptionText.text = "Quest Completed";
            }
            else
            {
                descriptionText.text = quest.description;
            }

            goalText.text = _questObj.questGoal.currentAmount.ToString() + " / " + _questObj.questGoal.goalAmount.ToString();
            
            rewardGoldText.text = quest.rewardGold.ToString();
            rewardExpText.text = quest.rewardExp.ToString();
            
            if(quest.rewardItem >= 0)
            {
                rewardItemText.text = UIManager.Instance.dataBase.itemObjects[quest.rewardItem].name;
                itemImage.sprite = UIManager.Instance.dataBase.itemObjects[quest.rewardItem].icon;
                itemImage.enabled = true;
            }
            else
            {
                rewardItemText.text = "";
                itemImage.sprite = null;
                itemImage.enabled = false;
            }

            ResetBtn();

            // ��ư ����
            switch (_questObj.questState)
            {
                case QuestState.Ready:
                    acceptButton.gameObject.SetActive(true);
                    break;
                case QuestState.Accept:
                    giveupButton.gameObject.SetActive(true);
                    break;
                case QuestState.Complete:
                    okButton.gameObject.SetActive(true);
                    break;
            }

        }

        void ResetBtn()
        {
            acceptButton.gameObject.SetActive(false);
            giveupButton.gameObject.SetActive(false);
            okButton.gameObject.SetActive(false);
        }

        // �÷��̾� ����Ʈ ����
        public void OpenPlayerQuestUI(Action _closeMethod)
        {
            if (_closeMethod != null)
            {
                OnCloseQuestUI += _closeMethod;
            }

            if (questManager.playerQuests.Count <= 0)
            {
                CloseQuestUI();
                return;
            }

            questManager.SetCurrentQuest(questManager.playerQuests[0]);
            SetQuestUI(questManager.currentQuest);
        }

        // NPC ����Ʈ ����
        public void OpenQuestUI(Action _closeMethod)
        {
            if(_closeMethod != null)
            {
                OnCloseQuestUI += _closeMethod;
            }

            if (questManager.currentQuest == null)
            {
                CloseQuestUI();
                return;
            }

            SetQuestUI(questManager.currentQuest);
        }

        public void CloseQuestUI()
        {
            OnCloseQuestUI?.Invoke();
            ResetBtn();
        }

        public void AcceptQuest()
        {
            // �÷��̾�� ����Ʈ ����Ʈ�� currentQuest �߰�
            questManager.AddPlayerQuest();
            
            CloseQuestUI();
        }
        public void GiveUpQuest()
        {
            // �÷��̾�� ����Ʈ ����Ʈ�� currentQuest ����
            questManager.GiveupPlayerQuest();

            CloseQuestUI();
        }
    }
}