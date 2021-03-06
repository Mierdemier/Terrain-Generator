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
public class UIBrushSelector : MonoBehaviour
{
    [Header("Selectables")]
    [SerializeField]
    string[] Names;
    [SerializeField]
    string[] Descriptions; //Only brushes have descriptions.
    [SerializeField]
    Sprite[] Sprites;
    [SerializeField]
    Brush[]  Objects;

    [Header("Display")]
    [SerializeField]
    Image previousImage;
    [SerializeField]
    Image currentImage;
    [SerializeField]
    Image nextImage;
    [SerializeField]
    TMP_Text nameText;  //Optional. Leave it as null if you don't need it.
    [SerializeField]
    TMP_Text descriptionText; 

    int selection = 0;

    void Start()
    {
        //Initialises display.
        ChangeSelection(0);
    }

    public void ChangeSelection(int towhat)
    {
        //(The added Names.Length stops negative values from breaking everything.)
        selection = (selection + Names.Length + towhat) % Names.Length;
        int next = (selection + 1) % Names.Length;
        int prev = (selection + Names.Length - 1) % Names.Length;

        previousImage.sprite = Sprites[prev];
        currentImage.sprite = Sprites[selection];
        nextImage.sprite = Sprites[next];

        if (nameText != null)
            nameText.text = Names[selection];
        if (descriptionText != null)
            descriptionText.text = Descriptions[selection];
    }

    public Brush GetSelected()
    {
        return Objects[selection];
    }

    public int GetIndex() {return selection;}
}
