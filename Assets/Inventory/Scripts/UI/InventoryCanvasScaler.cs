using UnityEngine;
using UnityEngine.UI;

public class InventoryCanvasScaler : MonoBehaviour
{
    private CanvasScaler scaler;
    private Vector2 referenceRes = new Vector2(1920, 1080);
    private int lastScreenWidth;
    private int lastScreenHeight;

    private void Awake()
    {
        scaler = GetComponent<CanvasScaler>();
        lastScreenWidth  = Screen.width;
        lastScreenHeight = Screen.height;
        if(scaler != null) UpdateCanvasScale();
    }

    private void Update()
    {
        if(Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            lastScreenWidth  = Screen.width;
            lastScreenHeight = Screen.height;
            UpdateCanvasScale();
        }
    }

    private void UpdateCanvasScale()
    {
        if(Screen.width > Screen.height)
        {
            scaler.referenceResolution = referenceRes;
            scaler.matchWidthOrHeight = 0.5f;
        }
        else
        {
            scaler.referenceResolution = new Vector2(referenceRes.y, referenceRes.x);
            scaler.matchWidthOrHeight = 0f;
        }
    }
}
