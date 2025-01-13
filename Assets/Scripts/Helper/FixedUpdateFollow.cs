using UnityEngine;

namespace MySampleEx
{
    // ���� ������Ʈ�� Ư�� ������Ʈ�� ��ġ�� ����
    [DefaultExecutionOrder(9999)]
    public class FixedUpdateFollow : MonoBehaviour
    {
        // ������ ���
        public Transform toFollow;

        // Update is called once per frame
        void Update()
        {
            transform.position = toFollow.position;
            transform.rotation = toFollow.rotation;
        }
    }
}