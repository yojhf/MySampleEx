using System.Linq;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor.Overlays;
using System;

namespace MySampleEx
{
    // Inventory ������ �����̳ʸ� ������ �ִ� ScriptableObject
    [CreateAssetMenu(fileName = "New InventoryObject", menuName = "Inventory System/Inventory/New Inventory")]
    public class InventoryObject : ScriptableObject
    {
        // ������ ������ ������ �ִ� ScriptableObject
        public ItemDataBase dataBase;
        // �κ��丮 Ÿ��
        public InterfaceType type;

        public Inventory container = new Inventory();

        // �κ�Ʈ�� ���� �б� ����
        public ItemSlot[] Slots => container.slots;

        // ������ ���� ��ϵ� �޼��� ȣ��
        public Action<ItemObject> OnUseItem;

        // ���� �� ���� ����
        public int EmptySlotCount
        {
            get 
            {
                int count = 0;

                foreach (var slot in Slots)
                {
                    if (slot.item.id <= -1)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        // �κ��丮 ������ �߰�
        public bool AddItem(Item item, int amount)
        {
            ItemSlot slot = FindItemInInventory(item);

            if (!dataBase.itemObjects[item.id].stackable || slot == null)
            {
                // �κ�Ǯ üũ
                if(EmptySlotCount <= 0)
                {
                    Debug.Log("Full : " + EmptySlotCount);

                    return false;
                }

                // �� ���� ������ �߰�
                ItemSlot emptySlot = GetEmptySlot();

                emptySlot.UpdateSlot(item, amount);
            }
            else
            { 
                // �������� ���� ���Կ� amount ����
                slot.AddItemAmount(amount);
            }

            return true;
        }

        // �Ű������� ���� �������� ���� ���� ã��
        public ItemSlot FindItemInInventory(Item item)
        {
            return Slots.FirstOrDefault(i => i.item.id == item.id);
        }

        // �� ���� ã��
        public ItemSlot GetEmptySlot()
        {
            return Slots.FirstOrDefault(i => i.item.id <= -1);
        }

        // ������ �ٲٱ�
        public void SwapItems(ItemSlot itemA, ItemSlot itemB)
        {
            if (itemA == itemB)
                return;

            if(itemB.CanPlaceInSlot(itemA.ItemObject) && itemA.CanPlaceInSlot(itemB.ItemObject))
            {
                ItemSlot temp = new ItemSlot(itemB.item, itemB.amount);

                itemB.UpdateSlot(itemA.item, itemA.amount);
                itemA.UpdateSlot(temp.item, temp.amount);
            }
        }

        // �κ��丮 ������ �����ϱ�, �ҷ�����
        #region Save / Load Methods
        public string savePath = "/Inventory.json";

        [ContextMenu("Save")]
        public void Save()
        {
            string path = Application.persistentDataPath + savePath;
            // ������ 2��ȭ �غ�
            BinaryFormatter bf = new BinaryFormatter();
            // ������ ���Ͽ� ����
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            // ������ �����͸� �غ�
            string saveData = JsonUtility.ToJson(container, true);

            // �����͸� ���Ͽ� ����
            bf.Serialize(fs, saveData);
            // ���� �ݱ�
            fs.Close();

            Debug.Log(path);
            Debug.Log(saveData);
        }

        [ContextMenu("Load")]
        public void Load()
        {
            string path = Application.persistentDataPath + savePath;

            if(File.Exists(path))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                string loadData = bf.Deserialize(fs).ToString();

                JsonUtility.FromJsonOverwrite(loadData, container);
                fs.Close();

                Debug.Log(loadData);
            }
        }

        [ContextMenu("Clear")]
        public void Clear()
        { 
            container.Clear();
        }
        #endregion
    }
}