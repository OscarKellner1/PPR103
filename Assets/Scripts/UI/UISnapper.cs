using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISnapper : MonoBehaviour
{
    private int Index;
    private float waitTime;
    private Coroutine swapCoroutine;
    private Transform me;
    public float StartRotation;
    public float EndRotation;
    public float StartSize;
    public float EndSize;
    public bool dosize;
    void Start()
    {
        me = this.gameObject.GetComponent<Transform>();
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
            float waitTime = UnityEngine.Random.Range(0.25f, 0.5f);
            yield return new WaitForSeconds(waitTime);

            // Swap between 0 and 1
            Index = (Index == 1) ? 0 : 1;

            if (Index == 1)
            {
                // Set rotation around Z-axis
                me.rotation = Quaternion.Euler(me.rotation.eulerAngles.x, me.rotation.eulerAngles.y, StartRotation);
                // Set uniform scale
                if (dosize)
                {
                    me.localScale = new Vector3(StartSize, StartSize, StartSize);
                }
                
            }
            else
            {
                // Set rotation around Z-axis
                me.rotation = Quaternion.Euler(me.rotation.eulerAngles.x, me.rotation.eulerAngles.y, EndRotation);
                // Set uniform scale
                if (dosize)
                {
                    me.localScale = new Vector3(EndSize, EndSize, EndSize);
                }
                
            }
        }
    }


}
