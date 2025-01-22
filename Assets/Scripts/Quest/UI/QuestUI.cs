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
        public Action OnCloseQuest;

        void SetQuestUI(Quest _quest)
        {
            quest = _quest;

            nameText.text = quest.name;
            descriptionText.text = quest.description;
            Debug.Log(descriptionText.text);
            goalText.text = quest.questGoal.currentAmount.ToString() + " / " + quest.questGoal.goalAmount.ToString();
            
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
            switch (quest.questState)
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

        public void OpenQuestUI()
        {
            if(QuestManager.Instance.currentQuest == null)
            {
                // 
                return;
            }

            SetQuestUI(QuestManager.Instance.currentQuest);

        }
    }
}