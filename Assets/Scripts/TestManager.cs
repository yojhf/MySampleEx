using UnityEngine;

namespace MySampleEx
{
    public class TestManager : MonoBehaviour
    {
        public ItemDataBase dataBase;
        public InventoryObject inventoryObject;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Item newItem = dataBase.itemObjects[0].CreateItem();

            inventoryObject.AddItem(newItem, 1);
        }
    }
}