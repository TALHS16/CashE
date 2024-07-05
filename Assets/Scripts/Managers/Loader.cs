using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Loader : MonoBehaviour
{
    public GameObject user_manager;
    public GameObject firebase_manager;
    public GameObject transaction_manager;
    public GameObject target_manager;
    public GameObject target_history_manager;

    public GameObject category_manager;
    public Image loading;
    public TMP_Text warning_txt;
    public GameObject error_txt;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(this);
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            error_txt.SetActive(true);
            loading.fillAmount = 1f;
        }
        else
        {
            Instantiate(firebase_manager, gameObject.transform);
            Instantiate(user_manager, gameObject.transform);
            Instantiate(category_manager, gameObject.transform);
            UserManager.Instance.LogIn();
            if (UserManager.Instance.GetCurrentUser() == null)
            {
                SceneManager.LoadScene(1);
            }
            else
            {

                Instantiate(transaction_manager, gameObject.transform);
                Instantiate(target_manager, gameObject.transform);
                Instantiate(target_history_manager, gameObject.transform);
            }
        }

    }

    public void Update()
    {
        if (loading != null && loading.fillAmount < 1f)
        {
            loading.fillAmount += 0.025f;
            if (loading.fillAmount == 1f && !error_txt.gameObject.activeSelf)
            {
                iTween.FadeTo(warning_txt.gameObject, iTween.Hash("looptype", "pingpong", "time", 2.0f, "alpha", 1f, "delay", 1f));
            }
        }
    }
}
