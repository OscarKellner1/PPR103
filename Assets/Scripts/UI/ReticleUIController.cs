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

    private void Update()
    {
        if (interactionSystem.LookAtObject == null) targetReticuleAlpha = 0f;
        else targetReticuleAlpha = maxReticuleAlpha;

        if (DialogueManager.Instance.HasActiveDialogue)
        {
            targetReticuleAlpha = 0f;
        }

        float delta = Time.deltaTime * animationSpeed;
        reticuleAlphaAnimationValue = Mathf.MoveTowards(reticuleAlphaAnimationValue, targetReticuleAlpha, delta);
        ReticuleAlpha = alphaAnimation.Evaluate(reticuleAlphaAnimationValue);
    }
}
