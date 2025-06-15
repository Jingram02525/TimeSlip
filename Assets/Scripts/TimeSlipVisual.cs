using UnityEngine;
using UnityEngine.UI;

public class TimeSlipVisual : MonoBehaviour
{
    public Image overlay;
    public void ShowEffect()
    {
        if (overlay != null)
            overlay.enabled = true;
    }
    public void HideEffect()
    {
        if (overlay != null)
            overlay.enabled = false;
    }
}
