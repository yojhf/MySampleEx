using UnityEngine;
using System;
using System.Linq;


namespace MySampleEx
{
    // 아이템 슬롯들을 가지고 있는 클래스
    [Serializable]
    public class Inventory
    {
        public ItemSlot[] slots = new ItemSlot[0];

        // 인벤토리 슬롯 초기화
        public void Clear()
        {
            foreach (var slot in slots)
            {
                // 빈 슬롯 만들기
                slot.UpdateSlot(new Item(), 0);
            }
        }

        // 아이템 찾기 : ItemObject
        public bool IsContain(ItemObject itemObject)
        {
            return IsContain(itemObject.data.id);

            //return Array.Find(slots, i => i.item.id == itemObject.data.id) != null;
        }

        // 아이템 찾기 : id
        public bool IsContain(int id)
        {
            return slots.FirstOrDefault(i => i.item.id == id) != null;
        }
    }
}