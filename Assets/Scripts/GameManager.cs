using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public UserData userData;


    public TMP_InputField idInput;
    public TMP_InputField pwInput;





    //private string savePath;
    private string saveDirectory;
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
        //savePath = Path.Combine(Application.dataPath, "userdata.json");
        saveDirectory = Path.Combine(Application.dataPath, "UserData");
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
    }
    void Start()
    {

        //LoadUserData();
        //Refresh();

    }



    public void SaveUserData() //���̽� ����
    {

        string filePath = Path.Combine(saveDirectory, userData.userId + ".json");//��� �����ϴ� ��
        string json = JsonUtility.ToJson(userData, true); //���̽� ��ȯ 
        File.WriteAllText(filePath, json); // ����

        //Debug.Log(json);

    }

    public void OnLogin() //�α���
    {
        string id = idInput.text;
        string pw = pwInput.text;
        LoadUserData(id, pw);
    }

    public void LoadUserData(string id, string pw) //���̽� �ε� �� �α���
    {
        string filePath = Path.Combine(saveDirectory, id + ".json");


        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath); //���� �а�
            UserData loadedUser = JsonUtility.FromJson<UserData>(json); // �ٽ� ���� �����ͷ� ��ȯ

            if (loadedUser.userPw == pw) //
            {
                userData = loadedUser;
                UIManager.Instance.OnBankPopUp();
                UIManager.Instance.popupBank.Refresh();
                Debug.Log("�α��� ����! ȯ���մϴ�, " + userData.name);
            }
            else
            {
                Debug.LogError("��й�ȣ�� Ʋ�Ƚ��ϴ�.");
            }
        }
        else
        {
            Debug.LogError("���̵� �������� �ʽ��ϴ�.");
        }

        UIManager.Instance.popupBank.Refresh();
    }

    public void Register() // ȸ������
    {
        string id = idInput.text;
        string pw = pwInput.text;
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
        {
            Debug.Log("���̵�� �н����� �� �� �Է��ϼ���.");
            return;
        }
        string filePath = Path.Combine(saveDirectory, id + ".json");

        if (File.Exists(filePath))
        {
            Debug.LogError("�̹� �����ϴ� ���̵��Դϴ�. �ٸ� ���̵� ����ϼ���.");
        }
        else
        {
            userData = new UserData(id, pw, "�迵��", 100000, 200000);
            SaveUserData();
            Debug.Log("ȸ������ ����! �α��� �� �̿��ϼ���.");
        }
    }
}
