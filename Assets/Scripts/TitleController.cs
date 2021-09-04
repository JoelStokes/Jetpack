using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    private enum Options    //Order of options on the main Title Screen
    {
        Play,
        Help,
        Credits
    }
    private int selection = 0;  //Currently highlighted option
    private Vector3 StartPos;   //Cursor starting location

    public AudioClip CursorMove;
    public GameObject TopBlack;
    public GameObject BottomBlack;
    public GameObject LoadText;
    public GameObject MainCamera;

    private bool loading = false;
    private bool submenu = false;

    private float moveSpeed = 0.0001f;  //Black Bar movement options
    private float moveAdd = .0015f;
    private bool moveDone = false;

    void Start()
    {
        StartPos = transform.position;
        LoadText.SetActive(false);
    }

    void Update()
    {
        if (!loading)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                HandleChoice();
            }
        } else
        {
            HandleTransition();
        }
    }

    private void HandleChoice()
    {
        if (selection == (int)Options.Play)
        {
            loading = true;
        }
        //Choose option
    }

    private void HandleTransition()
    {

    }
}