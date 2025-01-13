using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace MySampleEx
{
    // 데미지 기능 구현 클래스
    public partial class Damageable : MonoBehaviour
    {
        // MaxHealth
        public int maxHitPoint;
        // 데미지 후 무적 타임
        public float invulnerablityTime = 0f;

        //
        [Range(0f, 360f)]
        public float hitAngle = 360f;
        [Range(0f, 360f)]
        public float hitForwardRotation = 360f;

        // 무적 여부
        public bool IsInvulnerable { get; set; }
        // Current Health
        public int CurrentHitPoint { get; private set; }

        public List<MonoBehaviour> onDamgeMessageReceviers = new List<MonoBehaviour>();

        // 무적타이머 카운트
        protected float m_timeSinceLastHit = 0f;

        public UnityAction OnDeath;
        public UnityAction OnReciveDamage;
        public UnityAction OnHitWhileVulnerable;
        public UnityAction OnBecomeVulnerable;
        public UnityAction OnResetDamage;

        protected Collider m_Collider;
        // 등록된 함수를 LateUpdate에서 호출해서 실행
        private Action schedule;

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            // 무적 타이머
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

        // 충돌체 활성화, 비활성
        void SetColliderState(bool enableed)
        {
            m_Collider.enabled = enableed;
        }

        // 데미지 데이터 초기화
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

        // 데미지 입기
        public void TakeDamage(DamageMessage data)
        {
            // 죽으면 데미지 입지 않기
            if (CurrentHitPoint <= 0)
                return;

            // 무적이면 
            if (IsInvulnerable)
            { 
                OnHitWhileVulnerable?.Invoke();
                return;
            }

            // Hit 방향 구하기
            Vector3 forward = transform.forward;

            forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

            Vector3 positionToDamager = data.damageSource - transform.position;
            positionToDamager -= transform.up * Vector3.Dot(transform.up, positionToDamager);

            if(Vector3.Angle(forward, positionToDamager) > (hitAngle * 0.5f))
            {
                return;
            }

            // 데미지 처리
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

            // 데미지 메세지 보내기
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
            // Hit 방향 구하기
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