using Unity.Cinemachine;
using UnityEngine;

namespace MySampleEx
{
    // �÷��̾� ĳ���� ����(�̵�, ����, �ִϸ��̼�....) ���� Ŭ����
    public class PlayerCon : MonoBehaviour, IMessageReceiver
    {
        //
        public InventoryObject inventory;
        public InventoryObject equipment;

        // �÷��̾� �ְ� �̵��ӵ�
        public float maxForwardSpeed = 0f;
        // �÷��̾� ���� ȸ�� �ӵ�
        public float minTurnSpeed = 400f;
        // �÷��̾� �ְ� ȸ�� �ӵ�
        public float maxTurnSpeed = 1200f;

        protected PlayerInputAction m_Input;
        protected CharacterController m_CharCtrl;
        protected Animator m_Animator;
        protected Damageable m_Damageable;
        
        protected bool m_IsGround = true;
        // �÷��̾� �Է¿� ���� �� �� �ִ� �ְ� �ӵ�
        protected float m_DesiredForwardSpeed;
        // �÷��̾� ���� �ӵ�
        protected float m_ForwardSpeed;
        // Ÿ���� ���� ȸ����
        protected Quaternion m_TargetRotation;
        // �÷��̾��� ȸ������ Ÿ���� ȸ������ ����
        protected float m_AngleDiff;

        // �̵� �Է°� üũ
        protected bool IsMoveInput
        {
            get { return !Mathf.Approximately(m_Input.Move.sqrMagnitude, 0f); }
        }

        // ����
        // �߷°�
        public float gravity = 20f;
        // ���� �ӵ�
        public float jumpSpeed = 10f;

        // ���� �غ� �ܰ�
        protected bool m_ReadyToJump = false;
        // ���� �̵� �ӵ�
        protected float m_VerticalSpeed;

        // ��� ���·� ������
        // �̵����� ��⿡�� 5�ʰ� ������ ��� ���·� ����
        public float idleTimeout = 5f;
        // Ÿ�̸� ī��Ʈ
        protected float m_IdleTimer = 0f;

        // ī�޶�(������)
        public CameraSettings m_CameraSettings;

        // ����
        public MeeleWeapon meeleWeapon;
        // ���� ���� �Ǵ�
        protected bool m_InAttack = false;
        // ���� ���� ����
        protected bool m_InCombo = false;


        // ���
        // �̵� ���ӵ� ��
        const float k_GroundAcceleration = 20f;
        // �̵� ���ӵ� ��
        const float k_GroundDeceleration = 25f;
        // �׶��� üũ �� ���� �Ÿ� ��
        const float k_GroundRayDistacne = 1f;
        // ���� �ӵ�
        const float k_JumpAbortSpeed = 10f;
        // 
        const float k_StickingGravityPropotion = 0.3f;

        // �ִϸ��̼� ����
        // �ִϸ��̼� ��ȯ Ȯ��
        protected bool m_IsAnimationTransitioning;
        // ���� �ִϸ��̼� ����
        protected AnimatorStateInfo m_CurrentStateInfo;
        // ���� �ִϸ��̼� ����
        protected AnimatorStateInfo m_NextStateInfo;
        // ���� �ִϸ��̼� ���� ���� ����
        protected AnimatorStateInfo m_PreviousCurrentStateInfo;
        // ���� �ִϸ��̼� ���� ���� ����
        protected AnimatorStateInfo m_PreviousNextStateInfo;
        // ���� ��ȯ ������ Ȯ�� ���� ����
        protected bool m_PreviousIsAnimationTransitioning;

        // �ִϸ��̼� �Ķ����
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        readonly int m_HashAngleDeltaRad = Animator.StringToHash("AngleDeltaRad");
        readonly int m_HashInputDetected = Animator.StringToHash("InputDetected");
        readonly int m_HashAirborneVerticalSpeed = Animator.StringToHash("AirborneVerticalSpeed"); 
        readonly int m_HashGround = Animator.StringToHash("Ground"); 
        readonly int m_HashTimeoutIdle = Animator.StringToHash("TimeoutIdle"); 
        readonly int m_HashMeleeAttack = Animator.StringToHash("MeleeAttack"); 
        readonly int m_HashStateTime = Animator.StringToHash("StateTime"); 

        // �ִϸ��̼� ���� �ؽð�
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

        // �̵����� ��⿡�� ���ð�(5��)�� ��� �Ǹ� Idle ����
        void TimeoutToIdle()
        {
            // �Է°� üũ(�̵�, ����)
            bool inputDetected = IsMoveInput || m_Input.IsJump || m_InAttack;

            // Ÿ�̸� ī��Ʈ
            if (m_IsGround && !inputDetected)
            {
                m_IdleTimer += Time.deltaTime;

                if (m_IdleTimer >= idleTimeout)
                {
                    m_Animator.SetTrigger(m_HashTimeoutIdle);

                    // �ʱ�ȭ
                    m_IdleTimer = 0f;
                }
            }
            else
            {
                // �ʱ�ȭ
                m_IdleTimer = 0f;
                m_Animator.ResetTrigger(m_HashTimeoutIdle);
            }

            // �ִ� �Է°� ����
            m_Animator.SetBool(m_HashInputDetected, inputDetected);
        }

        // (Forward) �̵��ӵ� ���
        void CalculateForwardMovement()
        {
            Vector2 moveInput = m_Input.Move;

            if (moveInput.sqrMagnitude > 1f)
            {
                moveInput.Normalize();
            }

            m_DesiredForwardSpeed = moveInput.magnitude * maxForwardSpeed;

            // �Է¿� ���� ����, ���� ����
            float acceleration = IsMoveInput ? k_GroundAcceleration : k_GroundDeceleration;

            // ���� �̵��ӵ��� ���Ѵ�
            m_ForwardSpeed = Mathf.MoveTowards(m_ForwardSpeed, m_DesiredForwardSpeed, acceleration * Time.deltaTime);

            // �ִϸ��̼� �Է°� ����
            m_Animator.SetFloat(m_HashForwardSpeed, m_ForwardSpeed);
        }

        // ����(Vertical) �̵��ӵ� ���
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

        // �̵����� ���
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
            // �ִϸ��̼� �Է°� ����
            m_Animator.SetFloat(m_HashAngleDeltaRad, m_AngleDiff * Mathf.Deg2Rad);

            // ȸ�� ����
            Vector3 localInput = new Vector3(m_Input.Move.x, 0f, m_Input.Move.y);

            float groundedTurnSpeed = Mathf.Lerp(maxTurnSpeed, minTurnSpeed, m_ForwardSpeed / m_DesiredForwardSpeed);
            // ���� ȸ�� �ӵ�, �׶��� ȸ�� �ӵ�
            float acutalTurnSpeed = groundedTurnSpeed;

            m_TargetRotation = Quaternion.RotateTowards(transform.rotation, m_TargetRotation, acutalTurnSpeed * Time.deltaTime);
        
            transform.rotation = m_TargetRotation;
        }

        // �ִϸ��̼� ���°� ���ϱ�
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
            // �̵�
            bool updateOrientationForLocomotion = (!m_IsAnimationTransitioning &&
                m_CurrentStateInfo.shortNameHash == m_HashLocomotion || 
                m_NextStateInfo.shortNameHash == m_HashLocomotion);

            // ����
            bool updateOrientationForAirborne = (!m_IsAnimationTransitioning &&
                m_CurrentStateInfo.shortNameHash == m_HashAirborne ||
                m_NextStateInfo.shortNameHash == m_HashAirborne);

            // ����
            bool updateOrientationForLanding = (!m_IsAnimationTransitioning &&
                m_CurrentStateInfo.shortNameHash == m_HashLanding ||
                m_NextStateInfo.shortNameHash == m_HashLanding);

            return updateOrientationForLocomotion || updateOrientationForAirborne || 
                updateOrientationForLanding || m_InCombo && !m_InAttack;
        }

        // ����ó��
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

        // ���⸦ ����ϴ� ��������
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

        // �޽��� �������̽� ���
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

        // ������ ó��, �ִϸ��̼�, ����(vfx, sfx), ... 
        void Damaged(Damageable.DamageMessage damageMessage)
        {
            // TODO
        }

        // ���� ó��, �ִϸ��̼�, ����(vfx, sfx), ... 
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

            // m_VerticalSpeed ���ǵ� ����
            movement += m_VerticalSpeed * Vector3.up * Time.deltaTime;

            // ȸ���� ����
            m_CharCtrl.transform.rotation *= m_Animator.deltaRotation;

            // �̵� ����
            m_CharCtrl.Move(movement);

            // �ִϸ��̼� ����
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