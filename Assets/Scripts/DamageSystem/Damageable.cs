using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace MySampleEx
{
    // ������ ��� ���� Ŭ����
    public partial class Damageable : MonoBehaviour
    {
        // MaxHealth
        public int maxHitPoint;
        // ������ �� ���� Ÿ��
        public float invulnerablityTime = 0f;

        //
        [Range(0f, 360f)]
        public float hitAngle = 360f;
        [Range(0f, 360f)]
        public float hitForwardRotation = 360f;

        // ���� ����
        public bool IsInvulnerable { get; set; }
        // Current Health
        public int CurrentHitPoint { get; private set; }

        public List<MonoBehaviour> onDamgeMessageReceviers = new List<MonoBehaviour>();

        // ����Ÿ�̸� ī��Ʈ
        protected float m_timeSinceLastHit = 0f;

        public UnityAction OnDeath;
        public UnityAction OnReciveDamage;
        public UnityAction OnHitWhileVulnerable;
        public UnityAction OnBecomeVulnerable;
        public UnityAction OnResetDamage;

        protected Collider m_Collider;
        // ��ϵ� �Լ��� LateUpdate���� ȣ���ؼ� ����
        private Action schedule;

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            // ���� Ÿ�̸�
            InvulnerablityTimer();
        }

        private void LateUpdate()
        {
            if (schedule != null)
            {
                schedule();
                schedule = null;
            }
        }

        void Init()
        {
            m_Collider = GetComponent<Collider>();

            ResetDamage();
        }

        // �浹ü Ȱ��ȭ, ��Ȱ��
        void SetColliderState(bool enableed)
        {
            m_Collider.enabled = enableed;
        }

        // ������ ������ �ʱ�ȭ
        public void ResetDamage()
        {
            CurrentHitPoint = maxHitPoint;
            IsInvulnerable = false;
            m_timeSinceLastHit = 0f;
            OnReciveDamage?.Invoke();
        }

        void InvulnerablityTimer()
        { 
            if(IsInvulnerable)
            {
                m_timeSinceLastHit += Time.deltaTime;

                if(m_timeSinceLastHit > invulnerablityTime)
                {
                    IsInvulnerable = false;
                    OnBecomeVulnerable?.Invoke();

                    m_timeSinceLastHit = 0f;
                }
            }
        }

        // ������ �Ա�
        public void TakeDamage(DamageMessage data)
        {
            // ������ ������ ���� �ʱ�
            if (CurrentHitPoint <= 0)
                return;

            // �����̸� 
            if (IsInvulnerable)
            { 
                OnHitWhileVulnerable?.Invoke();
                return;
            }

            // Hit ���� ���ϱ�
            Vector3 forward = transform.forward;

            forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

            Vector3 positionToDamager = data.damageSource - transform.position;
            positionToDamager -= transform.up * Vector3.Dot(transform.up, positionToDamager);

            if(Vector3.Angle(forward, positionToDamager) > (hitAngle * 0.5f))
            {
                return;
            }

            // ������ ó��
            IsInvulnerable = true;
            CurrentHitPoint -= data.amount;

            if (CurrentHitPoint <= 0)
            { 
                if(OnDeath != null) 
                {
                    schedule += OnDeath.Invoke;
                }
            }
            else
            {
                OnReciveDamage?.Invoke();
            }

            // ������ �޼��� ������
            var messageType = CurrentHitPoint <= 0 ? MessageType.Death : MessageType.Damaged;

            for (int i = 0; i < onDamgeMessageReceviers.Count; i++)
            {
                var receiver = onDamgeMessageReceviers[i] as IMessageReceiver;

                receiver.OnReceiveMessage(messageType, this, data);
            }

        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Hit ���� ���ϱ�
            Vector3 forward = transform.forward;

            forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

            if(Event.current.type == EventType.Repaint)
            {
                Handles.color = Color.blue;
                Handles.ArrowHandleCap(0, transform.position, 
                    Quaternion.LookRotation(forward), 1f, EventType.Repaint);
            }

            Handles.color = new Color(1f, 0f, 0.5f);

            forward = Quaternion.AngleAxis(-hitAngle * 0.5f, transform.up) * forward;

            Handles.DrawSolidArc(transform.position, transform.up, forward, hitAngle, 1.0f);
        }
#endif
    }

}