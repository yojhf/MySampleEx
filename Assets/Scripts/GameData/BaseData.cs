using UnityEngine;

namespace MySampleEx
{
    // Data �⺻ Ŭ����
    // ���� ������ : �̸�, ���
    // �������� ��� : �������� ���� ��������, �̸� ��� ����Ʈ ������, ������ �߰�, ����, ����
    public class BaseData : ScriptableObject
    {
        public string[] names;
        public const string dataDirectory = "Data/";

        // ������
        public BaseData() { }

        // �������� ���� ��������
        public int GetDataCount()
        {
            if (names == null)
                return 0;

            return names.Length;
        }

        // ���� ����ϱ� ���� �̸� ��� ����Ʈ ������
        public string[] GetNameList(bool showId, string filterWord = "")
        {
            int lenth = GetDataCount();

            string[] retList = new string[lenth];

            for (int i = 0; i < lenth; i++)
            {
                if (filterWord != "")
                {
                    if (names[i].ToLower().Contains(filterWord.ToLower()))
                    {
                        continue;
                    }
                }

                if (showId)
                {
                    retList[i] = i.ToString() + " : " + names[i];
                }
                else
                { 
                    retList[i] = names[i];
                }
            }

            return retList;
        }

        // ������ �߰� �� ���� ���� ����
        public virtual int AddData(string newName)
        { 


            return GetDataCount();
        }

        // ������ ����
        public virtual void CopyData(int index)
        {
            
        }

        // ������ ����
        public virtual void RemoveData(int index)
        {

        }
    }
}