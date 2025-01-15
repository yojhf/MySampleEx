using UnityEngine;

namespace MySampleEx
{
    // 아이템 기본 정보를 저장하는 ScriptableObject
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Item/New Item")]
    public class ItemObject : ScriptableObject
    {
        public Item data = new Item();

        public ItemType type;
        // 슬롯에 누적 여부
        public bool stackable;

        public Sprite icon;
        public GameObject modelPrefab;
        [TextArea(15, 20)]
        public string description;

        public Item CreateItem()
        { 
            Item newItem = new Item(this);

            return newItem;
        }
    }
}