using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;

namespace MySampleEx
{
    // 대화창 구현 클래스
    // 대화 데이터 파일 읽기
    // 대화 데이터UI 적용
    public class DialogUI : MonoBehaviour
    {
        // Path
        public string xmlFile = "Dialog/DialogData";

        private Queue<Dialog> dialogs;

        // UI
        public TMP_Text nameText;
        public TMP_Text sentenceText;
        public GameObject npcImage;
        public GameObject nextBtn;

        // 대화 종료 시 실행된 이벤트
        public Action OnCloseDialog;

        private void Start()
        {
            //StartDialog(0);
        }

        private void OnEnable()
        {
            dialogs = new Queue<Dialog>();
            InitDialog();
        }

        private void OnDisable()
        {
            InitDialog();
            dialogs = null;
            OnCloseDialog = null;
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
        }

        // 대화 시작하기
        public void StartDialog(int dialogIndex)
        {
            // 현재 대화 씬 내용을 큐에 입력
            foreach (var dialog in DataManager.GetDialogData().dialogs.dialogs)
            {
                if(dialog.number == dialogIndex)
                {
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

            //if (dialog.character > 0)
            //{
            //    npcImage.SetActive(true);

            //    npcImage.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Dialog/Npc/npc0{dialog.character.ToString()}");
            //}
            //else
            //{ 
            //    npcImage.SetActive(false);
            //}

            nextBtn.gameObject.SetActive(false);

            nameText.text = dialog.name;
            //sentenceText.text = dialog.sentence;
            StartCoroutine(TypingText(dialog.sentence)); 
        }

        // 대화종료
        void EndDialog()
        { 
            // 대화 종료 시 이벤트 처리
            OnCloseDialog?.Invoke();
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