using System;
using System.Collections.Generic;
using UnityEngine;

namespace MySampleEx
{
    // ĳ���� �Ӽ� ���� �����ϴ� Ŭ����
    [Serializable]
    public class ModifiableInt
    {
        // �⺻ ��
        [NonSerialized]
        private int baseValue;
        // ������ ��, ���� ��
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

        // modifedValue �� ���� �� ��ϵ� �Լ� ����
        private event Action<ModifiableInt> OnModifedValue;
        // modifedValue �� ���� �߰��� ������ ������ ����Ʈ
        private List<IModifier> modifiers = new List<IModifier>();

        // ������ - �� ���� �� ȣ�� �� �Լ��� �Ű������� �޾� ���
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

        // modifedValue �� ���ϱ�, 
        void UpdateModifedValue()
        {
            int valueToAdd = 0;

            foreach(var modifier in modifiers)
            {
                modifier.AddValue(ref valueToAdd);
            }

            ModifedValue = baseValue + valueToAdd;

            // modifedValue �� ���� �� ��ϵ� �Լ� ȣ��
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