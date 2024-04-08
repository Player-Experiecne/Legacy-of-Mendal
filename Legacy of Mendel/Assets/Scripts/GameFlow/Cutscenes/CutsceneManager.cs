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
            GameEvents.TriggerTitleScreen();
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
}
