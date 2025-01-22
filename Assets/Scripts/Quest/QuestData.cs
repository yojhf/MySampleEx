using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MySampleEx
{
    public class QuestData : ScriptableObject
    {
        // 대화 다이얼로그 리스트를 관리하는 객체
        public Quests dialogs;

        //private string xmlFilePath = string.Empty;
        private string dataPath = "Data/QuestData";

        // 생성자
        public QuestData() { }

        public void LoadData()
        {
            TextAsset asset = (TextAsset)ResourcesManager.Load(dataPath);

            if (asset == null || asset.text == null)
            {
                // TODO : 새로운 데이터 추가
                return;
            }

            using (XmlTextReader reader = new XmlTextReader(new StringReader(asset.text)))
            {
                var xs = new XmlSerializer(typeof(Quests));

                dialogs = (Quests)xs.Deserialize(reader);
            }

        }
    }
}