using UnityEngine;
using UnityEngine.UI;

public class ConstText : MonoBehaviour
{
    public string key;
    private Text text;

    public void SetConstVariable(Text text)
    {
        this.text = text;
    }
    public void SetText(string newText)
    {
        text.text = newText;
    }}
