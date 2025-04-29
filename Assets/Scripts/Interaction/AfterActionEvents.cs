using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterActionEvents : MonoBehaviour
{
    #region AfterDialogueStuff
    #region AnaisEvents


    #endregion
    #region JosephineEvents


    #endregion
    #region MofuEvents
    public void MofuAfterFirstTalk()
    {
        PlayerPrefs.SetInt("MofuTalkedTo", 1);
    }

    #endregion
    #region AshfrostEvents


    #endregion
    #region TerryEvents


    #endregion
    #endregion
    #region AfterPlacementStuff
    public void TestMethod()
    {
        Debug.Log("Hey Bud");
    }

    #endregion
}
