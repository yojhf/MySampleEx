using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MySampleEx
{
    public class MeeleWeapon : MonoBehaviour
    {
        [Serializable]
        public class AttackPoint
        {
            public float radius;
            public Vector3 offset;
            public Transform attackRoot;
#if UNITY_EDITOR
            public List<Vector3> previousPositions = new List<Vector3>();
#endif
        }

        // hit시 데미지 포인트
        public int damage = 1;

        public AttackPoint[] attackPoints = new AttackPoint[0];
        public TimeEffect[] effects;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            for (int i = 0; i < attackPoints.Length; i++)
            {
                AttackPoint apt = attackPoints[i];

                if(apt.attackRoot != null)
                {
                    Vector3 worldPos = apt.attackRoot.TransformVector(apt.offset);
                    Gizmos.color = new Color(1f, 1f, 1f, 0.4f);
                    Gizmos.DrawSphere(apt.attackRoot.position + worldPos, apt.radius);
                }

                if(apt.previousPositions.Count > 0)
                {
                    Handles.DrawAAPolyLine(10, apt.previousPositions.ToArray());
                }
            }
        }
#endif
    }

}