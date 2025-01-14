using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MySampleEx
{
    public class MeeleWeapon : MonoBehaviour
    {
        [Serializable]
        public class AttackPoint
        {
            public float radius;
            public Vector3 offset;
            public Transform attackRoot;
#if UNITY_EDITOR
            public List<Vector3> previousPositions = new List<Vector3>();
#endif
        }

        // hit시 데미지 포인트
        public int damage = 1;

        // 공격 포인트
        public AttackPoint[] attackPoints = new AttackPoint[0];
        // 공격(스윙) 이펙트
        public TimeEffect[] effects;

        // 공격 시 타격 이펙트
        public ParticleSystem hitParticlePrefab;
        public LayerMask targetLayers;

        protected GameObject m_Owner;

        protected Vector3[] m_PreviousPos = null;
        protected Vector3 m_Direction;

        protected bool m_IsThrowingHit = false;
        protected bool m_InAttack = false;

        public bool ThrowingHit 
        {
            get
            {
                return m_IsThrowingHit;
            }
            set
            {
                m_IsThrowingHit = value;
            }
                
        }

        const int particle_Count = 10;
        protected ParticleSystem[] m_ParticlesPool = new ParticleSystem[particle_Count];
        protected int m_CurrentParticle = 0;

        protected static RaycastHit[] s_RaytcastHitCache = new RaycastHit[32];
        protected static Collider[] s_ColliderCache = new Collider[32];

        private void Awake()
        {
            Init();
        }

        private void FixedUpdate()
        {
            if(m_InAttack)
            {
                // 어택 포인트별 히트 판정
                for (int i = 0; i < attackPoints.Length; i++)
                {
                    AttackPoint apt = attackPoints[i];
                    Vector3 worldPos = attackPoints[i].attackRoot.position +
                        attackPoints[i].attackRoot.TransformVector(attackPoints[i].offset);
                    Vector3 attackVector = worldPos - m_PreviousPos[i];

                    if(attackVector.magnitude < 0.001f)
                    {
                        attackVector = Vector3.forward * 0.0001f;
                    }

                    Ray ray = new Ray(worldPos, attackVector.normalized);

                    int contacts = Physics.SphereCastNonAlloc(ray, apt.radius,
                        s_RaytcastHitCache, attackVector.magnitude, ~0, QueryTriggerInteraction.Ignore);

                    for (int j = 0; j < contacts; j++)
                    {
                        Collider col = s_RaytcastHitCache[j].collider;

                        if(col != null)
                        {
                            CheckDamage(col, apt);
                        }
                    }

                    m_PreviousPos[i] = worldPos;
#if UNITY_EDITOR
                    attackPoints[i].previousPositions.Add(m_PreviousPos[i]);
#endif
                }
            }
        }

        void Init()
        {
            // 타격이펙트 Pool 생성
            if(hitParticlePrefab != null)
            {
                for(int i = 0; i < particle_Count; i++)
                {
                    m_ParticlesPool[i] = Instantiate(hitParticlePrefab);
                    m_ParticlesPool[i].Stop();
                }
            }
        }

        public void SetOwner(GameObject owner)
        {
            m_Owner = owner;
        }

        public void BeginAttack(bool throwingAttack)
        {
            ThrowingHit = throwingAttack;

            m_PreviousPos = new Vector3[attackPoints.Length];

            for (int i = 0; i < attackPoints.Length; i++)
            {
                Vector3 worldPos = attackPoints[i].attackRoot.position +
                    attackPoints[i].attackRoot.TransformVector(attackPoints[i].offset);

                m_PreviousPos[i] = worldPos;

#if UNITY_EDITOR
                attackPoints[i].previousPositions.Clear();
                attackPoints[i].previousPositions.Add(m_PreviousPos[i]);
#endif
            }
                
            m_InAttack = true;
        }

        public void EndAttack()
        {
            m_InAttack = false;

#if UNITY_EDITOR
            for (int i = 0; i < attackPoints.Length; i++)
            {
                attackPoints[i].previousPositions.Clear();
            }
#endif
        }

        // 콜라이더 확인 후 데미지 주기
        void CheckDamage(Collider other, AttackPoint apt)
        {
            // Damageable 콜라이더 확인
            Damageable d = other.GetComponent<Damageable>();

            if (d == null)
            {
                return;
            }

            // 셀프 데미지 체크
            if(d.gameObject == m_Owner)
            {
                return;
            }

            // 타겟 레이어 체크
            if((targetLayers.value & (1 << other.gameObject.layer)) == 0)
            {
                return;
            }

            // 데미지 데이터 가공 후 데미지 주기
            Damageable.DamageMessage data;

            data.amount = damage;
            data.damager = this;
            data.direction = m_Direction.normalized;
            data.damageSource = m_Owner.transform.position;
            data.throwing = ThrowingHit;
            data.stopCamera = false;

            d.TakeDamage(data);

            // 타격 이펙트
            if (hitParticlePrefab != null)
            {
                m_ParticlesPool[m_CurrentParticle].transform.position = apt.attackRoot.transform.position;
                m_ParticlesPool[m_CurrentParticle].time = 0;
                m_ParticlesPool[m_CurrentParticle].Play();
                m_CurrentParticle = (m_CurrentParticle + 1) % particle_Count;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            for (int i = 0; i < attackPoints.Length; i++)
            {
                AttackPoint apt = attackPoints[i];

                if(apt.attackRoot != null)
                {
                    Vector3 worldPos = apt.attackRoot.TransformVector(apt.offset);
                    Gizmos.color = new Color(1f, 1f, 1f, 0.4f);
                    Gizmos.DrawSphere(apt.attackRoot.position + worldPos, apt.radius);
                }

                if(apt.previousPositions.Count > 0)
                {
                    Handles.DrawAAPolyLine(10, apt.previousPositions.ToArray());
                }
            }
        }
#endif
    }

}