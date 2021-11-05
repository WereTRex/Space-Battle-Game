using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider shieldSlider;


    public void SetHealth(float _health)
    {
        healthSlider.value = _health;
    }
    public void SetShields(float _shields)
    {
        shieldSlider.value = _shields;
    }


    public void Setup(float _maxHealth, float _maxSheilds)
    {
        healthSlider.maxValue = _maxHealth;
        shieldSlider.maxValue = _maxSheilds;
    }
}
