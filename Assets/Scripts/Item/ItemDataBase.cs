using System.Collections.Generic;
using UnityEngine;

namespace MySampleEx
{
    // 아이템 정보를 가지고 있는 오브젝트들을 모아놓은 ScriptableObject
    [CreateAssetMenu(fileName = "New ItemDataBase", menuName = "Inventory System/Item/New ItemDataBase")]
    public class ItemDataBase : ScriptableObject
    {
        public List<ItemObject> itemObjects = new List<ItemObject>();

        // 인스펙터창에서 값을 저장할 때 마다 호출되는 함수
        // itemObjects에 있는 item의 id 값을 설정
        private void OnValidate()
        {
            for (int i = 0; i < itemObjects.Count; i++)
            {
                if (itemObjects[i] == null)
                {
                    continue;
                }

                itemObjects[i].data.id = i;
            }
        }
    }
}