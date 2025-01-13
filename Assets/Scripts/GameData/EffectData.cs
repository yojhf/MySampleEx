using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace MySampleEx
{
    // 데이터 : 이펙트 클립 리스트, 이펙트 데이터 파일 이름, 경로
    // 기능 : 데이터 파일(xml) 읽기(Load), 쓰기(Save), 데이터 추가, 복사, 제거
    public class EffectData : BaseData
    {
        // 이펙트 클립 리스트
        public Effect effect;

        private string xmlFilePath = string.Empty;
        private string xmlFileName = "effectData.xml";
        private string dataPath = "Data/effectData";

        // 생성자
        public EffectData() { }

        // 데이터 파일(xml) 읽기(Load)
        public void LoadData()
        {
            TextAsset asset = (TextAsset)ResourcesManager.Load(dataPath);

            if (asset == null || asset.text == null)
            {
                // TODO : 새로운 데이터 추가
                AddData("NewEffect");
                return;
            }

            using (XmlTextReader reader = new XmlTextReader(new StringReader(asset.text)))
            { 
                var xs = new XmlSerializer(typeof(Effect));

                effect = (Effect)xs.Deserialize(reader);

                // Data Setting
                int length = effect.effectClips.Count;
                names = new List<string>();

                for (int i = 0; i < length; i++)
                {
                    names.Add(effect.effectClips[i].name);
                }
            }
        }

        // 데이터 파일(xml) 쓰기(Save)
        public void SaveData()
        {
            // 데이터 저장 경로
            xmlFilePath = Application.dataPath + dataDirectory;
            Debug.Log(xmlFilePath);

            using (XmlTextWriter xml = new XmlTextWriter(xmlFilePath + xmlFileName, System.Text.Encoding.Unicode))
            {
                var xs = new XmlSerializer(typeof(Effect));

                // 저장할 내용 셋팅
                int length = effect.effectClips.Count;

                for (int i = 0; i < length; i++)
                {
                    effect.effectClips[i].id = i;
                    effect.effectClips[i].name = names[i];
                }

                xs.Serialize(xml, effect);
            }
        }

        // 데이터 추가, 데이터 목록 갯수 반환
        public override int AddData(string newName)
        {
            // 데이터 파일이 존재하지 않을 경우
            if (names == null)
            {
                names = new List<string>() { newName };
                effect = new Effect();
                effect.effectClips = new List<EffectClip>() { new EffectClip() };
            }
            else
            {
                names.Add(newName);
                effect.effectClips.Add(new EffectClip());
            }


            return GetDataCount();
        }

        // 데이터 삭제
        public override void RemoveData(int index)
        {
            names.Remove(names[index]);

            if (names.Count == 0)
            { 
                names = null;
            }

            effect.effectClips.Remove(effect.effectClips[index]);

            if (effect.effectClips.Count == 0)
            {
                effect.effectClips = null;
                effect = null;
            }
        }

        // 데이터 복사, 현재 지정한 인덱스의 클립을 복사해서 추가
        public override void CopyData(int index)
        {
            names.Add(names[index]);
            effect.effectClips.Add(GetCopy(index));
            
        }

        // 데이터 복사
        public EffectClip GetCopy(int index)
        {
            // 인덱스 체크
            if (index < 0 || index >= effect.effectClips.Count)
            { 
                return null;
            }

            EffectClip orignal = effect.effectClips[index];
            EffectClip clip = new EffectClip();

            clip.effectName = orignal.effectName;
            clip.effectType = orignal.effectType;
            clip.effectPath = orignal.effectPath;

            return clip;
        }

        // 모든 데이터 해제
        public void ClearData()
        {
            foreach (var clip in effect.effectClips)
            {
                clip.ReleaseEffect();   
            }

            effect.effectClips = null;
            effect = null;
            names = null;
        }

        // 현재 선택한 이펙트 클립 가져오기
        public EffectClip GetClip(int index)
        {
            // 인덱스 체크
            if (index < 0 || index >= effect.effectClips.Count)
            {
                return null;
            }

            // 프리팹 로드
            effect.effectClips[index].PreLoad();

            return effect.effectClips[index];
        }


    }
}