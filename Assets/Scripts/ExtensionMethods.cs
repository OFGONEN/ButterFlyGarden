using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFStudio
{
    public static class ExtensionMethods
    {
        public static Vector2 ReturnV2FromUnSignedAngle(this float angle)
        {
            var _angle = (int)angle;
            switch (_angle)
            {
                case 0: return Vector2.up;
                case 90: return Vector2.right;
                case 180: return Vector2.down;
                case 270: return Vector2.left;
                default: return Vector2.zero;
            }
        }
    }
}

