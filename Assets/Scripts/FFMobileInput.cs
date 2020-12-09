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
        Debug.Log("Tapped: " + count);
    }

    Vector2 DecideDirection(float unsignedAngle, Vector2 delta)
    {
        if (unsignedAngle >= 150)
        {
            Debug.Log("Left Swipe");
            return Vector2.left;
        }
        else if (60 <= unsignedAngle && unsignedAngle <= 120)
        {
            if (delta.y >= 0)
            {
                Debug.Log("Up Swipe");

                return Vector2.up;
            }
            else
            {
                Debug.Log("Down Swipe");

                return Vector2.down;
            }
        }
        else if (unsignedAngle <= 30)
        {
            Debug.Log("Right Swipe");
            return Vector2.right;
        }
        else
        {
            Debug.Log("Zero Swipe");
            return Vector2.zero;
        }
    }
}
