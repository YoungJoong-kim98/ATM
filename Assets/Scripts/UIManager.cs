using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public PopupBank popupBank;

    public GameObject objPopUpBank;
    public GameObject objPopUpLogin;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        popupBank = GetComponentInChildren<PopupBank>();
        objPopUpLogin.SetActive(true);
        objPopUpBank.SetActive(false);
    }


    public void OnBankPopUp() //은행 화면
    {
        objPopUpLogin.SetActive(false);
        objPopUpBank.SetActive(true);
    }
    public void OnLoginPopUp() // 로그인 화면
    {
        objPopUpLogin.SetActive(true);
        objPopUpBank.SetActive(false);
    }
}
