using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MySampleEx
{
    public class QuestData : ScriptableObject
    {
        // ��ȭ ���̾�α� ����Ʈ�� �����ϴ� ��ü
        public Quests dialogs;

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

                dialogs = (Quests)xs.Deserialize(reader);
            }

        }
    }
}