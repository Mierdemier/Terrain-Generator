using UnityEngine;
using TMPro;
using UnityEngine.UI;

//A selector is a component that should be attached to a menu to help with selecting one from multiple objects.
//It assumes we have a row of selectable objects, three of which can be displayed at a time.
//  ChangeSelection(int delta) switched the current selection by delta (usually 1 or -1)
//      This automatically updates the display.
//  GetSelected() returns the currently selected object.

//Unity does not allow you to make generic components :(
//So we will have to create a copy of this class for every type of selector (luckily only 2 right now)
public class UICurveSelector : MonoBehaviour
{
    //These arrays should be aligned!
    //  That is to say: they should all be the same length, and values at the same index should correspond with
    //  each other. For example, Names[i] is the name of Objects[i].
    //I would make a struct (string, Sprite, Objet), but the Unity Inspector breaks if I do that :(.
    [Header("Selectables")]
    [SerializeField]
    string[] Names;
    [SerializeField]
    Sprite[] Sprites;
    [SerializeField]
    AnimationCurve[]  Objects;

    [Header("Display")]
    [SerializeField]
    Image previousImage;
    [SerializeField]
    Image currentImage;
    [SerializeField]
    Image nextImage;
    [SerializeField]
    TMP_Text nameText;  //Optional. Leave it as null if you don't need it.

    int selection = 0;

    void Start()
    {
        //Initialise display.
        ChangeSelection(0);
    }

    public void ChangeSelection(int delta)
    {
        //(The added Names.Length stops negative values from breaking everything.)
        selection = (selection + Names.Length + delta) % Names.Length;
        int next = (selection + 1) % Names.Length;
        int prev = (selection + Names.Length - 1) % Names.Length;

        previousImage.sprite = Sprites[prev];
        currentImage.sprite = Sprites[selection];
        nextImage.sprite = Sprites[next];

        if (nameText != null)
            nameText.text = Names[selection];
    }

    public AnimationCurve GetSelected()
    {
        return Objects[selection];
    }

    //For the rare case that you need to know the actual index and not just the object.
    public int GetIndex() {return selection;}
}