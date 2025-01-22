using System;
using UnityEngine;

namespace MySampleEx
{
    [Serializable]
    public class QuestGoal
    {
        // 퀘스트 타입
        public QuestType questType { get; set; }
        // 퀘스트 목표 시 아이템 타입, Npc 타입
        public int goalIndex { get; set; }
        // 퀘스트 목표 수량
        public int goalAmount { get; set; }
        // 퀘스트 현재 달성량
        [NonSerialized]
        public int currentAmount;
        // 퀘스트 달성 여부
        public bool IsReached => (currentAmount >= goalAmount);

    }
}