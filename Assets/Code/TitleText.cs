using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleText : MonoBehaviour
{
    public int Count = 1;
    Text uiText1;
    Text uiText2;
    Text uiText3;

    public enum Scene
    {
        Title,
        Game
    }

    void Start()
    {

        Transform childTransform1 = transform.Find("New Game");
        Transform childTransform2 = transform.Find("Continue");
        Transform childTransform3 = transform.Find("Exit");
        if (childTransform1 != null)
        {
            uiText1 = childTransform1.GetComponent<Text>();
            //Debug.Log(uiText1);

        }
        if (childTransform2 != null)
        {
            uiText2 = childTransform2.GetComponent<Text>();
            //Debug.Log(uiText2);
        }
        if (childTransform3 != null)
        {
            uiText3 = childTransform3.GetComponent<Text>();
            //Debug.Log(uiText3);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Count < 3)
                Count++;
            else if (Count >= 3)
                Count = 1;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Count <= 1)
                Count = 3;
            else if (Count > 1)
                Count--;
        }
        TextColor();
    }

    void TextColor()
    {
        switch (Count)
        {
            case 1:

                uiText1.color = Color.yellow;
                uiText2.color = Color.gray;
                uiText3.color = Color.gray;
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    SceneManager.LoadScene(Scene.Game.ToString());
                }
                break;
            case 2:

                uiText1.color = Color.gray;
                uiText2.color = Color.yellow;
                uiText3.color = Color.gray;
                break;
            case 3:

                uiText1.color = Color.gray;
                uiText2.color = Color.gray;
                uiText3.color = Color.yellow;
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    Application.Quit();
                }
                break;
            default:
                break;

        }
    }
}
