using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace upx.level
{
    [CreateAssetMenu(menuName = "Assets/ New Level Data")]
    public class LevelData : ScriptableObject
    {
        public string levelName;
        public Sprite levelImage;
    }
}
