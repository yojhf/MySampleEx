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
            // ����Ʈ ó��
            QuestManager.Instance.UpdateQuest(QuestType.Kill, 0);

            Destroy(gameObject);
        }

    }
}