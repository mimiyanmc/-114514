using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] Menu[] menus;

    private void Awake()
    {
        Instance = this;
    }
    public void openMenu(string menuName)
    {
        for(int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                menus[i].Open(); 
            }
            else if (menus[i].open)
            {
                closeMenu(menus[i]);
            }
        }
    }
    public void openMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                closeMenu(menus[i]);
            }
        }
        menu.Open();
    }
    public void closeMenu(Menu menu)
    {
        menu.Close();
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
