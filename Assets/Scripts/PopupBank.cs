using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PopupBank : MonoBehaviour
{

    public GameObject atm;         // �� ��� ����
    public GameObject depositUI;   // �Ա� UI 
    public GameObject withdrawalUI; // ��� UI
    public GameObject transferUI; //�۱� UI

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
            Debug.Log("�߸��� �Է��Դϴ�.");
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
            Debug.Log("�߸��� �Է��Դϴ�.");
        }
    }

    public void Transfer()
    {
        string filePath = Path.Combine(saveDirectory, transferId.text + ".json");

        //�ڽ����� ������ ����ó�� ���� ����� �Ϻ�

        if (!File.Exists(filePath))
        {
            Debug.Log("����� �����ϴ�.");
            return;
        }

        int amount;
        if (!int.TryParse(transferAmount.text, out amount) || amount <= 0)
        {
            Debug.Log("��ȿ�� �ݾ��� �Է��ϼ���.");
            return;
        }

        if (GameManager.Instance.userData.balance < amount)
        {
            Debug.Log("�ܾ��� �����մϴ�.");
            return;
        }

        //�޴� ��� ������
        string json = File.ReadAllText(filePath);
        UserData TransferRecipient = JsonUtility.FromJson<UserData>(json);

        TransferRecipient.balance += amount;
        GameManager.Instance.userData.balance -= amount;

        // ���� ��� ������ ����
        GameManager.Instance.SaveUserData();

        // �޴� ��� ������ ���� 
        File.WriteAllText(filePath, JsonUtility.ToJson(TransferRecipient, true));

        Refresh();
        Debug.Log($"�۱� ����! {transferId.text}�Կ��� {amount}���� �۱ݵǾ����ϴ�.");
    }

    public void Refresh() //UI ������Ʈ
    {
        TxtName.text = GameManager.Instance.userData.name;
        TxtBalance.text = GameManager.Instance.userData.balance.ToString("N0");
        Txtcash.text = GameManager.Instance.userData.cash.ToString("N0");
    }

    
}
