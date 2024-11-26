using UnityEngine;

namespace Data
{
    [UnityEngine.CreateAssetMenu(fileName = "Scriptable Object", menuName = "Data/GemData", order = 0)]
    public class GemData : ScriptableObject
    {
        public float gemSize;
        public float gemMoveSpeed;
    }
}