using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class HighlightButtonEffectUI : MonoBehaviour, ISelectHandler, ISubmitHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    float endScale = 1.25f, duration = 0.2f;
    RectTransform rect = null;
    bool isHighlighted = false;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void OnDisable()
    {
        rect.localScale = Vector3.one;
    }

    private void UserHighlightButton()
    {
        isHighlighted = true;
        MenuSoundEffectsSystemUI.Play?.ButtonSelected();
        rect.DOScale(endScale, duration).SetUpdate(true);
    }

    private void UserExitButton()
    {
        isHighlighted = false;
        float resetScale = 1f;
        rect.DOScale(resetScale, duration).SetUpdate(true);
    }

    private void UserPressedButton()
    {
        MenuSoundEffectsSystemUI.Play?.ButtonSubmitted();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        UserExitButton();
    }

    public void OnSelect(BaseEventData eventData)
    {
        // when the mouse clicks on button, for some reason it fires this function and OnPointerClick.
        // this will just gate it out if that's the case.
        if (isHighlighted) return;

        UserHighlightButton();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        UserPressedButton();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UserHighlightButton();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UserExitButton();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UserPressedButton();
    }
}
