using System.Collections.Generic;
using UnityEngine;

namespace MySampleEx
{
    public class Effect
    { 
        public List<EffectClip> effectClips { get; set; }
    }

    // ����Ʈ �Ӽ� ������ : ����Ʈ ������, ���, Ÿ�Ե�....
    // ��� : ������ ���� �ε�, ����Ʈ �ν��Ͻ�
    public class EffectClip
    {
        // ����Ʈ ��� �ε���
        public int id { get; set; }
        // ����Ʈ �̸�
        public string name { get; set; }
        // ����Ʈ Ÿ��
        public EffectType effectType { get; set; }
        // ����Ʈ ������ ���
        public string effectPath { get; set; }
        // ����Ʈ ������ �̸�
        public string effectName { get; set; }

        private GameObject effectPrefab = null;

        // ������
        public EffectClip() { }

        // ������ ���� �ε�
        public void PreLoad()
        {
            if (effectPath == null || effectName == null)
                return;

            var effectFullPath = effectPath + effectName;

            if (effectFullPath != string.Empty && effectPrefab == null)
            {
                effectPrefab = ResourcesManager.Load(effectFullPath) as GameObject;
            }
        }

        // ������ ����
        public void ReleaseEffect()
        {
            if (effectPrefab != null)
            {
                effectPrefab = null;
            }
        }

        // ����Ʈ �ν��Ͻ�
        public GameObject Instantiate(Vector3 pos)
        {
            if (effectPrefab == null)
            {
                PreLoad();
            }

            if (effectPrefab != null)
            { 
                GameObject effectGo = GameObject.Instantiate(effectPrefab, pos, Quaternion.identity);

                return effectGo;
            }

            return null;
        }
    }
}