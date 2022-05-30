using UnityEngine;
using UnityEngine.UI;

public class UITabs : MonoBehaviour
{
    //All of these arrays need to line up.
    [SerializeField]
    GameObject[] TabPanels;
    [SerializeField]
    Image[] TabButtons;
    [SerializeField]
    Sprite[] NormalSprites;
    [SerializeField]
    Sprite[] HLSprites;

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
