using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatDIsplayS : MonoBehaviour
{
    public Slider slider;

    public void MaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHelth(int health)
    {
        slider.value = health;
    }
}
