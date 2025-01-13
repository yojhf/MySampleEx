using System.Collections.Generic;
using UnityEngine;

namespace MySampleEx
{
    public class Effect
    { 
        public List<EffectClip> effectClips { get; set; }
    }

    // 이펙트 속성 데이터 : 이펙트 프리펩, 경로, 타입등....
    // 기능 : 프리펩 사전 로딩, 이펙트 인스턴스
    public class EffectClip
    {
        // 이펙트 목록 인덱스
        public int id { get; set; }
        // 이펙트 이름
        public string name { get; set; }
        // 이펙트 타입
        public EffectType effectType { get; set; }
        // 이펙트 프리팹 경로
        public string effectPath { get; set; }
        // 이펙트 프리팹 이름
        public string effectName { get; set; }

        private GameObject effectPrefab = null;

        // 생성자
        public EffectClip() { }

        // 프리펩 사전 로딩
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

        // 프리펩 해제
        public void ReleaseEffect()
        {
            if (effectPrefab != null)
            {
                effectPrefab = null;
            }
        }

        // 이펙트 인스턴스
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