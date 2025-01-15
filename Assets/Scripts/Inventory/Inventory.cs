using UnityEngine;
using System;
using System.Linq;


namespace MySampleEx
{
    // ������ ���Ե��� ������ �ִ� Ŭ����
    [Serializable]
    public class Inventory
    {
        public ItemSlot[] slots = new ItemSlot[0];

        // �κ��丮 ���� �ʱ�ȭ
        public void Clear()
        {
            foreach (var slot in slots)
            {
                // �� ���� �����
                slot.UpdateSlot(new Item(), 0);
            }
        }

        // ������ ã�� : ItemObject
        public bool IsContain(ItemObject itemObject)
        {
            return IsContain(itemObject.data.id);

            //return Array.Find(slots, i => i.item.id == itemObject.data.id) != null;
        }

        // ������ ã�� : id
        public bool IsContain(int id)
        {
            return slots.FirstOrDefault(i => i.item.id == id) != null;
        }
    }
}