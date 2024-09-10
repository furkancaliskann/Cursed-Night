using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    private MainMenu mainMenu;
    [SerializeField] private GameObject loadingObject;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    void Awake()
    {
        mainMenu = GetComponent<MainMenu>();
    }

    public void OpenLoadingScreen()
    {
        mainMenu.ClosePanels();
        int randomNumber = Random.Range(0, sprites.Count);
        loadingObject.GetComponent<Image>().sprite = sprites[randomNumber];
        loadingObject.SetActive(true);
    }
}
