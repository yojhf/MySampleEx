using UnityEngine;

namespace MySampleEx
{
    public class UIManager : MonoBehaviour
    {
        public ItemDataBase dataBase;
        public DynamicInventoryUI playerInventoryUI;

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
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                AddNewItem();
            }
        }

        public void AddNewItem()
        { 
            ItemObject itemObject = dataBase.itemObjects[1];
            Item newItem = itemObject.CreateItem();

            playerInventoryUI.inventoryObject.AddItem(newItem, 1);
        }
    }
}