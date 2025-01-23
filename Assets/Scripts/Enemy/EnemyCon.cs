using UnityEngine;

namespace MySampleEx
{
    public class EnemyCon : MonoBehaviour, IMessageReceiver
    {
        protected Damageable e_Damageable;

        private void OnEnable()
        {
            e_Damageable = GetComponent<Damageable>();
            e_Damageable.onDamgeMessageReceviers.Add(this);
            e_Damageable.IsInvulnerable = true;
        }

        private void OnDisable()
        {
            e_Damageable.onDamgeMessageReceviers.Remove(this);
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
            // 퀘스트 처리
            QuestManager.Instance.UpdateQuest(QuestType.Kill, 0);

            Destroy(gameObject);
        }

    }
}