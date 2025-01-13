using UnityEngine;

namespace MySampleEx
{
    public enum MessageType
    { 
        Damaged,
        Death,
        Respawn
    }

    // IMessageReceiver를 상속받는 클래스의 MessageType 상태전환 전달
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