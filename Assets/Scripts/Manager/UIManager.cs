using MySample;
using UnityEngine;

namespace MySampleEx
{
    public class UIManager : Singleton<UIManager>
    {
        public ItemDataBase dataBase;
        public DynamicInventoryUI playerInventoryUI;
        public StaticInventoryUI playerEquipmentUI;
        public PlayerStatesUI playerStatesUI;
        public DialogUI dialogUI;
        public QuestUI questUI;

        public int itemId = 0;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.I))
            {  
                Toggle(playerInventoryUI.gameObject);

                // 커서
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                Toggle(playerEquipmentUI.gameObject);

                // 커서
            }
            if(Input.GetKeyDown(KeyCode.O))
            {
                Toggle(playerStatesUI.gameObject);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                AddNewItem();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                OpenPlayerQuestUI();
            }

        }

        public void Toggle(GameObject go)
        {
            go.SetActive(!go.activeSelf);
        }

        public void AddNewItem()
        { 
            ItemObject itemObject = dataBase.itemObjects[itemId];
            Item newItem = itemObject.CreateItem();

            playerInventoryUI.inventoryObject.AddItem(newItem, 1);
        }

        public void OpenDialogUI(int _dialogIndex, NpcType npcType = NpcType.None)
        {
            Toggle(dialogUI.gameObject);

            dialogUI.OnCloseDialog += CloseDialogUI;

            if(npcType == NpcType.QuestGiver)
            {
                // QuestUI 열기
                dialogUI.OnCloseDialog += OpenQuestUI;
            }

            dialogUI.StartDialog(_dialogIndex);

        }

        public void CloseDialogUI()
        {
            Toggle(dialogUI.gameObject);
        }

        // 플레이어 퀘스트 보기 (퀘스트 리스트)
        public void OpenPlayerQuestUI()
        {
            Toggle(questUI.gameObject);

            if (questUI.gameObject.activeSelf)
            {
                questUI.OpenPlayerQuestUI(CloseQuestUI);
            }
            
        }

        public void OpenQuestUI()
        {
            Toggle(questUI.gameObject);
            questUI.OpenQuestUI(CloseQuestUI);
        }

        public void CloseQuestUI()
        {
            Toggle(questUI.gameObject);
        }
    }
}