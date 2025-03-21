using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UserData 
{
    public string userId;
    public string userPw;
    public string name;
    public int balance;
    public int cash;
    public UserData(string userid,string userpw,string name, int balance, int cash)
    {
        this.userId = userid;
        this.userPw = userpw;
        this.name = name;
        this.balance = balance;
        this.cash = cash;
    }
}
