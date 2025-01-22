using UnityEngine;
using TMPro;

namespace MySampleEx
{
    // NPC를 관리하는 클래스, 인터렉티브 기능 추가
    public class PickUpNpc : MonoBehaviour
    {
        public NPC npc;

        // 인터랙티브 기능
        protected PlayerCon playerCon;
        protected float distance;
        
        public TMP_Text actionTextUI;
        public string actionText = "Pick Up";

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected virtual void OnMouseOver()
        {
            distance = Vector3.Distance(transform.position, playerCon.transform.position);

            if(distance < 2f)
            {
                ShowActionUI();
            }
            else
            {
                HiddenActionUI();
            }

            if(Input.GetKeyDown(KeyCode.E) && distance < 2f)
            {
                //transform.GetComponent<Collider>().enabled = false;

                DoAction();
            }
        }

        protected virtual void OnMouseExit()
        {
            HiddenActionUI();
        }

        void Init()
        {
            playerCon = FindAnyObjectByType<PlayerCon>();
        }

        
        protected virtual void ShowActionUI()
        {
            actionTextUI.gameObject.SetActive(true);
            actionTextUI.text = actionText + npc.name;
        }

        protected virtual void HiddenActionUI()
        {
            actionTextUI.gameObject.SetActive(false);
            actionTextUI.text = "";
        }

        public virtual void DoAction()
        {
            UIManager.Instance.OpenDialogUI(0, npc.npcType);
        }
       
    }
}
