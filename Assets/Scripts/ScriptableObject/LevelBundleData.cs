using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "New LevelBundleData", menuName = "Level Bundle Data", order = 10)]
    public class LevelBundleData : UnityEngine.ScriptableObject
    {
        [SerializeField] private LevelData levelData;

        public LevelData LevelData => levelData;
    }
}