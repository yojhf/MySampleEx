using System.Collections.Generic;
using UnityEngine;

namespace MySampleEx
{
    // ������ ������ ������ �ִ� ������Ʈ���� ��Ƴ��� ScriptableObject
    [CreateAssetMenu(fileName = "New ItemDataBase", menuName = "Inventory System/Item/New ItemDataBase")]
    public class ItemDataBase : ScriptableObject
    {
        public ItemObject[] itemObjects;

        // �ν�����â���� ���� ������ �� ���� ȣ��Ǵ� �Լ�
        // itemObjects�� �ִ� item�� id ���� ����
        private void OnValidate()
        {
            for (int i = 0; i < itemObjects.Length; i++)
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