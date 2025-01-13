using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace MySampleEx
{
    // ������ : ����Ʈ Ŭ�� ����Ʈ, ����Ʈ ������ ���� �̸�, ���
    // ��� : ������ ����(xml) �б�(Load), ����(Save), ������ �߰�, ����, ����
    public class EffectData : BaseData
    {
        // ����Ʈ Ŭ�� ����Ʈ
        public Effect effect;

        private string xmlFilePath = string.Empty;
        private string xmlFileName = "effectData.xml";
        private string dataPath = "Data/effectData";

        // ������
        public EffectData() { }

        // ������ ����(xml) �б�(Load)
        public void LoadData()
        {
            TextAsset asset = (TextAsset)ResourcesManager.Load(dataPath);

            if (asset == null || asset.text == null)
            {
                // TODO : ���ο� ������ �߰�
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

        // ������ ����(xml) ����(Save)
        public void SaveData()
        {
            // ������ ���� ���
            xmlFilePath = Application.dataPath + dataDirectory;
            Debug.Log(xmlFilePath);

            using (XmlTextWriter xml = new XmlTextWriter(xmlFilePath + xmlFileName, System.Text.Encoding.Unicode))
            {
                var xs = new XmlSerializer(typeof(Effect));

                // ������ ���� ����
                int length = effect.effectClips.Count;

                for (int i = 0; i < length; i++)
                {
                    effect.effectClips[i].id = i;
                    effect.effectClips[i].name = names[i];
                }

                xs.Serialize(xml, effect);
            }
        }

        // ������ �߰�, ������ ��� ���� ��ȯ
        public override int AddData(string newName)
        {
            // ������ ������ �������� ���� ���
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

        // ������ ����
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

        // ������ ����, ���� ������ �ε����� Ŭ���� �����ؼ� �߰�
        public override void CopyData(int index)
        {
            names.Add(names[index]);
            effect.effectClips.Add(GetCopy(index));
            
        }

        // ������ ����
        public EffectClip GetCopy(int index)
        {
            // �ε��� üũ
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

        // ��� ������ ����
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

        // ���� ������ ����Ʈ Ŭ�� ��������
        public EffectClip GetClip(int index)
        {
            // �ε��� üũ
            if (index < 0 || index >= effect.effectClips.Count)
            {
                return null;
            }

            // ������ �ε�
            effect.effectClips[index].PreLoad();

            return effect.effectClips[index];
        }


    }
}