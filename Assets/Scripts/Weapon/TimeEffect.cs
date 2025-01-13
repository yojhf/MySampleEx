using UnityEngine;
using System.Collections;

namespace MySampleEx
{
    // 플레이어 공격 이펙트 애니메이션 플레이
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

        // 이펙트 연출
        public void Activate()
        {
            gameObject.SetActive(true);
            staffLight.enabled = true;

            if(m_Animation != null)
            {
                m_Animation.Play();
            }

            // 이펙트 초기화
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