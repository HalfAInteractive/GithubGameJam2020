using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Some script that I made a while ago to highlight a button when a menu goes active.
/// Useful for games where you don't use a mouse or jsut want the button be highlighted initially.
/// </summary>
public class InitialButtonSelectorUI : MonoBehaviour
{
    [SerializeField] private GameObject highlightThisButton;
    [SerializeField] EventSystem eventSystem;
    private GameObject previousButton = null;

    private void OnEnable()
    {
        StartCoroutine(WaitEndFrameRoutine());
    }


    private void OnDisable()
    {
        if (eventSystem?.currentSelectedGameObject != null)
        {
            previousButton = eventSystem.currentSelectedGameObject;
        }
    }

    // fixes the bug where the highlighted button doesn't get selected/highlighted. wait end of frame gets rid of it
    private IEnumerator WaitEndFrameRoutine()
    {
        yield return new WaitForEndOfFrame();
        eventSystem.SetSelectedGameObject(null);

        //print($"prev {previousButton}");
        if (previousButton == null)
        {
            eventSystem.SetSelectedGameObject(highlightThisButton);
        }
        else
        {
            eventSystem.SetSelectedGameObject(previousButton);
        }
    }
}
