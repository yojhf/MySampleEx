using UnityEngine;

namespace MySampleEx
{
    // 이펙트 데이터 기눙구현
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

        // 이펙트 플레이
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