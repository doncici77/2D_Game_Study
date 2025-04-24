using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    public GameObject fill;

    public void UpdateHealthBar(float currentHp, float maxHp)
    {
        if(currentHp <= 0)
        {
            currentHp = 0;
            fill.SetActive(false);

            return;
        }

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
