using UnityEngine;

namespace MySampleEx
{
    // 데미지 메세지 정의 클래스
    public partial class Damageable : MonoBehaviour
    {
        // 메세지 리시버를 통해 전달할 데미지 내용
        public struct DamageMessage
        {
            public MonoBehaviour damager;
            public int amount;
            
            public Vector3 direction;
            public Vector3 damageSource;
            public bool throwing;

            public bool stopCamera;

        }
    }
}