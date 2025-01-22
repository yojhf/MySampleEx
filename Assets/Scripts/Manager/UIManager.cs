using UnityEngine;

namespace MySampleEx
{
    public class UIManager : MonoBehaviour
    {
        public ItemDataBase dataBase;
        public DynamicInventoryUI playerInventoryUI;
        public StaticInventoryUI playerEquipmentUI;
        public PlayerStatesUI playerStatesUI;
        public DialogUI dialogUI;

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

        public void OpenDialogUI(int _dialogIndex)
        {
            Toggle(dialogUI.gameObject);
            dialogUI.StartDialog(_dialogIndex);

        }

        public void CloseDialogUI()
        {
            Toggle(dialogUI.gameObject);
        }
    }
}