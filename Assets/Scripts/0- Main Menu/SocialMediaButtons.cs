using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialMediaButtons : MonoBehaviour
{
    public void SocialMediaButton(int number)
    {
        switch (number)
        {
            case 0: Application.OpenURL("https://www.instagram.com/furkann.caliskan/"); break;
            case 1: Application.OpenURL("https://twitter.com/Furfanim"); break;
            case 2: Application.OpenURL("https://www.youtube.com/@furkancaliskan2022"); break;
        }
    }
}
