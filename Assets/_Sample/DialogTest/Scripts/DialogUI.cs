using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

namespace MySampleEx
{
    // 대화창 구현 클래스
    // 대화 데이터 파일 읽기
    // 대화 데이터UI 적용
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
            // 데이터 파일 읽기
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

        // Xml 데이터 읽기
        void LoadDialogXml(string path)
        { 
            TextAsset xmlFile = Resources.Load<TextAsset>(path);

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(xmlFile.text);

            allNodes = xmlDoc.SelectNodes("root/Dialog");
        }

        // 대화 시작하기
        public void StatDialog(int dialogIndex)
        {
            // 현재 대화 씬 내용을 큐에 입력
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

            // 첫번째 대화를 보여준다
            DrawNextDialog();
        }

        // 다음 대화를 보여준다 - (큐) dialog에서 하나 꺼내서 보여준다
        public void DrawNextDialog()
        {
            if (dialogs.Count == 0)
            {
                EndDialog();
                return;
            }

            nextBtn.SetActive(false);

            // dialogs에서 하나 꺼내온다
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

        // 대화종료
        void EndDialog()
        { 
            InitDialog();

            // 대화 종료 시 이벤트 처리
        }

        // 텍스트 타이핑 연출
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