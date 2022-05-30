using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIExpander : MonoBehaviour
{
    bool expanded = false;
    Image plusOrMinus;

    [SerializeField]
    GameObject ContentObject;
    [SerializeField]
    Sprite Plus;
    [SerializeField]
    Sprite Minus;
    [SerializeField]
    Image HeaderObject;
    [SerializeField]
    Sprite ExpandedSprite;
    [SerializeField]
    Sprite ImpandedSprite;

    void Start()
    {
        plusOrMinus = GetComponent<Image>();
    }

    public void ToggleExpand()
    {
        expanded = !expanded;

        if (expanded)
        {
            ContentObject.SetActive(true);
            plusOrMinus.sprite = Minus;
            HeaderObject.sprite = ExpandedSprite;
            HeaderObject.SetNativeSize();
        }
        else
        {
            ContentObject.SetActive(false);
            plusOrMinus.sprite = Plus;
            HeaderObject.sprite = ImpandedSprite;
            HeaderObject.SetNativeSize();
        }
    }

}
