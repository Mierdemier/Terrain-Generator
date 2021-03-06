using UnityEngine;
using UnityEngine.UI;


//This class keeps track of a grid of coloured squares, and changes their size at runtime.
//It tries to keep the spacing of the grid intact.

//Unfortunately due to some quirks in the UnityEngine.UI system we had to write some of these methods in a weird way.
//So make sure you're using them correctly!
public class UIColourGrid : MonoBehaviour
{
    //w = width h = height
    [SerializeField]
    float wConversionFactor;
    [SerializeField]
    float hConversionFactor;
    [SerializeField]
    RectTransform[] ColourSquares;    //Coordinate system: y * Columns + x.
    [SerializeField]
    RectTransform[] RowHolders;
    [SerializeField]
    int Columns;                    //Rows can be inferred from ColourSquares.Length if necessary.
    [SerializeField]
    Slider[] wSliders;
    [SerializeField]
    Slider[] hSliders;
    int column;
    int row;

    //Unfortunately some idiot at Unity decided that sliders can only pass one parameter to each function.
    //So we use this function to tell the script which square to change.
    //And then either ChangeWidth or ChangeHeight to set that square's parameters.
    public void SetColumn(int i)
    {
        column = i;
    }
    public void SetRow(int i)
    {
        row = i;
    }

    //ChangeSquareWidth changes the actual squares themselves.
    public void ChangeSquareWidth(float sliderValue)
    {
        //You cannot directly set the size of the last column!
        if (column >= Columns - 1)
            return;

        //Make sure the sliders don't go outside of their bounds.
        //A slider should not go further than the slider after it.
        if (wSliders.Length > column + 1 && sliderValue > wSliders[column + 1].value)
        {
            wSliders[column].value = wSliders[column + 1].value;
            return;
        }
        if (column > 0 && sliderValue < wSliders[column - 1].value)
        {
            wSliders[column].value = wSliders[column - 1].value;
            return;
        }

        float newSize = sliderValue * wConversionFactor;
        newSize -= column > 0 ? wSliders[column - 1].value * wConversionFactor : 0;

        float oldSize = ColourSquares[column].sizeDelta.x;

        for (int i = column; i < ColourSquares.Length; i += Columns)
        {
            ColourSquares[i].sizeDelta = new Vector2(newSize, ColourSquares[i].sizeDelta.y);

            //The square after this one should also be resized to keep the size of the grid the same.
            ColourSquares[i + 1].sizeDelta = 
            new Vector2(ColourSquares[i + 1].sizeDelta.x - (newSize - oldSize),  ColourSquares[i + 1].sizeDelta.y);
        }
    }

    //ChangeSquareHeight changes the height of a *layout object* that contains a row of squares.
    public void ChangeSquareHeight(float sliderValue)
    {
        //You cannot directly set the size of the last row!
        if (row >= (ColourSquares.Length / Columns) - 1)
            return;

        if (hSliders.Length > row + 1 && sliderValue > hSliders[row + 1].value)
        {
            hSliders[row].value = hSliders[row + 1].value;
            return;
        }
        if (row > 0 && sliderValue < hSliders[row - 1].value)
        {
            hSliders[row].value = hSliders[row - 1].value;
            return;
        }

        float newSize = sliderValue * hConversionFactor;
        newSize -= row > 0 ? hSliders[row - 1].value * hConversionFactor : 0;

        float oldSize = RowHolders[row].sizeDelta.y;

        RowHolders[row].sizeDelta = new Vector2(RowHolders[row].sizeDelta.x, newSize);
        RowHolders[row].GetComponent<HorizontalLayoutGroup>().SetLayoutHorizontal();
        RowHolders[row].GetComponent<HorizontalLayoutGroup>().SetLayoutVertical();

        //The row after this one should also be resized to keep the size of the grid the same.
        RowHolders[row + 1].sizeDelta = 
        new Vector2(RowHolders[row + 1].sizeDelta.x, RowHolders[row + 1].sizeDelta.y - (newSize - oldSize));
        RowHolders[row+1].GetComponent<HorizontalLayoutGroup>().SetLayoutHorizontal();
        RowHolders[row+1].GetComponent<HorizontalLayoutGroup>().SetLayoutVertical();
    }
}