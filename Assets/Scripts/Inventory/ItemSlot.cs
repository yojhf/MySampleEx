using UnityEngine;
using System;
using Unity.VisualScripting;

namespace MySampleEx
{
    [Serializable]
    public class ItemSlot
    {
        public Item item;
        public int amount;

        // ���Կ� ���� �� �ִ� ������ Ÿ�� ����
        public ItemType[] AllowedItems = new ItemType[0];

        [NonSerialized]
        public InventoryObject parent;
        // ������ ����Ǵ� UI ������Ʈ
        [NonSerialized]
        public GameObject slotUI;

        // ���Կ� ������ ������ ����Ǳ� ������ ��ϵ� �Լ� ȣ��
        [NonSerialized]
        public Action<ItemSlot> OnPreUpdate;
        // ���Կ� ������ ������ ����� �Ŀ� ��ϵ� �Լ� ȣ��
        [NonSerialized]
        public Action<ItemSlot> OnPostUpdate;

        // item�� ������ ������ �ִ� ItemObject
        public ItemObject ItemObject
        {
            get
            {
                return item.id >= 0 ? parent.dataBase.itemObjects[item.id] : null;
            }
        }

        // ������
        public ItemSlot()
        {
            // �� ����(�������� ���� ����)
            UpdateSlot(new Item(), 0);
        }
        public ItemSlot(Item _item, int _amount)
        {
            // �Ű������� ���� �������� ���� ���� ����
            UpdateSlot(_item, _amount);
        }

        // ���� ������Ʈ
        public void UpdateSlot(Item _item, int _amount)
        {
            // �󽽷� üũ
            if(_amount == 0)
            {
                _item = new Item();
            }

            OnPreUpdate?.Invoke(this);

            item = _item;
            amount = _amount;

            OnPostUpdate?.Invoke(this);          
        }

        // ���Կ��� ������ ����
        public void RemoveItem()
        {
            UpdateSlot(new Item(), 0);
        }

        // ������ ���� �߰�
        public void AddItemAmount(int value)
        {
            int addValue = amount + value;

            UpdateSlot(item, addValue);
        }

        // ���Կ� ������ ������ ���ɿ��� �Ǵ�
        public bool CanPlaceInSlot(ItemObject itemObject)
        {
            if (AllowedItems.Length <= 0 || itemObject == null || itemObject.data.id <= -1)
                return true;

            // AllowedItems üũ
            foreach (var itemType in AllowedItems)
            {
                if(itemObject.type == itemType)
                {
                    return true;
                }
            }

            return false;
        }
    }
}