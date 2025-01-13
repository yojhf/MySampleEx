using UnityEngine;

namespace MySampleEx
{
    // 현재 오브젝트를 특정 오브젝트의 위치에 부착
    [DefaultExecutionOrder(9999)]
    public class FixedUpdateFollow : MonoBehaviour
    {
        // 부착할 대상
        public Transform toFollow;

        // Update is called once per frame
        void Update()
        {
            transform.position = toFollow.position;
            transform.rotation = toFollow.rotation;
        }
    }
}