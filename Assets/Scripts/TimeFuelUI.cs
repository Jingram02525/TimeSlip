using UnityEngine;
using UnityEngine.UI;

public class TimeFuelUI : MonoBehaviour
{
    public Slider fuelSlider;
    public void SetFuel(float value)
    {
        fuelSlider.value = value;
    }
}