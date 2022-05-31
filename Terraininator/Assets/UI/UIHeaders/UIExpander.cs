using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
//Use this little class to create submenus that can expand at the click of a button.
//  ToggleExpand() is the method you need to call for this.
public class UIExpander : MonoBehaviour
{
    bool expanded = false;
    Image plusOrMinus;

    [SerializeField]
    GameObject ContentObject; //Stuff that needs to be hidden when not expanded should be a child of this.

    [SerializeField]
    Sprite Plus;
    [SerializeField]
    Sprite Minus;
    [SerializeField]
    Image HeaderObject; //The header that this Expander expands.
    [SerializeField]
    Sprite ExpandedSprite;
    [SerializeField]
    Sprite ImpandedSprite; //The opposite of expanded... ?

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
