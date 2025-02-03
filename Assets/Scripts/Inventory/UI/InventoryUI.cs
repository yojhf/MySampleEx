using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MySampleEx
{
    // �κ��丮 UI �θ� (�߻�)Ŭ����
    [RequireComponent(typeof(EventTrigger))]
    public abstract class InventoryUI : MonoBehaviour
    {
        public InventoryObject inventoryObject;
        public Dictionary<GameObject, ItemSlot> slotUIs = new Dictionary<GameObject, ItemSlot>();

        // ���� ���� ��� - ���� ���õ� ����
        protected GameObject selectSlotObject = null;
        public Action<GameObject> OnUpdateSelectSlot;

        private void Awake()
        {
            CreateSlots();
            Init();
        }

        private void Start()
        {
            // UI ���� ����
            for (int i = 0; i < inventoryObject.Slots.Length; i++)
            {
                inventoryObject.Slots[i].UpdateSlot(inventoryObject.Slots[i].item, inventoryObject.Slots[i].amount);
            }

        }
        public abstract void CreateSlots();

        void Init()
        {
            // inventoryObject Slots ����
            for (int i = 0; i < inventoryObject.Slots.Length; i++)
            {
                inventoryObject.Slots[i].parent = inventoryObject;
                // ������ ���� ���� �� ȣ��Ǵ� UI �Լ� ���
                inventoryObject.Slots[i].OnPostUpdate += OnPostUpdate;
            }

            // �̺�Ʈ Ʈ���� �̺�Ʈ ���
            AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
            AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
        }

        public void OnPostUpdate(ItemSlot itemSlot)
        {
            // ������ ���� üũ
            if (itemSlot == null || itemSlot.slotUI == null)
                return;

            itemSlot.slotUI.transform.GetChild(0).GetComponent<Image>().sprite = 
                itemSlot.item.id < 0 ? null : itemSlot.ItemObject.icon;

            itemSlot.slotUI.transform.GetChild(0).GetComponent<Image>().color =
                itemSlot.item.id < 0 ? new Color(1f, 1f, 1f, 0f) : new Color(1f, 1f, 1f, 1f);

            itemSlot.slotUI.GetComponentInChildren<TextMeshProUGUI>().text = 
                itemSlot.item.id < 0 ? string.Empty : (itemSlot.amount == 1) ? string.Empty :  itemSlot.amount.ToString();
        }

        // �̺�Ʈ Ʈ���� �̺�Ʈ ���
        protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action) 
        {
            EventTrigger trigger = go.GetComponent<EventTrigger>();

            if (trigger == null)
            {
                Debug.Log("No EventTrigger");
                return;
            }

            EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };

            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }

        // ���� ������Ʈ�� ���콺�� ���� ȣ��
        public void OnEnterInterface(GameObject gameObject)
        {
            MouseData.interfaceMouseIsOver = gameObject.GetComponent<InventoryUI>();
        }

        // ���� ������Ʈ�� ���콺�� ������ ȣ��
        public void OnExitInterface(GameObject gameObject)
        {
            MouseData.interfaceMouseIsOver = null;
        }

        // ���� ������Ʈ�� ���콺�� ���� ȣ��
        public void OnEnter(GameObject go)
        {
            MouseData.slotHoverdOver = go;
            MouseData.interfaceMouseIsOver = GetComponent<InventoryUI>();
        }

        // ���� ������Ʈ�� ���콺�� ���� ȣ��
        public void OnExit(GameObject go)
        {
            MouseData.slotHoverdOver = null;
        }

        // ���� ������Ʈ�� ���콺�� �巡�� ���� �� �� ȣ��
        public void OnStartDrag(GameObject go)
        {
            MouseData.tempItemBeginDragged = CreateDragImage(go);

            UpdateSelectSlot(null);
        }

        // ���콺 �巡�� ���� �� �� ���콺 ���̆��� �ް� �ٴϴ� �̹��� ������Ʈ ����
        GameObject CreateDragImage(GameObject go) 
        {
            if (slotUIs[go].item.id <= -1)
            {
                return null;
            }

            GameObject dragImage = new GameObject();

            RectTransform rectTransform = dragImage.AddComponent<RectTransform>();

            rectTransform.sizeDelta = new Vector2(50f, 50f);
            dragImage.transform.SetParent(transform.parent);

            Image image = dragImage.AddComponent<Image>();
            image.sprite = slotUIs[go].ItemObject.icon;
            image.raycastTarget = false;

            dragImage.name = "Drage Image";

            return dragImage;
        }

        // ���콺 �巡�� �� ���콺 �����Ϳ� �ް� �ٴϴ� �̹��� ��ġ�� ���콺 ������ ����
        public void OnDrag(GameObject go)
        {
            if (MouseData.tempItemBeginDragged == null)
                return;

            MouseData.tempItemBeginDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }

        // ���� ������Ʈ�� ���콺�� �巡�� ���� �� ȣ��
        public void OnEndDrag(GameObject go)
        {
            Destroy(MouseData.tempItemBeginDragged);

            // ���콺�� ��ġ�� �κ��丮 UI �ۿ� ���� ���
            if(MouseData.interfaceMouseIsOver == null)
            {
                // ������ ������
                slotUIs[go].RemoveItem();
            }
            else if(MouseData.interfaceMouseIsOver != null) // ���콺�� ��ġ�� ���� ���� ������Ʈ ���� ���� ���
            {
                // ������ �ٲٱ�
                // ���콺�� ��ġ�� ���� ������Ʈ�� ����
                ItemSlot mouseHoverSlot = MouseData.interfaceMouseIsOver.slotUIs[MouseData.slotHoverdOver];

                inventoryObject.SwapItems(slotUIs[go], mouseHoverSlot);
            }
        }

        // ���� ������Ʈ�� ���콺�� Ŭ���� �� ȣ��
        public void OnClick(GameObject go)
        {
            OnUpdateSelectSlot?.Invoke(null);

            ItemSlot slot = slotUIs[go];

            // ������ ���� ���� üũ
            if(slot.item.id >= 0)
            {
                // ���õǾ� �ִ� ���� �ٽ� ����
                if(selectSlotObject == go)
                {
                    UpdateSelectSlot(null);
                }
                else
                {
                    UpdateSelectSlot(go);
                }

            }

        }

        public virtual void UpdateSelectSlot(GameObject go)
        {
            selectSlotObject = go;

            foreach(var slot in slotUIs)
            {
                if(slot.Key == go)
                {
                    slot.Value.slotUI.transform.GetChild(1).GetComponent<Image>().enabled = true;
                }
                else
                {
                    slot.Value.slotUI.transform.GetChild(1).GetComponent<Image>().enabled = false;
                }
            }
        }

        public virtual void CloseInventoryUI()
        {

        }
    }
}