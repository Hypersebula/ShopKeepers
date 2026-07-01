using UnityEngine;

public class CodeLock : MonoBehaviour
{
    public CycleBlock block1;
    public CycleBlock block2;
    public CycleBlock block3;

    public int correctIndex1;
    public int correctIndex2;
    public int correctIndex3;

    public GameObject door;

    private bool solved = false;

    private void Update()
    {
        if (solved) return;

        if (block1.GetCurrentIndex() == correctIndex1 &&
            block2.GetCurrentIndex() == correctIndex2 &&
            block3.GetCurrentIndex() == correctIndex3)
        {
            solved = true;
            if (door != null) Destroy(door);
        }
    }
}