using TMPro;
using UnityEngine;

namespace MySampleEx
{
    public class PlayerStatesUI : MonoBehaviour
    {
        public InventoryObject equipment;
        public StatsObject stats;

        public TMP_Text[] attributeTexts;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            if (equipment != null && stats != null)
            {
                foreach (var slot in equipment.Slots)
                {
                    slot.OnPreUpdate += OnRemoveItem;
                    slot.OnPostUpdate += OnEquipItem;
                }
            }
        }

        private void OnEnable()
        {
            stats.OnChangedStats += OnChangedStats;
            UpdateattributeText();
        }

        private void OnDisable()
        {
            stats.OnChangedStats -= OnChangedStats;
        }

        void UpdateattributeText()
        {
            attributeTexts[0].text = stats.GetModifiredValue(CharacterAttributes.Agility).ToString();
            attributeTexts[1].text = stats.GetModifiredValue(CharacterAttributes.Intellect).ToString();
            attributeTexts[2].text = stats.GetModifiredValue(CharacterAttributes.Stamina).ToString();
            attributeTexts[3].text = stats.GetModifiredValue(CharacterAttributes.Strength).ToString();
        }

        private void OnChangedStats(StatsObject _object)
        {
            UpdateattributeText();
        }

        void OnRemoveItem(ItemSlot _itemSlot)
        {
            if (_itemSlot.ItemObject == null)
                return;

            Debug.Log("OnRemoveItem");

            if(_itemSlot.parent.type == InterfaceType.Equipment)
            {
                foreach(var buff in _itemSlot.item.buffs)
                {
                    foreach(var attribute in stats.attributes)
                    {
                        if(attribute.type == buff.stat)
                        {
                            attribute.value.RemoveModifer(buff);
                        }
                    }
                }
            }
        }

        void OnEquipItem(ItemSlot _itemSlot)
        {
            if (_itemSlot.ItemObject == null)
                return;

            Debug.Log("OnEquipItem");

            if (_itemSlot.parent.type == InterfaceType.Equipment)
            {
                foreach (var buff in _itemSlot.item.buffs)
                {
                    foreach (var attribute in stats.attributes)
                    {
                        if (attribute.type == buff.stat)
                        {
                            attribute.value.AddModifer(buff);
                        }
                    }
                }
            }
        }
    }
}