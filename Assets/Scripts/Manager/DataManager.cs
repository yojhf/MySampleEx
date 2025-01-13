using UnityEngine;

namespace MySampleEx
{
    // 게임에서 사용하는 데이터를 관리하는 클래스
    public class DataManager : MonoBehaviour
    {
        private static EffectData effectData = null;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // 이펙트 데이터 가져오기
            if (effectData == null)
            {
                effectData = ScriptableObject.CreateInstance<EffectData>();
                effectData.LoadData();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        // 이펙트 데이터 가져오기
        public static EffectData GetEffectData()
        {
            if (effectData == null)
            {
                effectData = ScriptableObject.CreateInstance<EffectData>();
                effectData.LoadData();
            }

            return effectData;
        }
    }
}