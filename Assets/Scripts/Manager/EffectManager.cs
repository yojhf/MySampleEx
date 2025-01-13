using UnityEngine;

namespace MySampleEx
{
    // ����Ʈ ������ �ⴱ����
    public class EffectManager : Singleton<EffectManager>
    {
        private Transform effectRoot = null;

        private void Start()
        {
            if (effectRoot == null)
            { 
                effectRoot = new GameObject("EffectRoot").transform;

                effectRoot.SetParent(transform);
            }
        }

        // ����Ʈ �÷���
        public GameObject EffectOneShot(int index, Vector3 pos)
        {
            EffectClip clip = DataManager.GetEffectData().GetClip(index);

            if (clip == null)
            {
                return null;
            }

            GameObject effectInstance = clip.Instantiate(pos);

            effectInstance.SetActive(true);            

            return effectInstance;

        }
    }
}