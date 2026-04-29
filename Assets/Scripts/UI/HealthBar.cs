using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public HealthManager healthManager;
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = healthManager.maxHealth;
    }

    private void Update()
    {
        slider.value = healthManager.currentHealth;
    }
}