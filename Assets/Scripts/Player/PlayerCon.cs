using Unity.Cinemachine;
using UnityEngine;

namespace MySampleEx
{
    // 플레이어 캐릭터 제어(이동, 점프, 애니메이션....) 관리 클래스
    public class PlayerCon : MonoBehaviour, IMessageReceiver
    {
        //
        public InventoryObject inventory;
        public InventoryObject equipment;

        // 플레이어 최고 이동속도
        public float maxForwardSpeed = 0f;
        // 플레이어 최저 회전 속도
        public float minTurnSpeed = 400f;
        // 플레이어 최고 회전 속도
        public float maxTurnSpeed = 1200f;

        protected PlayerInputAction m_Input;
        protected CharacterController m_CharCtrl;
        protected Animator m_Animator;
        protected Damageable m_Damageable;
        
        protected bool m_IsGround = true;
        // 플레이어 입력에 따라 낼 수 있는 최고 속도
        protected float m_DesiredForwardSpeed;
        // 플레이어 현재 속도
        protected float m_ForwardSpeed;
        // 타겟을 향한 회전값
        protected Quaternion m_TargetRotation;
        // 플레이어의 회전값과 타겟의 회전값의 차이
        protected float m_AngleDiff;

        // 이동 입력값 체크
        protected bool IsMoveInput
        {
            get { return !Mathf.Approximately(m_Input.Move.sqrMagnitude, 0f); }
        }

        // 점프
        // 중력값
        public float gravity = 20f;
        // 점프 속도
        public float jumpSpeed = 10f;

        // 점프 준비 단계
        protected bool m_ReadyToJump = false;
        // 점프 이동 속도
        protected float m_VerticalSpeed;

        // 대기 상태로 보내기
        // 이동상태 대기에서 5초가 지나면 대기 상태로 진입
        public float idleTimeout = 5f;
        // 타이머 카운트
        protected float m_IdleTimer = 0f;

        // 카메라(프리룩)
        public CameraSettings m_CameraSettings;

        // 공격
        public MeeleWeapon meeleWeapon;
        // 공격 여부 판단
        protected bool m_InAttack = false;
        // 어택 상태 여부
        protected bool m_InCombo = false;


        // 상수
        // 이동 가속도 값
        const float k_GroundAcceleration = 20f;
        // 이동 감속도 값
        const float k_GroundDeceleration = 25f;
        // 그라운드 체크 시 레이 거리 값
        const float k_GroundRayDistacne = 1f;
        // 점프 속도
        const float k_JumpAbortSpeed = 10f;
        // 
        const float k_StickingGravityPropotion = 0.3f;

        // 애니메이션 상태
        // 애니메이션 전환 확인
        protected bool m_IsAnimationTransitioning;
        // 현재 애니메이션 상태
        protected AnimatorStateInfo m_CurrentStateInfo;
        // 다음 애니메이션 상태
        protected AnimatorStateInfo m_NextStateInfo;
        // 현재 애니메이션 상태 정보 저장
        protected AnimatorStateInfo m_PreviousCurrentStateInfo;
        // 다음 애니메이션 상태 정보 저장
        protected AnimatorStateInfo m_PreviousNextStateInfo;
        // 상태 전환 중인지 확인 내용 저장
        protected bool m_PreviousIsAnimationTransitioning;

        // 애니메이션 파라미터
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        readonly int m_HashAngleDeltaRad = Animator.StringToHash("AngleDeltaRad");
        readonly int m_HashInputDetected = Animator.StringToHash("InputDetected");
        readonly int m_HashAirborneVerticalSpeed = Animator.StringToHash("AirborneVerticalSpeed"); 
        readonly int m_HashGround = Animator.StringToHash("Ground"); 
        readonly int m_HashTimeoutIdle = Animator.StringToHash("TimeoutIdle"); 
        readonly int m_HashMeleeAttack = Animator.StringToHash("MeleeAttack"); 
        readonly int m_HashStateTime = Animator.StringToHash("StateTime"); 

        // 애니메이션 상태 해시값
        readonly int m_HashLocomotion = Animator.StringToHash("Locomotion");
        readonly int m_HashAirborne = Animator.StringToHash("Airborne");
        readonly int m_HashLanding = Animator.StringToHash("Landing");
        readonly int m_HashEllenCombo1 = Animator.StringToHash("EllenCombo1");
        readonly int m_HashEllenCombo2 = Animator.StringToHash("EllenCombo2");
        readonly int m_HashEllenCombo3 = Animator.StringToHash("EllenCombo3");
        readonly int m_HashEllenCombo4 = Animator.StringToHash("EllenCombo4");

        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            m_Damageable = GetComponent<Damageable>();
            m_Damageable.onDamgeMessageReceviers.Add(this);
            m_Damageable.IsInvulnerable = true;

            EquipMeeleWeapon(false);
        }

        private void OnDisable()
        {
            m_Damageable.onDamgeMessageReceviers.Remove(this);
        }

        private void FixedUpdate()
        {
            CachAnimatorState();
            EquipMeeleWeapon(IsWeaponEquiped());
            CalculateForwardMovement();
            CalculateVerticalMovement();
            SetTargetRotation();
            AttackState();

            if (IsOrientationUpdate() && IsMoveInput)
            {
                UpdateOrientation();
            }

            TimeoutToIdle();

        }

        void Init()
        { 
            m_Animator = GetComponent<Animator>();
            m_CharCtrl = GetComponent<CharacterController>();
            m_Input = GetComponent<PlayerInputAction>();
            m_CameraSettings = FindAnyObjectByType<CameraSettings>();

            if (m_CameraSettings != null)
            {
                if (m_CameraSettings.follow == null)
                {
                    m_CameraSettings.follow = transform;
                }
                if (m_CameraSettings.lookAt == null)
                {
                    m_CameraSettings.lookAt = transform.GetChild(0);
                }
            }

            meeleWeapon.SetOwner(gameObject);
        }

        // 이동상태 대기에서 대기시간(5초)이 경과 되면 Idle 상태
        void TimeoutToIdle()
        {
            // 입력값 체크(이동, 공격)
            bool inputDetected = IsMoveInput || m_Input.IsJump || m_InAttack;

            // 타이머 카운트
            if (m_IsGround && !inputDetected)
            {
                m_IdleTimer += Time.deltaTime;

                if (m_IdleTimer >= idleTimeout)
                {
                    m_Animator.SetTrigger(m_HashTimeoutIdle);

                    // 초기화
                    m_IdleTimer = 0f;
                }
            }
            else
            {
                // 초기화
                m_IdleTimer = 0f;
                m_Animator.ResetTrigger(m_HashTimeoutIdle);
            }

            // 애니 입력값 설정
            m_Animator.SetBool(m_HashInputDetected, inputDetected);
        }

        // (Forward) 이동속도 계산
        void CalculateForwardMovement()
        {
            Vector2 moveInput = m_Input.Move;

            if (moveInput.sqrMagnitude > 1f)
            {
                moveInput.Normalize();
            }

            m_DesiredForwardSpeed = moveInput.magnitude * maxForwardSpeed;

            // 입력에 따라서 가속, 감속 결정
            float acceleration = IsMoveInput ? k_GroundAcceleration : k_GroundDeceleration;

            // 현재 이동속도를 구한다
            m_ForwardSpeed = Mathf.MoveTowards(m_ForwardSpeed, m_DesiredForwardSpeed, acceleration * Time.deltaTime);

            // 애니메이션 입력값 설정
            m_Animator.SetFloat(m_HashForwardSpeed, m_ForwardSpeed);
        }

        // 수직(Vertical) 이동속도 계산
        void CalculateVerticalMovement()
        {
            if (m_IsGround)
            { 
                m_ReadyToJump = true;
            }
            else
            {
                m_Input.IsJump = false;
            }

            if (m_IsGround)
            {
                m_VerticalSpeed = -gravity * k_StickingGravityPropotion;
                
                if(m_Input.IsJump && m_ReadyToJump)
                {
                    m_VerticalSpeed = jumpSpeed;
                    
                    m_IsGround = false;
                    m_ReadyToJump = false;
                    m_Input.IsJump = false;
                }
            }
            else
            {
                if (!m_Input.IsJump && m_VerticalSpeed > 0f)
                { 
                     m_VerticalSpeed -= k_JumpAbortSpeed * Time.deltaTime;
                }

                if (Mathf.Approximately(m_VerticalSpeed, 0f))
                {
                    m_VerticalSpeed = 0f;
                }

                m_VerticalSpeed -= gravity * Time.deltaTime;
            }

        }

        // 이동방향 계산
        void SetTargetRotation()
        {
            Vector2 moveInput = m_Input.Move;
            Vector3 localMovementDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

            // TODO : Camera forward
            //Vector3 forward = Vector3.forward;
            Vector3 forward = Quaternion.Euler(0f,
                m_CameraSettings.freeLookCam.GetComponent<CinemachineOrbitalFollow>().HorizontalAxis.Value,
                0f) * Vector3.forward;

            forward.y = 0f;
            forward.Normalize();

            Quaternion targetRot;

            if (Mathf.Approximately(Vector3.Dot(localMovementDirection, Vector3.forward), -1.0f))
            { 
                targetRot = Quaternion.LookRotation(-forward);
            }
            else
            {
                Quaternion cameraToInputOffset = Quaternion.FromToRotation(Vector3.forward, localMovementDirection);

                targetRot = Quaternion.LookRotation(cameraToInputOffset * forward);
            }

            Vector3 resultingForward = targetRot * Vector3.forward;

            float angleCurrent = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
            float angleTarget = Mathf.Atan2(resultingForward.x, resultingForward.z) * Mathf.Rad2Deg;

            m_AngleDiff = Mathf.DeltaAngle(angleCurrent, angleTarget);
            m_TargetRotation = targetRot;
        }

        void UpdateOrientation()
        {
            // 애니메이션 입력값 설정
            m_Animator.SetFloat(m_HashAngleDeltaRad, m_AngleDiff * Mathf.Deg2Rad);

            // 회전 구현
            Vector3 localInput = new Vector3(m_Input.Move.x, 0f, m_Input.Move.y);

            float groundedTurnSpeed = Mathf.Lerp(maxTurnSpeed, minTurnSpeed, m_ForwardSpeed / m_DesiredForwardSpeed);
            // 공중 회전 속도, 그라운드 회전 속도
            float acutalTurnSpeed = groundedTurnSpeed;

            m_TargetRotation = Quaternion.RotateTowards(transform.rotation, m_TargetRotation, acutalTurnSpeed * Time.deltaTime);
        
            transform.rotation = m_TargetRotation;
        }

        // 애니메이션 상태값 구하기
        void CachAnimatorState()
        {
            m_PreviousCurrentStateInfo = m_CurrentStateInfo;
            m_PreviousNextStateInfo = m_NextStateInfo;
            m_PreviousIsAnimationTransitioning = m_IsAnimationTransitioning;

            m_CurrentStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
            m_NextStateInfo = m_Animator.GetNextAnimatorStateInfo(0);
            m_IsAnimationTransitioning = m_Animator.IsInTransition(0);

        }

        bool IsOrientationUpdate()
        {
            // 이동
            bool updateOrientationForLocomotion = (!m_IsAnimationTransitioning &&
                m_CurrentStateInfo.shortNameHash == m_HashLocomotion || 
                m_NextStateInfo.shortNameHash == m_HashLocomotion);

            // 점프
            bool updateOrientationForAirborne = (!m_IsAnimationTransitioning &&
                m_CurrentStateInfo.shortNameHash == m_HashAirborne ||
                m_NextStateInfo.shortNameHash == m_HashAirborne);

            // 착지
            bool updateOrientationForLanding = (!m_IsAnimationTransitioning &&
                m_CurrentStateInfo.shortNameHash == m_HashLanding ||
                m_NextStateInfo.shortNameHash == m_HashLanding);

            return updateOrientationForLocomotion || updateOrientationForAirborne || 
                updateOrientationForLanding || m_InCombo && !m_InAttack;
        }

        // 공격처리
        void AttackState()
        {
            m_Animator.ResetTrigger(m_HashMeleeAttack);

            m_Animator.SetFloat(m_HashStateTime, 
                Mathf.Repeat(m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));

            if (m_Input.Attack)
            {
                m_Animator.SetTrigger(m_HashMeleeAttack);
            }


        }

        // 무기를 사용하는 상태인지
        bool IsWeaponEquiped()
        {
            bool equiped = m_NextStateInfo.shortNameHash == m_HashEllenCombo1 || 
                m_CurrentStateInfo.shortNameHash == m_HashEllenCombo1;
            equiped |= m_NextStateInfo.shortNameHash == m_HashEllenCombo2 || 
                m_CurrentStateInfo.shortNameHash == m_HashEllenCombo2;
            equiped |= m_NextStateInfo.shortNameHash == m_HashEllenCombo3 || 
                m_CurrentStateInfo.shortNameHash == m_HashEllenCombo3;
            equiped |= m_NextStateInfo.shortNameHash == m_HashEllenCombo4 ||
                m_CurrentStateInfo.shortNameHash == m_HashEllenCombo4;

            return equiped;
        }

        void EquipMeeleWeapon(bool equiped)
        {
            meeleWeapon.gameObject.SetActive(equiped);
            m_InAttack = false;
            m_InCombo = equiped;

            if(!equiped)
            {
                m_Animator.ResetTrigger(m_HashMeleeAttack);
            }
        }

        public void MeleeAttackStart(int throwing = 0)
        {
            meeleWeapon.BeginAttack(throwing != 0);
            m_InAttack = true;
        }

        public void MeleeAttackEnd()
        {
            meeleWeapon.EndAttack();
            m_InAttack = false;
        }

        // 메시지 인터페이스 기능
        public void OnReceiveMessage(MessageType type, object sender, object msg)
        {
            switch (type)
            {
                case MessageType.Damaged:
                    {
                        Damageable.DamageMessage damageData = (Damageable.DamageMessage)msg;
                        Damaged(damageData);
                    }
                    break;
                case MessageType.Death:
                    {
                        Damageable.DamageMessage damageData = (Damageable.DamageMessage)msg;
                        Die(damageData);
                    }
                    break;

            }
        }

        // 데미지 처리, 애니메이션, 연출(vfx, sfx), ... 
        void Damaged(Damageable.DamageMessage damageMessage)
        {
            // TODO
        }

        // 죽음 처리, 애니메이션, 연출(vfx, sfx), ... 
        void Die(Damageable.DamageMessage damageMessage)
        {
            // TODO
            Destroy(gameObject);
        }

        private void OnAnimatorMove()
        {
            Vector3 movement;

            if (m_IsGround)
            {
                RaycastHit hit;

                Ray ray = new Ray(transform.position + Vector3.up * k_GroundRayDistacne * 0.5f, -Vector3.up);

                if (Physics.Raycast(ray, out hit, k_GroundRayDistacne, Physics.AllLayers,
                    QueryTriggerInteraction.Ignore))
                {
                    movement = Vector3.ProjectOnPlane(m_Animator.deltaPosition, hit.normal);
                }
                else
                { 
                    movement = m_Animator.deltaPosition;
                }
            }
            else 
            {
                movement = m_ForwardSpeed * transform.forward * Time.deltaTime;
            }

            // m_VerticalSpeed 스피드 적용
            movement += m_VerticalSpeed * Vector3.up * Time.deltaTime;

            // 회전값 설정
            m_CharCtrl.transform.rotation *= m_Animator.deltaRotation;

            // 이동 설정
            m_CharCtrl.Move(movement);

            // 애니메이션 적용
            m_IsGround = m_CharCtrl.isGrounded;

            if (!m_IsGround)
            {
                m_Animator.SetFloat(m_HashAirborneVerticalSpeed, m_VerticalSpeed);
            }

            m_Animator.SetBool(m_HashGround, m_IsGround);
        }

        public bool PickUpItem(ItemObject _itemObj)
        {
            Item newitem = _itemObj.CreateItem();

            return inventory.AddItem(newitem, 1);
        }

     
    }
}