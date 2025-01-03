using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace MySampleEx
{
    // ���ҽ� �ε�
    public class ResourcesManager : MonoBehaviour
    {
        public static UnityObject Load(string path)
        { 
            return Resources.Load(path);
        }

        public static GameObject LoadAndInstantiate(string path)
        {
            UnityObject source = Load(path);

            if(source == null)
                return null;

            return Instantiate(source) as GameObject;
        }
    }
}