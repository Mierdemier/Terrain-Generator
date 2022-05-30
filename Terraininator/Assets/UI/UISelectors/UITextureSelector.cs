using UnityEngine;
using UnityEngine.UI;

//Unity does not allow you to make generic components :(
//So we will have to create a copy of this class for every type of selector (luckily only 2 right now)
public class UITextureSelector : MonoBehaviour
{
    [SerializeField]
    string[] Names;
    [SerializeField]
    Sprite[] Sprites;
    [SerializeField]
    Texture2D[]  Objects;

    [SerializeField]
    Image previousImage;
    [SerializeField]
    Image currentImage;
    [SerializeField]
    Image nextImage;
    [SerializeField]
    Text nameText;

    int selection = 0;

    void Start()
    {
        ChangeSelection(0);
    }

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

    public Texture2D GetSelected()
    {
        return Objects[selection];
    }
}