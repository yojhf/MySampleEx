using System;
using UnityEngine;
using System.Collections.Generic;

namespace MySampleEx
{
    // ����Ʈ Ÿ��
    public enum QuestType
    {
        None = -1,
        Kill,
        Collect
    }
    public enum QuestState
    {
        None = -1,
        Ready,      // ����Ʈ ���� ����
        Accept,     // ����Ʈ ������ ����
        Complete,   // ����Ʈ �Ϸ�
        Reward      // ����
    }

    [Serializable]
    public class Quests
    {
        public List<Quest> quests;
    }
    // ����Ʈ ������ Ŭ����
    [Serializable]
    public class Quest
    {
        // ����Ʈ �ε���
        public int number {  get; set; }
        // ����Ʈ�� ������ �ִ� Npc
        public int npcNumber { get; set; }
        // ����Ʈ �̸�
        public string name { get; set; }
        // ����Ʈ ����
        public string description { get; set; }
        // ����Ʈ ��ȭ���� - �Ƿ�, ������, �Ϸ�
        public int dialogIndex { get; set; }
        // ����Ʈ ���� ����
        public int level { get; set; }
        // ����Ʈ ���� ���
        public int rewardGold { get; set; }
        // ����Ʈ ���� ����ġ
        public int rewardExp { get; set; }
        // ����Ʈ ���� ������
        public int rewardItem { get; set; }
        // ����Ʈ Ÿ��
        public QuestType questType { get; set; }
        // ����Ʈ ��ǥ �� ������ Ÿ��, Npc Ÿ��
        public int goalIndex { get; set; }
        // ����Ʈ ��ǥ ����
        public int goalAmount { get; set; }

        // ����Ʈ ��ǥ
        [NonSerialized]
        public QuestGoal questGoal;
        // ����Ʈ ����
        [NonSerialized]
        public QuestState questState;

        // ����Ʈ �̼� �޼� - Kill
        public void EnemyKill(int enemyId)
        {
            if(questGoal.questType == QuestType.Kill)
            {
                if(questGoal.goalIndex == enemyId)
                {
                    questGoal.currentAmount++;
                }
            }
        }

        // ����Ʈ �̼� �޼� - Collect
        public void ItemCollect(int itemId)
        {
            if (questGoal.questType == QuestType.Collect)
            {
                if (questGoal.goalIndex == itemId)
                {
                    questGoal.currentAmount++;
                }
            }
        }


    }
}