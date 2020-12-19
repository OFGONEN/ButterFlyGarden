using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class FFMobileInput : MonoBehaviour
{
    public SwipeInputEvent swipeInputEvent;
    public TapInputEvent tapInputEvent;
    public void Swiped(Vector2 delta)
    {
        swipeInputEvent.swipeDirection = DecideDirection(Vector2.Angle(Vector2.right, delta), delta);

        if (swipeInputEvent.swipeDirection != Vector2.zero)
            swipeInputEvent.Raise();

    }

    public void Tapped(int count)
    {
        tapInputEvent.tapCount = count;

        tapInputEvent.Raise();
    }

    Vector2 DecideDirection(float unsignedAngle, Vector2 delta)
    {
        if (unsignedAngle >= 140)
        {
            return Vector2.left;
        }
        else if (50 <= unsignedAngle && unsignedAngle <= 130)
        {
            if (delta.y >= 0)
            {
                return Vector2.up;
            }
            else
            {
                return Vector2.down;
            }
        }
        else if (unsignedAngle <= 40)
        {
            return Vector2.right;
        }
        else
        {
            return Vector2.zero;
        }
    }
}
