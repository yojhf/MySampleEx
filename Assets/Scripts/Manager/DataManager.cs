using UnityEngine;

namespace MySampleEx
{
    // ���ӿ��� ����ϴ� �����͸� �����ϴ� Ŭ����
    public class DataManager : MonoBehaviour
    {
        private static EffectData effectData = null;
        private static DialogData dialogData = null;
        private static QuestData questData = null;

        public InventoryObject inventoryObject;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // ����Ʈ ������ ��������
            if (effectData == null)
            {
                effectData = ScriptableObject.CreateInstance<EffectData>();
                effectData.LoadData();
            }

            // ���̾�α� ������ ��������
            if (dialogData == null)
            {
                dialogData = ScriptableObject.CreateInstance<DialogData>();
                dialogData.LoadData();
            }

            // ����Ʈ ������ ��������
            if (questData == null)
            {
                questData = ScriptableObject.CreateInstance<QuestData>();
                questData.LoadData();
            }

            inventoryObject.Load();
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

        // ���̾�α� ������ ��������
        public static DialogData GetDialogData()
        {
            if (dialogData == null)
            {
                dialogData = ScriptableObject.CreateInstance<DialogData>();
                dialogData.LoadData();
            }

            return dialogData;
        }

        // ����Ʈ ������ ��������
        public static QuestData GetQuestData()
        {
            if (questData == null)
            {
                questData = ScriptableObject.CreateInstance<QuestData>();
                questData.LoadData();
            }

            return questData;
        }
    }
}