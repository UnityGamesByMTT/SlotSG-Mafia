using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System.Runtime.InteropServices;

public class UIManager : MonoBehaviour
{

    [Header("Popus UI")]
    [SerializeField]
    private GameObject MainPopup_Object;
    [SerializeField]
    private Button Paytable_button;

    [Header("Paytable Popup")]
    [SerializeField]
    private GameObject PaytablePopup_Object;
    [SerializeField]
    private Button PaytableExit_Button;
    [SerializeField]
    private Button Left_Arrow;
    [SerializeField]
    private Button Right_Arrow;
    [SerializeField]
    private GameObject[] paytableList;
    [SerializeField]
    private int CurrentIndex = 0;
    [SerializeField]
    private Image Info_Image;
    [SerializeField]
    private TMP_Text[] SymbolsText;
    [SerializeField] private TMP_Text Scatter_Text;
    [SerializeField] private TMP_Text Jackpot_Text;
    [SerializeField] private TMP_Text Bonus_Text;
    [SerializeField] private TMP_Text Wild_Text;

    [Header("Win Popup")]
    [SerializeField] private GameObject WinPopup_Object;
    [SerializeField] private GameObject jackpot_Object;
    [SerializeField] private Image Win_Image;
    [SerializeField] private Sprite BigWin_Sprite;
    [SerializeField] private Sprite HugeWin_Sprite;
    [SerializeField] private Sprite MegaWin_Sprite;
    [SerializeField] private TMP_Text Win_Text;
    [SerializeField] private TMP_Text jackpot_Text;
    [SerializeField] private Button WinExitBtn;
    [SerializeField] private Button JackpotExitBtn;
    private Tween PopupTween;
    private Tween TextTween;

    [Header("Light Animation")]
    [SerializeField] private GameObject lightOn;
    [SerializeField] private GameObject lightOff;


    [Header("Menu popup")]
    [SerializeField] private Transform Menu_button_grp;
    [SerializeField] private Button Menu_button;
    [SerializeField] private bool isOpen = false;
    [SerializeField] private Sprite MenuOpenSprite;
    [SerializeField] private Sprite MenuCloseSprite;


    [Header("Settings popup")]
    [SerializeField] private Button Setting_button;
    [SerializeField] private GameObject settingObject;
    [SerializeField] private Button Sound_Button;
    [SerializeField] private Button Music_Button;
    [SerializeField] private Sprite OnSprite;
    [SerializeField] private Sprite OffSprite;
    [SerializeField] private Button Setting_exit_button;


    [Header("Splash Screen")]
    [SerializeField] private GameObject spalsh_screen;
    [SerializeField] private Image progressbar;
    [SerializeField] private TMP_Text progressbar_text;
    [SerializeField] private TMP_Text loadingText;


    [Header("Scripts")]
    [SerializeField] private AudioController audioController;
    [SerializeField] private SlotBehaviour slotManager;
    [SerializeField] private SocketIOManager socketManager;

    [Header("Free Spins")]
    [SerializeField] private Image freeSpinBar;
    [SerializeField] private TMP_Text freeSpinCount;

    [Header("Quit Popup")]
    [SerializeField] private GameObject QuitPopupObject;
    [SerializeField] private Button GameExit_Button;
    [SerializeField] private Button no_Button;
    [SerializeField] private Button cancel_Button;
    [SerializeField] private Button Quit_Button;

    [Header("disconnection popup")]
    [SerializeField] private GameObject DisconnectPopupObject;
    [SerializeField] private Button CloseDisconnect_Button;

    [Header("low balance popup")]
    [SerializeField] private Button Close_Button;
    [SerializeField] private GameObject LowBalancePopup_Object;

    [Header("AnotherDevice Popup")]
    [SerializeField] private Button CloseAD_Button;
    [SerializeField] private GameObject ADPopup_Object;

    [SerializeField] private Button m_AwakeGameButton;
    private bool isExit = false;


    private bool isMusic = true;
    private bool isSound = true;

    private void Awake()
    {
        // if (spalsh_screen) spalsh_screen.SetActive(true);
        // StartCoroutine(LoadingRoutine());
         SimulateClickByDefault();

    }

    private void SimulateClickByDefault()
    {
        Debug.Log("Awaken The Game...");
        m_AwakeGameButton.onClick.Invoke();
    }
    private void Start()
    {
        //StartCoroutine(lightanimation());

        if (PaytableExit_Button) PaytableExit_Button.onClick.RemoveAllListeners();
        if (PaytableExit_Button) PaytableExit_Button.onClick.AddListener(delegate { ClosePopup(PaytablePopup_Object); ResetInfoScreens(); });


        if (Paytable_button) Paytable_button.onClick.RemoveAllListeners();
        if (Paytable_button) Paytable_button.onClick.AddListener(delegate { OpenPopup(PaytablePopup_Object); });

        if (Left_Arrow) Left_Arrow.onClick.RemoveAllListeners();
        if (Left_Arrow) Left_Arrow.onClick.AddListener(delegate { slide(-1); });

        if (Right_Arrow) Right_Arrow.onClick.RemoveAllListeners();
        if (Right_Arrow) Right_Arrow.onClick.AddListener(delegate { slide(1); });

        if (GameExit_Button) GameExit_Button.onClick.RemoveAllListeners();
        if (GameExit_Button) GameExit_Button.onClick.AddListener(delegate { OpenPopup(QuitPopupObject); });

        if (no_Button) no_Button.onClick.RemoveAllListeners();
        if (no_Button) no_Button.onClick.AddListener(delegate { if(!isExit){ClosePopup(QuitPopupObject);} });

        if (cancel_Button) cancel_Button.onClick.RemoveAllListeners();
        if (cancel_Button) cancel_Button.onClick.AddListener(delegate { if(!isExit){ClosePopup(QuitPopupObject);} });

        if (Quit_Button) Quit_Button.onClick.RemoveAllListeners();
        if (Quit_Button) Quit_Button.onClick.AddListener(CallOnExitFunction);

        if (CloseDisconnect_Button) CloseDisconnect_Button.onClick.RemoveAllListeners();
        if (CloseDisconnect_Button) CloseDisconnect_Button.onClick.AddListener(CallOnExitFunction);

        if (Close_Button) Close_Button.onClick.RemoveAllListeners();
        if (Close_Button) Close_Button.onClick.AddListener(delegate { ClosePopup(LowBalancePopup_Object); });

        //Menu
        if (Menu_button) Menu_button.onClick.RemoveAllListeners();
        if (Menu_button) Menu_button.onClick.AddListener(OnMenuClick);

        //settings
        if (Setting_button) Setting_button.onClick.RemoveAllListeners();
        if (Setting_button) Setting_button.onClick.AddListener(delegate { OpenPopup(settingObject); });

        if (Setting_exit_button) Setting_exit_button.onClick.RemoveAllListeners();
        if (Setting_exit_button) Setting_exit_button.onClick.AddListener(delegate { ClosePopup(settingObject); });


        isMusic = true;
        isSound = true;

        if (Sound_Button) Sound_Button.onClick.RemoveAllListeners();
        if (Sound_Button) Sound_Button.onClick.AddListener(ToggleSound);

        if (Music_Button) Music_Button.onClick.RemoveAllListeners();
        if (Music_Button) Music_Button.onClick.AddListener(ToggleMusic);

        if (CloseAD_Button) CloseAD_Button.onClick.RemoveAllListeners();
        if (CloseAD_Button) CloseAD_Button.onClick.AddListener(CallOnExitFunction);

        if (WinExitBtn) WinExitBtn.onClick.RemoveAllListeners();
        if (WinExitBtn) WinExitBtn.onClick.AddListener(delegate { OnClickCloseWinPopup(false); } );

        if (JackpotExitBtn) JackpotExitBtn.onClick.RemoveAllListeners();
        if (JackpotExitBtn) JackpotExitBtn.onClick.AddListener(delegate { OnClickCloseWinPopup(true); });

    }



    private IEnumerator LoadingRoutine()
    {
        StartCoroutine(LoadingTextAnimate());
        float fillAmount = 0.7f;
        progressbar.DOFillAmount(fillAmount, 3f).SetEase(Ease.Linear).onUpdate = () =>
        {

            progressbar_text.text = (progressbar.fillAmount * 100).ToString("f0") + "%";
        };
        yield return new WaitForSecondsRealtime(3f);
        yield return new WaitUntil(() => !socketManager.isLoading);
        progressbar.DOFillAmount(1, 1f).SetEase(Ease.Linear).onUpdate = () =>
        {
            progressbar_text.text = (progressbar.fillAmount * 100).ToString("f0") + "%";

        };
        yield return new WaitForSecondsRealtime(1f);
        if (spalsh_screen) spalsh_screen.SetActive(false);
        StopCoroutine(LoadingTextAnimate());
    }

    private IEnumerator LoadingTextAnimate()
    {
        while (true)
        {
            if (loadingText) loadingText.text = "Loading.";
            yield return new WaitForSeconds(0.5f);
            if (loadingText) loadingText.text = "Loading..";
            yield return new WaitForSeconds(0.5f);
            if (loadingText) loadingText.text = "Loading...";
            yield return new WaitForSeconds(0.5f);
        }
    }


    internal void PopulateWin(int value, double amount)
    {
        switch (value)
        {
            case 1:
                if (Win_Image) Win_Image.sprite = BigWin_Sprite;
                break;
            case 2:
                if (Win_Image) Win_Image.sprite = HugeWin_Sprite;
                break;
            case 3:
                if (Win_Image) Win_Image.sprite = MegaWin_Sprite;
                break;

        }
        if (value == 4)
            StartPopupAnim(amount, true);
        else
            StartPopupAnim(amount, false);

    }


    private void StartPopupAnim(double amount, bool jackpot = false)
    {
        int initAmount = 0;
        if (jackpot)
        {
            if (jackpot_Object) jackpot_Object.SetActive(true);
        }
        else
        {
            if (WinPopup_Object) WinPopup_Object.SetActive(true);

        }

        if (MainPopup_Object) MainPopup_Object.SetActive(true);

       TextTween = DOTween.To(() => initAmount, (val) => initAmount = val, (int)amount, 5f).OnUpdate(() =>
        {
            if (jackpot)
            {
                if (jackpot_Text) jackpot_Text.text = initAmount.ToString();

            }
            else
            {

                if (Win_Text) Win_Text.text = initAmount.ToString();

            }
        });
        Debug.Log("Dev_test:" + "firest   " + Time.deltaTime);
        PopupTween = DOVirtual.DelayedCall(6f, () =>
        {
            Debug.Log("Dev_test:" + "delayed Called " + Time.deltaTime);
            if (jackpot)
            {

                ClosePopup(jackpot_Object);
                jackpot_Text.text="";

            }
            else
            {
                ClosePopup(WinPopup_Object);
                Win_Text.text="";

            }

            slotManager.CheckPopups = false;
        });
    }
    private void OnClickCloseWinPopup(bool jackpot)
    {
        if(PopupTween != null)
        {
            PopupTween.Kill();
            PopupTween = null;
        }
        if(TextTween != null)
        {
            TextTween.Kill();
            TextTween = null;
        }
        if (jackpot)
        {


            ClosePopup(jackpot_Object);
            jackpot_Text.text = "";

        }
        else
        {
            ClosePopup(WinPopup_Object);
            Win_Text.text = "";

        }

        slotManager.CheckPopups = false;
    }
    private void OpenPopup(GameObject Popup)
    {
        if (audioController) audioController.PlayButtonAudio();
        if (Popup) Popup.SetActive(true);


        if (MainPopup_Object) MainPopup_Object.SetActive(true);
    }

    private void ClosePopup(GameObject Popup)
    {
        if (audioController) audioController.PlayButtonAudio();
        if (Popup) Popup.SetActive(false);
        if (!DisconnectPopupObject.activeSelf)
        {
            if (MainPopup_Object) MainPopup_Object.SetActive(false);
        }
    }

    internal void LowBalPopup()
    {

        OpenPopup(LowBalancePopup_Object);
    }

    internal void ADfunction()
    {
        OpenPopup(ADPopup_Object);
    }

    private void slide(int i)
    {
        if (audioController) audioController.PlayButtonAudio();

        if (CurrentIndex < paytableList.Length - 1 && i > 0)
        {
            paytableList[CurrentIndex].SetActive(false);
            paytableList[CurrentIndex + 1].SetActive(true);
            CurrentIndex++;
        }

        if (CurrentIndex >= 1 && i < 0)
        {
            paytableList[CurrentIndex].SetActive(false);
            paytableList[CurrentIndex - 1].SetActive(true);
            CurrentIndex--;
        }
    }

    private void ResetInfoScreens()
    {
        CurrentIndex = 0;
        for(int i = 0; i < paytableList.Length; i++)
        {
            paytableList[i].SetActive(false);
        }
        paytableList[CurrentIndex].SetActive(true);
    }

    void OnMenuClick()
    {
        isOpen = !isOpen;
        if (audioController) audioController.PlayButtonAudio();
        if (isOpen)
        {
            if (Menu_button) Menu_button.image.sprite = MenuCloseSprite;
            for (int i = 0; i < Menu_button_grp.childCount - 1; i++)
            {
                Menu_button_grp.GetChild(i).DOLocalMoveY(-200 * (i + 1), 0.1f * (i + 1));
            }
        }
        else
        {

            if (Menu_button) Menu_button.image.sprite = MenuOpenSprite;

            for (int i = 0; i < Menu_button_grp.childCount - 1; i++)
            {
                Menu_button_grp.GetChild(i).DOLocalMoveY(0, 0.1f * (i + 1));
            }
        }


    }


    internal void updateFreeSPinData(float fillAmount, int count)
    {
        print("triggered freespin");
        if (fillAmount < 0)
            fillAmount = 0;
        if (fillAmount > 1)
            fillAmount = 1;

        freeSpinBar.DOFillAmount(fillAmount, 0.5f).SetEase(Ease.Linear);
        freeSpinCount.text = count.ToString();
    }


    internal void setFreeSpinData(int count)
    {
        freeSpinBar.DOFillAmount(1, 0.2f).SetEase(Ease.Linear);
        freeSpinCount.text = count.ToString();
    }

    internal void InitialiseUIData(string SupportUrl, string AbtImgUrl, string TermsUrl, string PrivacyUrl, Paylines symbolsText)
    {
        PopulateSymbolsPayout(symbolsText);
    }

    private void PopulateSymbolsPayout(Paylines paylines)
    {
        for (int i = 0; i < SymbolsText.Length; i++)
        {
            string text = null;
            if (paylines.symbols[i].Multiplier[0][0] != 0)
            {
                text += paylines.symbols[i].Multiplier[0][0]+"x";
            }
            if (paylines.symbols[i].Multiplier[1][0] != 0)
            {
                text += "\n" + paylines.symbols[i].Multiplier[1][0] + "x";
            }
            if (paylines.symbols[i].Multiplier[2][0] != 0)
            {
                text += "\n" + paylines.symbols[i].Multiplier[2][0] + "x";
            }
            if (SymbolsText[i]) SymbolsText[i].text = text;
        }



        for (int i = 0; i < paylines.symbols.Count; i++)
        {

            if (paylines.symbols[i].Name.ToUpper() == "SCATTER")
            {
                if (Scatter_Text) Scatter_Text.text = paylines.symbols[i].description.ToString();
            }
            if (paylines.symbols[i].Name.ToUpper() == "JACKPOT")
            {
                if (Jackpot_Text) Jackpot_Text.text = paylines.symbols[i].description.ToString();
            }
            if (paylines.symbols[i].Name.ToUpper() == "BONUS")
            {
                if (Bonus_Text) Bonus_Text.text = paylines.symbols[i].description.ToString();
            }
            if (paylines.symbols[i].Name.ToUpper() == "WILD")
            {
                if (Wild_Text) Wild_Text.text = paylines.symbols[i].description.ToString();
            }
        }
    }

    internal void DisconnectionPopup()
    {

        //ClosePopup(ReconnectPopup_Object);
        if (!isExit)
        {
            OpenPopup(DisconnectPopupObject);
        }

    }

    private void CallOnExitFunction()
    {
        isExit = true;
        slotManager.CallCloseSocket();
        // Application.ExternalCall("window.parent.postMessage", "onExit", "*");
    }

    private void lightanimation()
    {
        if (lightOn.activeSelf)
        {
            lightOn.SetActive(false);
        }
        else
        {
            lightOn.SetActive(true);
        }
    }

    internal void StartLightAnim()
    {
        InvokeRepeating("lightanimation", 0f, 0.2f);
    }

    internal void StopLightAnim()
    {
        CancelInvoke("lightanimation");
        lightOn.SetActive(false);
    }

    private void ToggleMusic()
    {
        //[SerializeField] private Sprite SoundOnSprite;
        //[SerializeField] private Sprite SoundOffSprite;
        //[SerializeField] private Sprite MusicOnSprite;
        //[SerializeField] private Sprite MusicOffSprite;
        isMusic = !isMusic;
        if (isMusic)
        {
            if (Music_Button) Music_Button.image.sprite = OnSprite;
            audioController.ToggleMute(false, "bg");
        }
        else
        {
            if (Music_Button) Music_Button.image.sprite = OffSprite;

            audioController.ToggleMute(true, "bg");
        }
    }



    private void ToggleSound()
    {
        isSound = !isSound;
        if (isSound)
        {
            //if (SoundOn_Object) SoundOn_Object.SetActive(true);
            //if (SoundOff_Object) SoundOff_Object.SetActive(false);

            if (Sound_Button) Sound_Button.image.sprite = OnSprite;
            if (audioController) audioController.ToggleMute(false, "button");
            if (audioController) audioController.ToggleMute(false, "wl");
        }
        else
        {
            //if (SoundOn_Object) SoundOn_Object.SetActive(false);
            //if (SoundOff_Object) SoundOff_Object.SetActive(true);
            if (Sound_Button) Sound_Button.image.sprite = OffSprite;

            if (audioController) audioController.ToggleMute(true, "button");
            if (audioController) audioController.ToggleMute(true, "wl");
        }
    }
}
