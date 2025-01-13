using UnityEngine;

namespace MySampleEx
{
    // ���ӿ��� ����ϴ� �����͸� �����ϴ� Ŭ����
    public class DataManager : MonoBehaviour
    {
        private static EffectData effectData = null;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // ����Ʈ ������ ��������
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

        // ����Ʈ ������ ��������
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