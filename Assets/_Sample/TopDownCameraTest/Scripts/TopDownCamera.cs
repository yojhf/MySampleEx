using UnityEngine;

namespace MySampleEx
{
    // 카메라 시점을 탑 다운 뷰로 관리하는 클래스
    public class TopDownCamera : MonoBehaviour
    {
        // 플레이어
        public Transform target;

        // 카메라 속성
        // 플레이어로 부터의 높이
        [SerializeField] private float height = 5f;
        // 플레이어로 부터의 거리
        [SerializeField] private float distance = 10f;
        // 플레이어로 부터의 회전 각도
        [SerializeField] private float angle = 45f;
        // 카메라 이동속도
        [SerializeField] private float smoothSpeed = 0.5f;

        // 카메라가 바라보는 높이
        [SerializeField] private float lookatHeight = 2f;

        // 카메라 현재 속도
        private Vector3 refVelocity;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            HandleCamera();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            HandleCamera();
        }

        void HandleCamera()
        {
            if (target == null)
                return;

            // 카메라 위치 설정 (플레이어 기준)
            Vector3 worldPos = (target.forward * -distance) + (Vector3.up * height);
            Vector3 rotateVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPos;

            // 높이에 따른 위치 보정
            Vector3 flatTargetPos = target.position;
            flatTargetPos.y += lookatHeight;

            // 카메라가 이동할 최종 위치
            Vector3 finalPos = flatTargetPos + rotateVector;

            // 카메라 이동
            transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref refVelocity, smoothSpeed);

            // 플레이어 바라보기
            transform.LookAt(flatTargetPos);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);

            if (target != null)
            { 
                Vector3 lookAtPos = target.position;

                lookAtPos.y += lookatHeight;

                Gizmos.DrawSphere(lookAtPos, 0.25f);
                Gizmos.DrawLine(transform.position, lookAtPos);
            }

            Gizmos.DrawSphere(transform.position, 0.25f);
        }
    }
}