using UnityEngine;
using UnityEngine.AI;

namespace MySampleEx
{
    // �÷��̾�(Agent) ĳ���� ����(�̵�, ����, �ִϸ��̼�....) ���� Ŭ����
    public class PlayerConAgent : MonoBehaviour
    {
        public GameObject clickEffectPrefab;

        protected PlayerInputAgent m_Input;
        protected CharacterController m_CharCtrl;
        protected Animator m_Animator;
        protected NavMeshAgent m_NavMeshAgent;
        protected Camera m_Camera;

        protected bool m_IsGround = true;

        [SerializeField] protected LayerMask groundLayerMask;

        // �̵� �Է°� üũ
        protected bool IsMoveInput
        {
            get { return !Mathf.Approximately(m_NavMeshAgent.velocity.magnitude, 0f); }
        }

        // ��� ���·� ������
        // �̵����� ��⿡�� 5�ʰ� ������ ��� ���·� ����
        public float idleTimeout = 5f;
        // Ÿ�̸� ī��Ʈ
        protected float m_IdleTimer = 0f;


        // �ִϸ��̼� �Ķ����
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        readonly int m_HashInputDetected = Animator.StringToHash("InputDetected");
        readonly int m_HashGround = Animator.StringToHash("Ground");
        readonly int m_HashTimeoutIdle = Animator.StringToHash("TimeoutIdle");

        private void Awake()
        {
            Init();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            CalculateForwardMovement();
            TimeoutToIdle();
        }

        void Init()
        {
            m_Animator = GetComponent<Animator>();
            m_CharCtrl = GetComponent<CharacterController>();
            m_Input = GetComponent<PlayerInputAgent>();

            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            // Agent�� ��ġ�� �������� �ʴ´�
            m_NavMeshAgent.updatePosition = false;
            // Agent�� ȸ���� ����
            m_NavMeshAgent.updateRotation = true;

            m_Camera = Camera.main;
        }

        void TimeoutToIdle()
        {
            // �Է°� üũ(�̵�, ����)
            bool inputDetected = IsMoveInput;

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

        void CalculateForwardMovement()
        { 
            if(m_Input.Click)
            {
                // ���콺 ��ġ�� ���� �ʻ��� ��ġ�� ����
                Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit, 100f, groundLayerMask))
                {
                    m_NavMeshAgent.SetDestination(hit.point);

                    if (clickEffectPrefab != null)
                    {
                        GameObject effectGo = Instantiate(clickEffectPrefab, hit.point + new Vector3(0f, 0.1f, 0f), clickEffectPrefab.transform.rotation);

                        Destroy(effectGo, 2f);
                    }
                }

                // �ʱ�ȭ
                m_Input.Click = false;
            }
        }

        private void OnAnimatorMove()
        {
            // ĳ���� ��ġ ����
            Vector3 pos = m_NavMeshAgent.nextPosition;

            m_Animator.rootPosition = pos;
            transform.position = pos;

            // �̵�
            if (m_NavMeshAgent.remainingDistance > m_NavMeshAgent.stoppingDistance)
            {
                m_CharCtrl.Move(m_NavMeshAgent.velocity * Time.deltaTime);
            }
            else
            {
                m_CharCtrl.Move(Vector3.zero);
            }

            // �ִϸ����� ����
            m_Animator.SetFloat(m_HashForwardSpeed, m_NavMeshAgent.velocity.magnitude);
            m_Animator.SetBool(m_HashGround, m_IsGround);
        }
    }
}