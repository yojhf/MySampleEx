using UnityEngine;

namespace MySampleEx
{
    // Data 기본 클래스
    // 공통 데이터 : 이름, 목록
    // 공통적인 기능 : 데이터의 갯수 가져오기, 이름 목록 리스트 얻어오기, 데이터 추가, 복사, 제거
    public class BaseData : ScriptableObject
    {
        public string[] names;
        public const string dataDirectory = "Data/";

        // 생성자
        public BaseData() { }

        // 데이터의 갯수 가져오기
        public int GetDataCount()
        {
            if (names == null)
                return 0;

            return names.Length;
        }

        // 툴에 출력하기 위해 이름 목록 리스트 얻어오기
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

        // 데이터 추가 후 최종 갯수 리턴
        public virtual int AddData(string newName)
        { 


            return GetDataCount();
        }

        // 데이터 복사
        public virtual void CopyData(int index)
        {
            
        }

        // 데이터 제거
        public virtual void RemoveData(int index)
        {

        }
    }
}