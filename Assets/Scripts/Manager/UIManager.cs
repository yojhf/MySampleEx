using UnityEngine;

namespace MySampleEx
{
    public class UIManager : MonoBehaviour
    {
        public ItemDataBase dataBase;
        public DynamicInventoryUI playerInventoryUI;
        public StaticInventoryUI playerEquipmentUI;

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
                playerInventoryUI.gameObject.SetActive(!playerInventoryUI.gameObject.activeSelf);

                // 커서

            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerEquipmentUI.gameObject.SetActive(!playerEquipmentUI.gameObject.activeSelf);

                // 커서

            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                AddNewItem();
            }
        }

        public void AddNewItem()
        { 
            ItemObject itemObject = dataBase.itemObjects[itemId];
            Item newItem = itemObject.CreateItem();

            playerInventoryUI.inventoryObject.AddItem(newItem, 1);
        }
    }
}