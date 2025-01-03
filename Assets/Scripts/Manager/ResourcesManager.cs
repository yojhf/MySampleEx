using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace MySampleEx
{
    // 리소스 로드
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