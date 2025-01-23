using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace MySampleEx
{
    // (퀘스트 목록), 퀘스트 진행창, 퀘스트 정보창 
    public class QuestUI : MonoBehaviour
    {
        // 퀘스트 정보창에 보이는 퀘스트
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

        // 퀘스트창 종료 시 실행된 이벤트
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

            // 버튼 셋팅
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

        // 플레이어 퀘스트 보기
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

        // NPC 퀘스트 보기
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
            // 플레이어에게 퀘스트 리스트에 currentQuest 추가
            questManager.AddPlayerQuest();
            
            CloseQuestUI();
        }
        public void GiveUpQuest()
        {
            // 플레이어에게 퀘스트 리스트에 currentQuest 제거
            questManager.GiveupPlayerQuest();

            CloseQuestUI();
        }
    }
}