using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainPlayButton : MonoBehaviour {

    private Text buttonText;

    void Awake()
    {

    }

    public void OnClick()
    {
        Application.LoadLevel("main");
    }
}
