using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PopupBank : MonoBehaviour
{

    public GameObject atm;         // 입 출금 선택
    public GameObject depositUI;   // 입금 UI 
    public GameObject withdrawalUI; // 출금 UI
    public GameObject transferUI; //송금 UI

    public InputField depositInputField;
    public InputField withdrawalInputField;
    public TMP_InputField transferId;
    public TMP_InputField transferAmount;


    public TextMeshProUGUI TxtName;
    public TextMeshProUGUI TxtBalance;
    public TextMeshProUGUI Txtcash;


    private string saveDirectory;

    private void Start()
    {
        atm.SetActive(true);
        depositUI.SetActive(false);
        withdrawalUI.SetActive(false);
        transferUI.SetActive(false);

        saveDirectory = Path.Combine(Application.dataPath, "UserData");
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
    }

    public void OpenDepositUI()
    {
        depositUI.SetActive(true);
        atm.SetActive(false);
    }


    public void OpenWithdrawalUI()
    {
        withdrawalUI.SetActive(true);
        atm.SetActive(false);
    }
    public void OpenTransferUI()
    {
        transferUI.SetActive(true);
        atm.SetActive(false);
    }

    public void BackToATM() 
    {
        atm.SetActive(true);
        depositUI.SetActive(false);
        withdrawalUI.SetActive(false);
        transferUI.SetActive(false);
    }

    public void Deposit(int money)
    {
        if(GameManager.Instance.userData.cash-money>=0)
        {
            GameManager.Instance.userData.balance += money;
            GameManager.Instance.userData.cash -= money;
            Refresh();
            GameManager.Instance.SaveUserData();
        }

    }

    public void DepositCustom()
    {
        int money;
        if(int.TryParse(depositInputField.text, out money))
        {
            Deposit(money);
        }
        else
        {
            Debug.Log("잘못된 입력입니다.");
        }
    }

    public void Withdrawal(int money)
    {
        if (GameManager.Instance.userData.balance - money >= 0)
        {
            GameManager.Instance.userData.balance -= money;
            GameManager.Instance.userData.cash += money;
            Refresh();
            GameManager.Instance.SaveUserData();
        }
    }
    public void WithdrawalCustom()
    {
        int money;
        if (int.TryParse(withdrawalInputField.text, out money))
        {
            Withdrawal(money);
        }
        else
        {
            Debug.Log("잘못된 입력입니다.");
        }
    }

    public void Transfer()
    {
        string filePath = Path.Combine(saveDirectory, transferId.text + ".json");

        //자신한테 보내는 예외처리 까지 만들면 완벽

        if (!File.Exists(filePath))
        {
            Debug.Log("대상이 없습니다.");
            return;
        }

        int amount;
        if (!int.TryParse(transferAmount.text, out amount) || amount <= 0)
        {
            Debug.Log("유효한 금액을 입력하세요.");
            return;
        }

        if (GameManager.Instance.userData.balance < amount)
        {
            Debug.Log("잔액이 부족합니다.");
            return;
        }

        //받는 사람 데이터
        string json = File.ReadAllText(filePath);
        UserData TransferRecipient = JsonUtility.FromJson<UserData>(json);

        TransferRecipient.balance += amount;
        GameManager.Instance.userData.balance -= amount;

        // 보낸 사람 데이터 저장
        GameManager.Instance.SaveUserData();

        // 받는 사람 데이터 저장 
        File.WriteAllText(filePath, JsonUtility.ToJson(TransferRecipient, true));

        Refresh();
        Debug.Log($"송금 성공! {transferId.text}님에게 {amount}원이 송금되었습니다.");
    }

    public void Refresh() //UI 업데이트
    {
        TxtName.text = GameManager.Instance.userData.name;
        TxtBalance.text = GameManager.Instance.userData.balance.ToString("N0");
        Txtcash.text = GameManager.Instance.userData.cash.ToString("N0");
    }

    
}
