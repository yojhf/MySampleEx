using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;

namespace MySampleEx
{
    // ��ȭâ ���� Ŭ����
    // ��ȭ ������ ���� �б�
    // ��ȭ ������UI ����
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

        // ��ȭ ���� �� ����� �̺�Ʈ
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

        // Xml ������ �б�
        void LoadDialogXml(string path)
        { 
            TextAsset xmlFile = Resources.Load<TextAsset>(path);

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(xmlFile.text);
        }

        // ��ȭ �����ϱ�
        public void StartDialog(int dialogIndex)
        {
            // ���� ��ȭ �� ������ ť�� �Է�
            foreach (var dialog in DataManager.GetDialogData().dialogs.dialogs)
            {
                if(dialog.number == dialogIndex)
                {
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

        // ��ȭ����
        void EndDialog()
        { 
            // ��ȭ ���� �� �̺�Ʈ ó��
            OnCloseDialog?.Invoke();
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