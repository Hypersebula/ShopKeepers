using UnityEngine;

public class DeathCount : MonoBehaviour
{
    public float deathCount = 0f;
    public float surrenderTeshold = 1f;
    public GameObject Unhide;

    private void Start()
    {
        deathCount = 0f;
        Unhide.SetActive(false);
    }

    private void Update()
    {
        if(deathCount > surrenderTeshold)
        {
            Unhide.SetActive(true);
        }
    }
}
