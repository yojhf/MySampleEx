using System;
using UnityEngine;
using System.Collections.Generic;

namespace MySampleEx
{
    public enum QuestType
    {
        None = -1,
        Kill,
        Collect
    }

    [Serializable]
    public class Quests
    {
        public List<Quest> quests;
    }
    // 퀘스트 데이터 클래스
    [Serializable]
    public class Quest
    {
        // 퀘스트 인덱스
        public int number {  get; set; }
        // 퀘스트를 가지고 있는 Npc
        public int npcNumber { get; set; }
        // 퀘스트 이름
        public string name { get; set; }
        // 퀘스트 내용
        public string description { get; set; }
        // 퀘스트 대화내용 - 의뢰, 진행중, 완료
        public int dialogIndex { get; set; }
        // 퀘스트 레벨 제한
        public int level { get; set; }
        // 퀘스트 타입
        public QuestType questType { get; set; }
        // 퀘스트 목표 시 아이템 타입, Npc 타입
        public int goalIndex { get; set; }
        // 퀘스트 목표 수량
        public int qoalAmount { get; set; }
        // 퀘스트 보상 골드
        public int rewardGold { get; set; }
        // 퀘스트 보상 경험치
        public int rewardExp { get; set; }
        // 퀘스트 보상 아이템
        public int rewardItem { get; set; }


    }
}