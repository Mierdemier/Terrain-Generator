using UnityEngine;
using UnityEngine.UI;

public class UISelector<T> : MonoBehaviour
{
    [SerializeField]
    string[] Names;
    [SerializeField]
    Sprite[] Sprites;
    [SerializeField]
    <T>[]   Objects;

    [SerializeField]
    Image previousImage;
    [SerializeField]
    Image currentImage;
    [SerializeField]
    Image nextImage;
    [SerializeField]
    Text nameText;

    int selection = 0;

    public void ChangeSelection(int towhat)
    {
        selection = (selection + Names.Length + towhat) % Names.Length;
        int next = (selection + 1) % Names.Length;
        int prev = (selection + Names.Length - 1) % Names.Length;

        previousImage.sprite = Sprites[prev];
        currentImage.sprite = Sprites[selection];
        nextImage.sprite = Sprites[next];

        if (nameText != null)
            nameText.text = Names[selection];
    }

    public <T> GetSelected()
    {
        return Objects[selection];
    }
}
