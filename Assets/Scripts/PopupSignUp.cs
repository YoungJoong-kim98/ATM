using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class PopupSignUp : MonoBehaviour
{
    public TMP_InputField idInput;
    public TMP_InputField pwInput;


    private string savePath;

    private void Start()
    {
        savePath = Path.Combine(Application.dataPath, "Sign.json");
    }

}
