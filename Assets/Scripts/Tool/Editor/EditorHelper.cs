using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;
using System.Text;
using System.IO;

namespace MySampleEx
{
    // Ʋ�� ���õ� ���� ��� ����
    // ��� ������, �̸� ��� ����Ʈ�� enum ���� �����
    public class EditorHelper
    {
        // �Ű������� ���� UnityObject�� ��ġ ��� ������
        public static string GetPath(UnityObject p_clip)
        { 
            string retString = string.Empty;

            // p_clip Ŭ���� ��ü ��� : Assets/Resources/EffectData
            retString = AssetDatabase.GetAssetPath(p_clip);

            string[] path_node = retString.Split('/');
            bool findResources = false;

            for (int i = 0; i < path_node.Length - 1; i++)
            {
                if (findResources == false)
                {
                    if (path_node[i] == "Resources")
                    {
                        findResources = true;
                        retString = string.Empty;
                    }
                }
                else
                { 
                    retString += path_node[i] + "/";
                }
            }

            return retString;
        }

        // �̸� ��� ����Ʈ�� enum���� �����
        public static void CreateEnumStructure(string enumName, StringBuilder data)
        {
            string templateFilePath = "Assets/Editor/EnumTemplate.txt";
            string entittyTemplate = File.ReadAllText(templateFilePath);

            entittyTemplate = entittyTemplate.Replace("$ENUM$", enumName);
            entittyTemplate = entittyTemplate.Replace("$DATA$", data.ToString());

            string folderPath = "Assets/Scripts/GameData/";

            if (Directory.Exists(folderPath) == false)
            {
                Directory.CreateDirectory(folderPath);    
            }

            string filePath = folderPath + enumName + ".cs";

            // ������ �����ϸ� ������ ����
            if (File.Exists(filePath))
            { 
                File.Delete(filePath);
            }

            File.WriteAllText(filePath, entittyTemplate);
        }

        // ������ �� ��� ���̾�, Add, Copy, Remove ��ư �׸���
        public static void EditToolTopLayer(BaseData data, ref int selection, ref UnityObject source, int uiWidth)
        { 
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Add", GUILayout.Width(uiWidth)))
                {
                    data.AddData("NewData");
                    selection = data.GetDataCount() - 1;
                    source = null;
                }
                if (GUILayout.Button("Copy", GUILayout.Width(uiWidth)))
                {
                    data.CopyData(selection);
                    selection = data.GetDataCount() - 1;
                    source = null;
                }
                if (data.GetDataCount() > 1)
                {
                    if (GUILayout.Button("Remove", GUILayout.Width(uiWidth)))
                    {
                        source = null;
                        data.RemoveData(selection);               
                    }
                }
                if (selection > data.GetDataCount() - 1)
                { 
                    selection = data.GetDataCount() - 1;
                }

            }
            EditorGUILayout.EndHorizontal();
        }

        // ������ ��� ����Ʈ �׸���
        public static void EditorToolListLayer(BaseData data, ref int selection, ref UnityObject source, int uiWidth, ref Vector2 scrollPos)
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(uiWidth));
            { 
                EditorGUILayout.Separator();

                EditorGUILayout.BeginVertical("box");
                { 
                    EditorGUILayout.BeginScrollView(scrollPos);
                    {
                        int lastSelect = selection;
                        selection = GUILayout.SelectionGrid(selection, data.GetNameList(true), 1);

                        // �ٸ� �����͸� �����ϸ�
                        if (lastSelect != selection)
                        {
                            source = null;
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
    }
}