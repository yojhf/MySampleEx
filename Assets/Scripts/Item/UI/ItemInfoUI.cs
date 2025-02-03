using TMPro;
using UnityEngine;

namespace MySampleEx
{
    // 아이템 정보창
    public class ItemInfoUI : MonoBehaviour
    {
        public TMP_Text itemName;
        public TMP_Text itemDescription;


        // 아이템 속성 텍스트
        

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