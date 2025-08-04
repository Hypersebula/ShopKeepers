using UnityEngine;

public class OrientationVerticalBlocker : MonoBehaviour
{
    private void Update()
    {
        Vector3 euler = transform.eulerAngles;
        euler.x = 0f;
        transform.eulerAngles = euler;
    }
}
