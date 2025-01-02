using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

namespace MySampleEx
{
    // ��ȭâ ���� Ŭ����
    // ��ȭ ������ ���� �б�
    // ��ȭ ������UI ����
    public class DialogUI : MonoBehaviour
    {
        // Path
        public string xmlFile = "Dialog/Dialog";
        // XML
        private XmlNodeList allNodes;

        private Queue<Dialog> dialogs;

        // UI
        public TMP_Text nameText;
        public TMP_Text sentenceText;
        public GameObject npcImage;
        public GameObject nextBtn;


        private void Start()
        {
            // ������ ���� �б�
            LoadDialogXml(xmlFile);

            dialogs = new Queue<Dialog>();

            InitDialog();

            //StatDialog(0);
        }

        void InitDialog()
        {
            dialogs.Clear();
            
            npcImage.SetActive(false);
            nameText.text = "";
            sentenceText.text = "";

            nextBtn.SetActive(false);
        }

        // Xml ������ �б�
        void LoadDialogXml(string path)
        { 
            TextAsset xmlFile = Resources.Load<TextAsset>(path);

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(xmlFile.text);

            allNodes = xmlDoc.SelectNodes("root/Dialog");
        }

        // ��ȭ �����ϱ�
        public void StatDialog(int dialogIndex)
        {
            // ���� ��ȭ �� ������ ť�� �Է�
            foreach (XmlNode node in allNodes)
            {
                int num = int.Parse(node["number"].InnerText);

                if (num == dialogIndex)
                { 
                    Dialog dialog = new Dialog();

                    dialog.number = num;
                    dialog.character = int.Parse(node["character"].InnerText);
                    dialog.name = node["name"].InnerText;
                    dialog.sentence = node["sentence"].InnerText;

                    dialogs.Enqueue(dialog);
                }

            }

            // ù��° ��ȭ�� �����ش�
            DrawNextDialog();
        }

        // ���� ��ȭ�� �����ش� - (ť) dialog���� �ϳ� ������ �����ش�
        public void DrawNextDialog()
        {
            if (dialogs.Count == 0)
            {
                EndDialog();
                return;
            }

            nextBtn.SetActive(false);

            // dialogs���� �ϳ� �����´�
            Dialog dialog = dialogs.Dequeue();

            if (dialog.character > 0)
            {
                npcImage.SetActive(true);

                npcImage.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Dialog/Npc/npc0{dialog.character.ToString()}");
            }
            else
            { 
                npcImage.SetActive(false);
            }

            nameText.text = dialog.name;
            //sentenceText.text = dialog.sentence;
            StartCoroutine(TypingText(dialog.sentence)); 
        }

        // ��ȭ����
        void EndDialog()
        { 
            InitDialog();

            // ��ȭ ���� �� �̺�Ʈ ó��
        }

        // �ؽ�Ʈ Ÿ���� ����
        IEnumerator TypingText(string text)
        { 
            sentenceText.text = "";

            foreach (var latter in text)
            { 
                sentenceText.text += latter;

                yield return new WaitForSeconds(0.03f);  
            }


            nextBtn.SetActive(true);
        }

    }
}