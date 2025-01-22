using System;
using System.Collections.Generic;

namespace MySampleEx
{
    public enum NextType
    {
        None = -1,
        Quest,
        Shop
    }

    [Serializable]
    // Dialog 데이터 모델 클래스
    public class Dialogs
    {
        public List<Dialog> dialogs;
    }

    [Serializable]
    public class Dialog
    {
        public NextType nextType;

        public int number;
        public int character;
        public string name;
        public string sentence;

    }

}
