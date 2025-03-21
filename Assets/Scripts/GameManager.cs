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



    public void SaveUserData() //제이슨 저장
    {

        string filePath = Path.Combine(saveDirectory, userData.userId + ".json");//경로 저장하는 곳
        string json = JsonUtility.ToJson(userData, true); //제이슨 변환 
        File.WriteAllText(filePath, json); // 생성

        //Debug.Log(json);

    }

    public void OnLogin() //로그인
    {
        string id = idInput.text;
        string pw = pwInput.text;
        LoadUserData(id, pw);
    }

    public void LoadUserData(string id, string pw) //제이슨 로드 및 로그인
    {
        string filePath = Path.Combine(saveDirectory, id + ".json");


        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath); //파일 읽고
            UserData loadedUser = JsonUtility.FromJson<UserData>(json); // 다시 원래 데이터로 변환

            if (loadedUser.userPw == pw) //
            {
                userData = loadedUser;
                UIManager.Instance.OnBankPopUp();
                UIManager.Instance.popupBank.Refresh();
                Debug.Log("로그인 성공! 환영합니다, " + userData.name);
            }
            else
            {
                Debug.LogError("비밀번호가 틀렸습니다.");
            }
        }
        else
        {
            Debug.LogError("아이디가 존재하지 않습니다.");
        }

        UIManager.Instance.popupBank.Refresh();
    }

    public void Register() // 회원가입
    {
        string id = idInput.text;
        string pw = pwInput.text;
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
        {
            Debug.Log("아이디와 패스워드 둘 다 입력하세요.");
            return;
        }
        string filePath = Path.Combine(saveDirectory, id + ".json");

        if (File.Exists(filePath))
        {
            Debug.LogError("이미 존재하는 아이디입니다. 다른 아이디를 사용하세요.");
        }
        else
        {
            userData = new UserData(id, pw, "김영중", 100000, 200000);
            SaveUserData();
            Debug.Log("회원가입 성공! 로그인 후 이용하세요.");
        }
    }
}
