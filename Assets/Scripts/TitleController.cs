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
    private Animator animator;
    private AudioSource audioSource;

    public AudioClip CursorMove;
    public GameObject TopBlack;
    public GameObject BottomBlack;
    public GameObject LoadText;
    public GameObject MainCamera;
    public GameObject Cursor;
    public GameObject[] OptionObjects;  //Loaded for color change on selection hover

    public float[] cursorY;   //Cursor starting location

    private bool loading = false;
    private bool submenu = false;

    private float moveSpeed = 0.0005f;  //Black Bar movement options
    private float moveAdd = .0035f;

    void Start()
    {
        LoadText.SetActive(false);

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Cursor.transform.position = new Vector3(Cursor.transform.position.x, cursorY[selection], Cursor.transform.position.z);

        SetOptionColors();
    }

    void Update()
    {
        if (!loading)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                if (!submenu)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Default")) //Prevent selection before animation finishes
                    {
                        HandleChoice();
                    }
                } else
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("WaitHelp") || animator.GetCurrentAnimatorStateInfo(0).IsName("WaitCredits"))
                    {
                        submenu = false;
                        animator.SetTrigger("Return");
                    }
                }
            } else if (!submenu && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
            {
                HandleCursorMove();
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
        } else if (selection == (int)Options.Credits)
        {
            submenu = true;
            animator.SetTrigger("Credits");
        } else
        {
            submenu = true;
            animator.SetTrigger("Help");
        }
    }

    private void HandleCursorMove()
    {
        audioSource.PlayOneShot(CursorMove, .4f);
        
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            selection--;
            if (selection < (int)Options.Play)
            {
                selection = (int)Options.Credits;
            }
        }
        else
        {
            selection++;
            if (selection > (int)Options.Credits)
            {
                selection = (int)Options.Play;
            }
        }
        Cursor.transform.position = new Vector3(Cursor.transform.position.x, cursorY[selection], Cursor.transform.position.z);

        SetOptionColors();
    }

    private void SetOptionColors()
    {
        for (int i = 0; i < 3; i++)
        {
            if (selection == i)
            {
                OptionObjects[i].GetComponent<TextMesh>().color = new Vector4(1,1,1,1);
            }
            else
            {
                OptionObjects[i].GetComponent<TextMesh>().color = new Vector4(1,1,1,.5f);
            }
        }
    }

    private void HandleTransition()
    {
        if (TopBlack.transform.position.y > 4)
        {
            moveSpeed += moveAdd;
            TopBlack.transform.position = new Vector3(TopBlack.transform.position.x, TopBlack.transform.position.y - (moveSpeed * Time.deltaTime * 60), TopBlack.transform.position.z);
            BottomBlack.transform.position = new Vector3(BottomBlack.transform.position.x, BottomBlack.transform.position.y + (moveSpeed * Time.deltaTime * 60), BottomBlack.transform.position.z);

            MainCamera.GetComponent<AudioSource>().volume -= .05f * Time.deltaTime * 60;  //Fade out music
        }
        else
        {
            //LoadText.SetActive(true); //Removed for WebGL Build since scene loads too quickly!
            SceneManager.LoadScene("Moon");
        }
    }
}