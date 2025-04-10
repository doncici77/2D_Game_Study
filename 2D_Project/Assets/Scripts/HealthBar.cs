using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;

    public void UpdateHealthBar(float currentHp, float maxHp)
    {
        slider.value = currentHp / maxHp;
    }

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
