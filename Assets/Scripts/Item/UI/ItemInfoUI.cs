using TMPro;
using UnityEngine;

namespace MySampleEx
{
    // ������ ����â
    public class ItemInfoUI : MonoBehaviour
    {
        public TMP_Text itemName;
        public TMP_Text itemDescription;


        // ������ �Ӽ� �ؽ�Ʈ
        

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetItemInfoUI(ItemSlot _itemSlot)
        {
            ItemObject itemObject = _itemSlot.ItemObject;

            itemName.text = itemObject.name ;
            itemDescription.text = itemObject.description;
        }
    }
}