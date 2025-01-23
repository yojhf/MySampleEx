using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MySampleEx
{
    public class PickUpObject : MonoBehaviour
    {
        [SerializeField] private float speed = 2.0f;
        [SerializeField] private float rotSpeed = 360f;
        [SerializeField] private float pos = 1f;

        private Vector3 startPos;

        public ItemObject itemObject;

        // Start is called before the first frame update
        void Start()
        {
            startPos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            MoveObject();
        }

        void MoveObject()
        {
            float bobingAnimationPhase = Mathf.Sin(Time.time * speed);

            transform.position = startPos + (Vector3.up * bobingAnimationPhase) / pos;

            transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime, Space.World);
        }

        // ¾ÆÀÌÅÛ È¹µæ ¼º°ø, ½ÇÆÐ ¹ÝÈ¯
        protected virtual bool OnPickUp()
        {
            // ¾ÆÀÌÅÛ È¹µæ


            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerCon playerCon = other.GetComponent<PlayerCon>();

                if (playerCon != null)
                {
                    if (playerCon.PickUpItem(itemObject))
                    {
                        // È¹µæ ¼º°ø È¿°ú »ç¿îµå / ÀÌÆåÆ®


                        // Á¦°Å
                        Destroy(gameObject);
                    }

                }


            }
        }

    }
}