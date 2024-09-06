using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonInteractUnit : MonoBehaviour
{
    [field: SerializeField] public Transform buttonHolder { get; private set; }
    [field: SerializeField] public TextMeshProUGUI inputText { get; private set; }

    [field: SerializeField] public Transform inputButtonBorder { get; private set; } //i want this to keep moving.

    [field: SerializeField] public TextMeshProUGUI titleText { get; private set; }

    [field: SerializeField] public TextMeshProUGUI levelText { get; private set; }



    private void Update()
    {
        RotateBorder();
    }

    public void RotateBorder()
    {
        inputButtonBorder.Rotate(new Vector3(0, 0, 50));
    }

    public void SetUp(string inputString, string titleString, string levelString)
    {

        inputText.text = inputString;
        inputText.gameObject.SetActive(inputString != "");


        titleText.text = titleString;
        titleText.gameObject.SetActive(titleString != "");


        levelText.text = levelString;
        levelText.gameObject.SetActive(levelString != "");

    }



}
