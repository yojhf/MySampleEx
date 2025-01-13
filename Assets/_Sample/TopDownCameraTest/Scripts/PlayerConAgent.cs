using UnityEngine;
using UnityEngine.AI;

namespace MySampleEx
{
    // 플레이어(Agent) 캐릭터 제어(이동, 점프, 애니메이션....) 관리 클래스
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

        // 이동 입력값 체크
        protected bool IsMoveInput
        {
            get { return !Mathf.Approximately(m_NavMeshAgent.velocity.magnitude, 0f); }
        }

        // 대기 상태로 보내기
        // 이동상태 대기에서 5초가 지나면 대기 상태로 진입
        public float idleTimeout = 5f;
        // 타이머 카운트
        protected float m_IdleTimer = 0f;


        // 애니메이션 파라미터
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
            // Agent의 위치값 적용하지 않는다
            m_NavMeshAgent.updatePosition = false;
            // Agent의 회전값 적용
            m_NavMeshAgent.updateRotation = true;

            m_Camera = Camera.main;
        }

        void TimeoutToIdle()
        {
            // 입력값 체크(이동, 공격)
            bool inputDetected = IsMoveInput;

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

        void CalculateForwardMovement()
        { 
            if(m_Input.Click)
            {
                // 마우스 위치로 부터 맵상의 위치를 얻어옴
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

                // 초기화
                m_Input.Click = false;
            }
        }

        private void OnAnimatorMove()
        {
            // 캐릭터 위치 보정
            Vector3 pos = m_NavMeshAgent.nextPosition;

            m_Animator.rootPosition = pos;
            transform.position = pos;

            // 이동
            if (m_NavMeshAgent.remainingDistance > m_NavMeshAgent.stoppingDistance)
            {
                m_CharCtrl.Move(m_NavMeshAgent.velocity * Time.deltaTime);
            }
            else
            {
                m_CharCtrl.Move(Vector3.zero);
            }

            // 애니메이터 적용
            m_Animator.SetFloat(m_HashForwardSpeed, m_NavMeshAgent.velocity.magnitude);
            m_Animator.SetBool(m_HashGround, m_IsGround);
        }
    }
}