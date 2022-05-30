using UnityEngine;
using UnityEngine.UI;

//This small UI class keeps track of a tab system.
//Use the two methods to open and close tabs:
//  Close(int i) closes the tab with index i.
//  Open(int i) you'r never gonna guess this but it opens the tab with index i.
//      It also automatically closes all other tabs since that is the whole point of having tabs.
public class UITabs : MonoBehaviour
{
    //All of these arrays need to line up.
    //  That is: They should all have the same length, and elements at the same index correspond to each other.
    //  For example: TabButtons[i] is the button that opens TabPanels[i].
    //I would use a struct (GameObject, Image, Sprite, Sprite) but that makes the Unity Inspector sad.
    [SerializeField]
    GameObject[] TabPanels;
    [SerializeField]
    Image[] TabButtons;
    [SerializeField]
    Sprite[] NormalSprites; //For the buttons, not the panels.
    [SerializeField]
    Sprite[] HLSprites;     //Highlighted sprite is displayed on the tab button when tab is open.

    void Start()
    {
        for (int i = 0; i < TabButtons.Length; i++)
            Close(i);
    }

    public void Close(int index)
    {
        if(TabPanels[index].activeSelf)
        {
            TabPanels[index].SetActive(false);
            TabButtons[index].sprite = NormalSprites[index];
        }
    }

    public void Open(int index)
    {
        for (int i = 0; i < TabPanels.Length; i++)
            Close(i);

        TabPanels[index].SetActive(true);
        TabButtons[index].sprite = HLSprites[index];
    }
}
