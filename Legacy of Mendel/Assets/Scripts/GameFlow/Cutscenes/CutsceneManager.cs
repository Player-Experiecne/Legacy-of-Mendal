using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro; // Ensure you're using the TextMeshPro namespace

public class CutsceneManager : MonoBehaviour
{
    

    public GameObject cutsceneObject;
    public TextMeshProUGUI dialogueText;
    public List<Cutscene> cutscenes;
    public LoadingScreen loadingScreen;
    public GameObject credits;

    private bool isTextFullyDisplayed = false;
    private bool isCutscenePlaying = false;
    private Coroutine currentDialogueCoroutine = null;


   

    public void TriggerCutsceneByIndex(int index)
    {
        if (index > 0 && index <= cutscenes.Count && !isCutscenePlaying)
        {
            cutsceneObject.SetActive(true);
            if(index == 4)
            {
                SoundManager.Instance.PlayMusic(MusicTrack.FinalVictory);
            }
            isCutscenePlaying = true;
            StartCoroutine(PlayCutscene(index));
        }
        else
        {
            Debug.LogWarning("Cutscene index out of range or cutscene already playing: " + index);
        }
    }

    private IEnumerator PlayCutscene(int index)
    {
        foreach (Dialogue dialogue in cutscenes[index - 1].dialogues)
        {
            isTextFullyDisplayed = false;

            currentDialogueCoroutine = StartCoroutine(ShowDialogue(dialogue));

            while (!isTextFullyDisplayed)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    CompleteCurrentDialogue(dialogue);
                }
                yield return null;
            } 
            
            while (isTextFullyDisplayed)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    break;
                }
                yield return null;
            }

            yield return new WaitForSeconds(0.25f); // Prevents skipping immediately to the next dialogue
        }
        // Cutscene Finished
        isCutscenePlaying = false;
        if(index != 4)
        {
            loadingScreen.OnCutsceneComplete();
        }
        else
        {
            cutsceneObject.SetActive(false);
            credits.SetActive(true);
        }
    }

    private IEnumerator ShowDialogue(Dialogue dialogue)
    {
        SoundManager.Instance.PlaySFX(SoundEffect.Typing);
        dialogueText.text = "";
        isTextFullyDisplayed = false;

        foreach (char letter in dialogue.content.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f); // Typing speed
        }

        isTextFullyDisplayed = true;
        SoundManager.Instance.StopSFX();
    }

    private void CompleteCurrentDialogue(Dialogue dialogue)
    {
        if (currentDialogueCoroutine != null)
        {
            StopCoroutine(currentDialogueCoroutine); // Stop only the current typing coroutine
            currentDialogueCoroutine = null; // Clear the reference
        }
        dialogueText.text = dialogue.content; // Display all text
        isTextFullyDisplayed = true;
        SoundManager.Instance.StopSFX();
    }

    public void TriggerCutsceneDirectly(Cutscene cutscene)
    {
        if (cutscene != null && !isCutscenePlaying)
        {
            cutsceneObject.SetActive(true);
            isCutscenePlaying = true;
            StartCoroutine(PlayCutsceneDirectly(cutscene));
        }
        else
        {
            Debug.LogWarning("Cutscene is null or another cutscene is already playing.");
        }
    }

    private IEnumerator PlayCutsceneDirectly(Cutscene cutscene)
    {
        foreach (Dialogue dialogue in cutscene.dialogues)
        {
            isTextFullyDisplayed = false;

            currentDialogueCoroutine = StartCoroutine(ShowDialogue(dialogue));

            while (!isTextFullyDisplayed)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    CompleteCurrentDialogue(dialogue);
                }
                yield return null;
            }

            while (isTextFullyDisplayed)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    break;
                }
                yield return null;
            }

            yield return new WaitForSeconds(0.25f); // Prevents skipping immediately to the next dialogue
        }

        // Handle completion
        CutsceneCompleted(cutscene);
    }

    private void CutsceneCompleted(Cutscene cutscene)
    {
        isCutscenePlaying = false;
        cutsceneObject.SetActive(false);
        Debug.Log("Cutscene completed: " + cutscene.name);

        // Optionally, trigger other events or transitions
    }

}
