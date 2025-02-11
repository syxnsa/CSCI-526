using UnityEngine;

public class Platform : MonoBehaviour
{
    public ColorType platformColor; 
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateColor();
    }

    public void ChangeColor(ColorType newColor)
    {
        platformColor = newColor;
        UpdateColor();
    }

    private void UpdateColor()
    {
        GetComponent<SpriteRenderer>().color = ColorManager.GetColor(platformColor);
    }

}
