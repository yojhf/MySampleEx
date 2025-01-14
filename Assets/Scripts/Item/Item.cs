using UnityEngine;
using System;

namespace MySampleEx
{
    // Item(id, name)
    // itemBuffs ������ �ɷ�ġ
    [Serializable]
    public class Item
    {
        public int id;
        public string name;

        public ItemBuff[] buffs;

        // ������
        public Item()
        {
            id = -1;
            name = "";
        }

        // ������ - �Ű������� ItemObject�� �޾Ƽ� ������ ������ ����
        public Item(ItemObject itemObject)
        {
            name = itemObject.name;
            id = itemObject.data.id;

            buffs = new ItemBuff[itemObject.data.buffs.Length];

            for (int i = 0; i < buffs.Length; i++)
            {
                buffs[i] = new ItemBuff(itemObject.data.buffs[i].Min, itemObject.data.buffs[i].Max)
                {
                    stat = itemObject.data.buffs[i].stat
                };
            }
        }
    }

}