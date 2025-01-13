using UnityEngine;
using UnityObject = UnityEngine.Object;
using UnityEditor;
using System.Text;

namespace MySampleEx
{
    public class EffectTool : EditorWindow
    {
        // UI�� �ʿ��� ����
        public int uiWidthLarge = 300;
        public int uiWidthMiddle = 200;
        private Vector2 sp1 = Vector2.zero;
        private Vector2 sp2 = Vector2.zero;

        // ������ ����� ���� ���õ� �ε���
        private int selection = 0;
        // ����Ʈ �ҽ�
        private GameObject effectSorce = null;

        // ����Ʈ ������
        private static EffectData effectData;

        [MenuItem("Tool/Effect Tool")]
        public static void Init()
        {
            // ������ ��������
            effectData = ScriptableObject.CreateInstance<EffectData>();
            effectData.LoadData();


            // �� â ����
            EffectTool window = GetWindow<EffectTool>(false, "Effect Tool");

            window.Show();
        }

        private void OnGUI()
        {
            if (effectData == null)
                return;

            EditorGUILayout.BeginVertical();
            {
                UnityObject source = effectSorce;

                // ��� ���̾�
                EditorHelper.EditToolTopLayer(effectData, ref selection, ref source, uiWidthMiddle);
                effectSorce = (GameObject)source;


                // ����Ʈ ���, ����Ʈ �Ӽ�â ���̾�
                EditorGUILayout.BeginHorizontal();
                {
                    // ����Ʈ ���
                    EditorHelper.EditorToolListLayer(effectData, ref selection, ref source, uiWidthLarge, ref sp1);
                    effectSorce = (GameObject)source;

                    // ����Ʈ �Ӽ�â
                    EditorGUILayout.BeginVertical();
                    {
                        sp2 = EditorGUILayout.BeginScrollView(sp2);
                        {
                            if (effectData.GetDataCount() > 0)
                            {
                                EditorGUILayout.BeginVertical();
                                {
                                    EditorGUILayout.Separator();

                                    // �Ӽ��� ����
                                    EditorGUILayout.LabelField("ID", selection.ToString(), GUILayout.Width(uiWidthLarge));

                                    effectData.names[selection] =
                                        EditorGUILayout.TextField("�̸�", effectData.names[selection], GUILayout.Width(uiWidthLarge * 1.5f));

                                    effectData.effect.effectClips[selection].effectType =
                                        (EffectType)EditorGUILayout.EnumPopup("����Ʈ �̸�", effectData.effect.effectClips[selection].effectType, GUILayout.Width(uiWidthLarge));

                                    EditorGUILayout.Separator();

                                    if (effectSorce == null && effectData.effect.effectClips[selection].name != string.Empty)
                                    {
                                        effectData.effect.effectClips[selection].PreLoad();
                                        effectSorce =
                                            Resources.Load(effectData.effect.effectClips[selection].effectPath +
                                            effectData.effect.effectClips[selection].effectName) as GameObject;
                                    }

                                    effectSorce = (GameObject)EditorGUILayout.ObjectField("����Ʈ", effectSorce, typeof(GameObject), false, GUILayout.Width(uiWidthLarge * 1.5f));

                                    if (effectSorce != null)
                                    {
                                        effectData.effect.effectClips[selection].effectPath = EditorHelper.GetPath(effectSorce);
                                        effectData.effect.effectClips[selection].effectName = effectSorce.name;
                                    }
                                    else
                                    {
                                        effectData.effect.effectClips[selection].effectPath = string.Empty;
                                        effectData.effect.effectClips[selection].effectName = string.Empty;
                                    }

                                }
                                EditorGUILayout.EndVertical();
                            }
                        }
                        EditorGUILayout.EndScrollView();
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            // �ϴ� ���̾ƿ�, �ε�, ����
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Reload Settings"))
                {
                    effectData = ScriptableObject.CreateInstance<EffectData>();
                    effectData.LoadData();
                    selection = 0;
                    effectSorce = null;
                }
                if (GUILayout.Button("Save"))
                {
                    effectData.SaveData();

                    CreateEnumStructure();

                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        public void CreateEnumStructure()
        {
            string enumName = "EffectList";
            StringBuilder builder = new StringBuilder();
            builder.AppendLine();

            for (int i = 0; i < effectData.names.Count; i++)
            {
                if (effectData.names[i] != string.Empty)
                {
                    builder.AppendLine("    " + effectData.names[i] + " = " + i + ", ");
                }
            }

            EditorHelper.CreateEnumStructure(enumName, builder);
        }


    }
}