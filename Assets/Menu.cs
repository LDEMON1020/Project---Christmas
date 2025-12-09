using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public KeyCode menuKey = KeyCode.Escape;
    public bool isMenuOpen;
    public GameObject menuUI;
    public GameObject SoundMenu;
    void Start()
    {
        menuUI.SetActive(false);
    }

   public void Update()
    {
        if (Input.GetKeyDown(menuKey))
        {
           
            isMenuOpen = !isMenuOpen;
            menuUI.SetActive(isMenuOpen);
            if (isMenuOpen)
            {
                Time.timeScale = 0f; // 게임 일시정지
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if (!isMenuOpen)
            {
                Time.timeScale = 1f; // 게임 재개
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                SoundMenu.SetActive(false);
            }
        }
    }
}
