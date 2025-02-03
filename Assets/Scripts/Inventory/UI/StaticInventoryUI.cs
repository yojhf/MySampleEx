using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MySampleEx
{
    public class StaticInventoryUI : InventoryUI
    {
        public GameObject[] staticSlots;

        public GameObject unEquipButton;

        public override void CreateSlots()
        {
            slotUIs = new Dictionary<GameObject, ItemSlot>();

            for (int i = 0; i < inventoryObject.Slots.Length; i++)
            {
                GameObject slotGo = staticSlots[i];

                // 생성된 슬롯 오브젝트의 이벤트 트리거 이벤트 등록
                AddEvent(slotGo, EventTriggerType.PointerEnter, delegate { OnEnter(slotGo); });
                AddEvent(slotGo, EventTriggerType.PointerExit, delegate { OnExit(slotGo); });
                AddEvent(slotGo, EventTriggerType.BeginDrag, delegate { OnStartDrag(slotGo); });
                AddEvent(slotGo, EventTriggerType.Drag, delegate { OnDrag(slotGo); });
                AddEvent(slotGo, EventTriggerType.EndDrag, delegate { OnEndDrag(slotGo); });
                AddEvent(slotGo, EventTriggerType.PointerClick, delegate { OnClick(slotGo); });

                inventoryObject.Slots[i].slotUI = slotGo;
                slotUIs.Add(slotGo, inventoryObject.Slots[i]);
            }
        }

        public void UnEquip()
        {
            if (selectSlotObject == null)
                return;

            if (UIManager.Instance.AddItemInventory(slotUIs[selectSlotObject].item, 1))
            {
                slotUIs[selectSlotObject].RemoveItem();

                UpdateSelectSlot(null);
            }
        }

        public override void UpdateSelectSlot(GameObject go)
        {
            base.UpdateSelectSlot(go);

            if (selectSlotObject == null)
            {
                unEquipButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                unEquipButton.GetComponent<Button>().interactable = true;
            }
        }


    }
}