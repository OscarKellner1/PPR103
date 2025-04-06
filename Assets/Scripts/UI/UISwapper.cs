using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UISwapper : MonoBehaviour
{
    public Sprite[] SwapSprites;
    private Image img;
    private int sprIndex;
    private float waitTime;
    private Coroutine swapCoroutine;

    void Start()
    {
        img = GetComponent<Image>();
        StartSwapper();
    }

    void OnEnable()
    {
        StartSwapper();
    }

    void OnDisable()
    {
        if (swapCoroutine != null)
        {
            StopCoroutine(swapCoroutine);
            swapCoroutine = null;
        }
    }

    private void StartSwapper()
    {
        if (swapCoroutine == null)
        {
            swapCoroutine = StartCoroutine(Swapper());
        }
    }

    private IEnumerator Swapper()
    {
        while (true)
        {
            waitTime = UnityEngine.Random.Range(0.25f, 0.5f);
            yield return new WaitForSeconds(waitTime);

            // Swap between 0 and 1
            sprIndex = (sprIndex == 1) ? 0 : 1;
            img.sprite = SwapSprites[sprIndex];
        }
    }
}
