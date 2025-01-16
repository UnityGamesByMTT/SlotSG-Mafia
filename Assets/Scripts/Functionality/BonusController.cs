using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BonusController : MonoBehaviour
{
    [SerializeField]
    private GameObject Bonus_Object;
    [SerializeField]
    private SlotBehaviour slotManager;
    [SerializeField]
    private GameObject raycastPanel;
    [SerializeField]
    private List<BonusGameSuitCase> BonusCases;
    [SerializeField]
    private AudioController _audioManager;
    [SerializeField]
    private GameObject exitBonus;

    [SerializeField]
    private List<int> CaseValues;

    int index = 0;
    internal double bet = 0;
    internal double totalWin = 0;

    [SerializeField] private Transform BonusWinPopup;
    [SerializeField] private TMP_Text BonusWintext;

    internal void GetSuitCaseList(List<int> values, double betperline)
    {
        index = 0;
        CaseValues.Clear();
        CaseValues.TrimExcess();
        CaseValues = values;
        bet = betperline;
        totalWin = 0;
        foreach (BonusGameSuitCase cases in BonusCases)
        {
            cases.ResetCase();
        }

        for (int i = 0; i < CaseValues.Count; i++) 
        {
            if(CaseValues[i] == -1)
            {
                CaseValues.RemoveAt(i);
                CaseValues.Add(-1);
            }
        }

        if (raycastPanel) raycastPanel.SetActive(false);
        StartBonus();
    }

    internal void enableRayCastPanel(bool choice)
    {
        if (raycastPanel) raycastPanel.SetActive(choice);
    }

    internal void GameOver()
    {
        BonusWinPopup.parent.gameObject.SetActive(true);
        BonusWinPopup.localScale = Vector3.zero;
        BonusWintext.text = totalWin.ToString();
        BonusWinPopup.DOScale(1,1f).SetEase(Ease.OutBounce);
        DOVirtual.DelayedCall(3f, ()=> {

            slotManager.CheckPopups = false;
            _audioManager.SwitchBGSound(false);

            if (Bonus_Object) Bonus_Object.SetActive(false);
            BonusWinPopup.parent.gameObject.SetActive(false);
            BonusWintext.text = "";

        });

    }

    internal int GetValue()
    {
        int value = 0;

        value = CaseValues[index];

        index++;

        return value;
    }



    internal void PlayWinLooseSound(bool isWin)
    {
        if(isWin)
        {
            _audioManager.PlayBonusAudio("win");
        }
        else
        {
            _audioManager.PlayBonusAudio("lose");
        }
    }

    private void StartBonus()
    {
        _audioManager.SwitchBGSound(true);
        if (Bonus_Object) Bonus_Object.SetActive(true);
    }
}
