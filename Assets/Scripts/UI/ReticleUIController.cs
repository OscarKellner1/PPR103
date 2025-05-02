using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReticleUIController : MonoBehaviour
{
    [SerializeField]
    InteractionSystem interactionSystem;
    [SerializeField]
    Image image;
    [SerializeField]
    AnimationCurve alphaAnimation;
    [SerializeField]
    private float animationSpeed = 2;

    private float maxReticuleAlpha;
    private float targetReticuleAlpha;
    private float reticuleAlphaAnimationValue;

    private float ReticuleAlpha
    {
        get
        {
            return image.color.a;
        }
        set
        {
            var newColor = image.color;
            newColor.a = value;
            image.color = newColor;
        }
    }


    private void Start()
    {
        maxReticuleAlpha = image.color.a;

        ReticuleAlpha = 0;
        targetReticuleAlpha = ReticuleAlpha;
    }

    private void OnEnable()
    {
        interactionSystem.OnLookAtChange.AddListener(HandleInteractionLookChange);
    }

    private void HandleInteractionLookChange(InteractionObject interactionObject)
    {
        if (interactionObject == null) targetReticuleAlpha = 0;
        else targetReticuleAlpha = maxReticuleAlpha;
    }

    private void Update()
    {
        float delta = Time.deltaTime * animationSpeed;
        reticuleAlphaAnimationValue = Mathf.MoveTowards(reticuleAlphaAnimationValue, targetReticuleAlpha, delta);
        ReticuleAlpha = alphaAnimation.Evaluate(reticuleAlphaAnimationValue);
    }

    private void OnDisable()
    {
        interactionSystem.OnLookAtChange.RemoveListener(HandleInteractionLookChange);
    }
}
