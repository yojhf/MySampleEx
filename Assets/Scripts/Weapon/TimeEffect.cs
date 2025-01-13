using UnityEngine;
using System.Collections;

namespace MySampleEx
{
    // �÷��̾� ���� ����Ʈ �ִϸ��̼� �÷���
    public class TimeEffect : MonoBehaviour
    {
        public Light staffLight;
        private Animation m_Animation;

        private void Awake()
        {
            Init();
        }

        void Init()
        {
            m_Animation = GetComponent<Animation>();
            gameObject.SetActive(false);
        }

        // ����Ʈ ����
        public void Activate()
        {
            gameObject.SetActive(true);
            staffLight.enabled = true;

            if(m_Animation != null)
            {
                m_Animation.Play();
            }

            // ����Ʈ �ʱ�ȭ
            StartCoroutine(DisableAtEndOfAnimation());
        }

        IEnumerator DisableAtEndOfAnimation()
        {
            yield return new WaitForSeconds(m_Animation.clip.length);

            gameObject.SetActive(false);
            staffLight.enabled = false;
        }
    }
}