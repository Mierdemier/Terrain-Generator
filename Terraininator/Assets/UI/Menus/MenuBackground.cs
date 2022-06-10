using UnityEngine;
using UnityEngine.UI;


//A small class that just changes it's image to a random one from a list.
[RequireComponent(typeof(Image))]
public class MenuBackground : MonoBehaviour
{
    [SerializeField]
    Sprite[] Images;

    Image panel;

    void Start()
    {
        panel = GetComponent<Image>();

        int rand = Random.Range(0, Images.Length);
        panel.sprite = Images[rand];
    }

}
