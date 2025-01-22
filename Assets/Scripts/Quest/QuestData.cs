using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEditor.PackageManager.Requests;

namespace MySampleEx
{
    public class QuestData : ScriptableObject
    {
        // ��ȭ ���̾�α� ����Ʈ�� �����ϴ� ��ü
        public Quests questList;

        //private string xmlFilePath = string.Empty;
        private string dataPath = "Data/QuestData";

        // ������
        public QuestData() { }

        public void LoadData()
        {
            TextAsset asset = (TextAsset)ResourcesManager.Load(dataPath);

            if (asset == null || asset.text == null)
            {
                // TODO : ���ο� ������ �߰�
                return;
            }

            using (XmlTextReader reader = new XmlTextReader(new StringReader(asset.text)))
            {
                var xs = new XmlSerializer(typeof(Quests));

                questList = (Quests)xs.Deserialize(reader);

                //foreach (var quest in questList.quests)
                //{
                //    quest.questGoal = new QuestGoal();
                //    quest.questGoal.questType = quest.questType;
                //    quest.questGoal.goalIndex = quest.goalIndex;
                //    quest.questGoal.goalAmount = quest.goalAmount;
                //    quest.questGoal.currentAmount = 0;
                //}
            }

        }
    }
}