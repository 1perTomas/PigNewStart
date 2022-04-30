using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField]
    private float writingSpeed = 15f;

    internal bool canAdvance;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !canAdvance)
        {
            writingSpeed = 50;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            writingSpeed = 15f;
        }
    }

    public Coroutine Run(string textToType, TMP_Text textLabel, TMP_Text continueText)
    {
       return StartCoroutine(routine:TypeText(textToType,textLabel,continueText));
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel, TMP_Text continueText) // added continue text so it doesnt glitch out from canAdvance bool
    {
        textLabel.text = string.Empty;
        continueText.text = string.Empty;

        //yield return new WaitForSeconds(2);

        float t = 0;
        int charIndex = 0;

        while(charIndex < textToType.Length)
        {
            canAdvance = false;
            t += Time.deltaTime * writingSpeed;
            charIndex = Mathf.FloorToInt(t); // rounds down to number
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textLabel.text = textToType.Substring(startIndex: 0, length: charIndex);

                        yield return null;
        }
        

        canAdvance = true;
        continueText.text = "E to continue";
        textLabel.text = textToType;

    }

}
