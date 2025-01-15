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

        // 슬롯에 넣을 수 있는 아이템 타입 설정
        public ItemType[] AllowedItems = new ItemType[0];

        [NonSerialized]
        public InventoryObject parent;
        // 슬롯이 적용되는 UI 오브젝트
        [NonSerialized]
        public GameObject slotUI;

        // 슬롯에 아이템 내용이 적용되기 이전에 등록된 함수 호출
        [NonSerialized]
        public Action<ItemSlot> OnPreUpdate;
        // 슬롯에 아이템 내용이 적용된 후에 등록된 함수 호출
        [NonSerialized]
        public Action<ItemSlot> OnPostUpdate;

        // item의 정보를 가지고 있는 ItemObject
        public ItemObject ItemObject
        {
            get
            {
                return item.id >= 0 ? parent.dataBase.itemObjects[item.id] : null;
            }
        }

        // 생성자
        public ItemSlot()
        {
            // 빈 슬롯(아이템이 없는 슬롯)
            UpdateSlot(new Item(), 0);
        }
        public ItemSlot(Item _item, int _amount)
        {
            // 매개변수로 들어온 아이템을 가진 슬롯 생성
            UpdateSlot(_item, _amount);
        }

        // 슬롯 업데이트
        public void UpdateSlot(Item _item, int _amount)
        {
            // 빈슬롯 체크
            if(_amount == 0)
            {
                _item = new Item();
            }

            OnPreUpdate?.Invoke(this);

            item = _item;
            amount = _amount;

            OnPostUpdate?.Invoke(this);          
        }

        // 슬롯에서 아이템 삭제
        public void RemoveItem()
        {
            UpdateSlot(new Item(), 0);
        }

        // 아이템 수량 추가
        public void AddItemAmount(int value)
        {
            int addValue = amount + value;

            UpdateSlot(item, addValue);
        }

        // 슬롯에 아이템 장착이 가능여부 판단
        public bool CanPlaceInSlot(ItemObject itemObject)
        {
            if (AllowedItems.Length <= 0 || itemObject == null || itemObject.data.id <= -1)
                return true;

            // AllowedItems 체크
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