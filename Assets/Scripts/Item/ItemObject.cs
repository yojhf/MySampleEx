using UnityEngine;

namespace MySampleEx
{
    // ������ �⺻ ������ �����ϴ� ScriptableObject
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Item/New Item")]
    public class ItemObject : ScriptableObject
    {
        public Item data = new Item();

        public ItemType type;
        // ���Կ� ���� ����
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