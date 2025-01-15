using System.Collections.Generic;
using UnityEngine;

namespace MySampleEx
{
    // ������ ������ ������ �ִ� ������Ʈ���� ��Ƴ��� ScriptableObject
    [CreateAssetMenu(fileName = "New ItemDataBase", menuName = "Inventory System/Item/New ItemDataBase")]
    public class ItemDataBase : ScriptableObject
    {
        public List<ItemObject> itemObjects = new List<ItemObject>();

        // �ν�����â���� ���� ������ �� ���� ȣ��Ǵ� �Լ�
        // itemObjects�� �ִ� item�� id ���� ����
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