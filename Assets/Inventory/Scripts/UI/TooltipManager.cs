using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }
    [SerializeField] private Vector2 mouseOffset = new Vector2(20f, -10f);
    private TextMeshProUGUI[] textComponents;
    private TextMeshProUGUI tooltipHeader;
    private TextMeshProUGUI tooltipContent;

    private void Awake()
    {
        textComponents = GetComponentsInChildren<TextMeshProUGUI>(true);
        tooltipHeader  = textComponents[0];
        tooltipContent = textComponents[1];

        tooltipHeader.text  = "";
        tooltipContent.text = "";
        gameObject.SetActive(false);

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ShowTooltip(string header, string desc)
    {
        tooltipHeader.text  = header;
        tooltipContent.text = desc;

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public void UpdatePosition(Vector2 pos)
    {
        Vector2 newPos = pos + mouseOffset;
        transform.position = newPos;
    }
}
