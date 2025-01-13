using UnityEngine;

namespace MySampleEx
{
    // ī�޶� ������ ž �ٿ� ��� �����ϴ� Ŭ����
    public class TopDownCamera : MonoBehaviour
    {
        // �÷��̾�
        public Transform target;

        // ī�޶� �Ӽ�
        // �÷��̾�� ������ ����
        [SerializeField] private float height = 5f;
        // �÷��̾�� ������ �Ÿ�
        [SerializeField] private float distance = 10f;
        // �÷��̾�� ������ ȸ�� ����
        [SerializeField] private float angle = 45f;
        // ī�޶� �̵��ӵ�
        [SerializeField] private float smoothSpeed = 0.5f;

        // ī�޶� �ٶ󺸴� ����
        [SerializeField] private float lookatHeight = 2f;

        // ī�޶� ���� �ӵ�
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

            // ī�޶� ��ġ ���� (�÷��̾� ����)
            Vector3 worldPos = (target.forward * -distance) + (Vector3.up * height);
            Vector3 rotateVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPos;

            // ���̿� ���� ��ġ ����
            Vector3 flatTargetPos = target.position;
            flatTargetPos.y += lookatHeight;

            // ī�޶� �̵��� ���� ��ġ
            Vector3 finalPos = flatTargetPos + rotateVector;

            // ī�޶� �̵�
            transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref refVelocity, smoothSpeed);

            // �÷��̾� �ٶ󺸱�
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