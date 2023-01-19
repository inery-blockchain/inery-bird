using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetName : MonoBehaviour
{

    // Potrebne reference:
    public InputField inputName;
    public InputField inputEmail;

    string wordName = null;
    string wordEmail = null;
    int wordNameIndex = 0;
    int wordEmailIndex = 0;
    string alpha;
    int focusElement;

    private void Update()
    {
        if (inputName.isFocused)
        {
            focusElement = 1;
        }
        else if (inputEmail.isFocused)
        {
            focusElement = 2;
        }
    }

    public void AlphabetFunction(string alphabet)
    {

        if (alphabet == "bs")
        {
            if (focusElement == 1)
            {
                wordNameIndex--;
                if (wordName.Length > 0)
                    wordName = wordName.Remove(wordName.Length - 1, 1);
                inputName.text = wordName.ToString();
            }
            else if (focusElement == 2)
            {
                if (alphabet == "bs")
                {
                    wordEmailIndex--;
                    if (wordEmail.Length > 0)
                        wordEmail = wordEmail.Remove(wordEmail.Length - 1, 1);
                    inputEmail.text = wordEmail;
                }
            }
        }
        else
        {
            if (focusElement == 1)
            {
                wordNameIndex++;
                wordName = wordName + alphabet;
                inputName.text = wordName;
            }
            else if (focusElement == 2)
            {
                wordEmailIndex++;
                wordEmail = wordEmail + alphabet;
                inputEmail.text = wordEmail;
            }
        }
    }
}
