using System;
using System.Collections.Generic;
using UnityEngine;

namespace MySampleEx
{
    // 캐릭터 속성 값을 관리하는 클래스
    [Serializable]
    public class ModifiableInt
    {
        // 기본 값
        [NonSerialized]
        private int baseValue;
        // 수정된 값, 최종 값
        [SerializeField]
        private int modifedValue;

        public int BaseValue
        { 
            get { return baseValue; } 
            set 
            { 
                baseValue = value;
                UpdateModifedValue();
            } 
        }

        public int ModifedValue
        {
            get { return modifedValue; }
            set { modifedValue = value; }
        }

        // modifedValue 값 변경 시 등록된 함수 실행
        private event Action<ModifiableInt> OnModifedValue;
        // modifedValue 값 계산시 추가될 값들을 저장한 리스트
        private List<IModifier> modifiers = new List<IModifier>();

        // 생성자 - 값 변경 시 호출 할 함수를 매개변수로 받아 등록
        public ModifiableInt(Action<ModifiableInt> _method = null)
        {
            ModifedValue = baseValue;
            RegisterModEvent(_method);
        }

        public void RegisterModEvent(Action<ModifiableInt> _method)
        {
            if (_method != null)
            {
                OnModifedValue += _method;
            }
        }
     
        public void UnRegisterModEvent(Action<ModifiableInt> _method)
        {
            if (_method != null)
            {
                OnModifedValue -= _method;
            }
        }

        // modifedValue 값 구하기, 
        void UpdateModifedValue()
        {
            int valueToAdd = 0;

            foreach(var modifier in modifiers)
            {
                modifier.AddValue(ref valueToAdd);
            }

            ModifedValue = baseValue + valueToAdd;

            // modifedValue 값 변경 시 등록된 함수 호출
            OnModifedValue?.Invoke(this);
        }

        public void AddModifer(IModifier _modifier)
        {
            modifiers.Add(_modifier);
            UpdateModifedValue();
        }

        public void RemoveModifer(IModifier _modifier)
        {
            modifiers.Remove(_modifier);
            UpdateModifedValue();
        }
    }
}