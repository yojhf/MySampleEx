using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MySampleEx
{
    public class DynamicInventoryUI : InventoryUI
    {
        public GameObject slotPrefab;
        
        public ItemInfoUI itemInfoUI;

        public override void CreateSlots()
        {
            slotUIs = new Dictionary<GameObject, ItemSlot>();

            for (int i = 0; i < inventoryObject.Slots.Length; i++)
            {
                GameObject go = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform.GetChild(2));

                // ������ ���� ������Ʈ�� �̺�Ʈ Ʈ���� �̺�Ʈ ���
                AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnter(go); });
                AddEvent(go, EventTriggerType.PointerExit, delegate { OnExit(go); });
                AddEvent(go, EventTriggerType.BeginDrag, delegate { OnStartDrag(go); });
                AddEvent(go, EventTriggerType.Drag, delegate { OnDrag(go); });
                AddEvent(go, EventTriggerType.EndDrag, delegate { OnEndDrag(go); });
                AddEvent(go, EventTriggerType.PointerClick, delegate { OnClick(go); });
                

                // 
                inventoryObject.Slots[i].slotUI = go;
                slotUIs.Add(go, inventoryObject.Slots[i]);
                go.name += " : " + i.ToString();
            }
        }

        public override void UpdateSelectSlot(GameObject go)
        {
            base.UpdateSelectSlot(go);

            if (selectSlotObject == null)
            {
                itemInfoUI.gameObject.SetActive(false);
            }
            else
            {
                itemInfoUI.gameObject.SetActive(true);
                itemInfoUI.SetItemInfoUI(slotUIs[selectSlotObject]);
            }

        }

        public void UseItem()
        {
            if (selectSlotObject == null)
                return;

            // �Ҹ�ǰ, �������� - ����

            inventoryObject.UseItem(slotUIs[selectSlotObject]);
            UpdateSelectSlot(null);
        }

        public void SellItem()
        {
            if (selectSlotObject == null)
                return;

            // ������ �Ǹ� ��� (���Ŵ���� �ݰ�)
            int sellPrice = slotUIs[selectSlotObject].ItemObject.shopPrice / 2;
            UIManager.Instance.AddGold(sellPrice);
            

            slotUIs[selectSlotObject].RemoveItem();
            UpdateSelectSlot(null);
        }

    }
}