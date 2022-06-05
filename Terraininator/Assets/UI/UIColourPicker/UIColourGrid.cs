using UnityEngine;
using UnityEngine.UI;

public class UIColourGrid : MonoBehaviour
{
    [SerializeField]
    float ConversionFactor;
    [SerializeField]
    RectTransform[] ColourSquares;
    int index = 0;

    //Unfortunately some idiot at Unity decided that sliders can only pass one parameter to each function.
    //So we use this function to tell the script which square to change.
    //And then either ChangeWidth or ChangeHeight to set that square's parameters.
    public void SetSquareIndex (int i)
    {
        index = i;
    }
    public void ChangeSquareWidth(float newSize)
    {
        //You cannot directly set the size of the last square!
        if (index >= ColourSquares.Length - 1)
            return;

        newSize *= ConversionFactor;

        float oldSize = ColourSquares[index].sizeDelta.x;

        ColourSquares[index].sizeDelta = new Vector2(newSize, ColourSquares[index].sizeDelta.y);

        ColourSquares[index + 1].sizeDelta = 
        new Vector2(ColourSquares[index + 1].sizeDelta.x - (newSize - oldSize), ColourSquares[index + 1].sizeDelta.y);
    }
}
