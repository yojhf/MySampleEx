using UnityEngine;

namespace MySampleEx
{
    public enum MessageType
    { 
        Damaged,
        Death,
        Respawn
    }

    // IMessageReceiver�� ��ӹ޴� Ŭ������ MessageType ������ȯ ����
    public interface IMessageReceiver
    {
        void OnReceiveMessage(MessageType type, object sender, object msg);
        
    }

    public class MessageSystem : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}