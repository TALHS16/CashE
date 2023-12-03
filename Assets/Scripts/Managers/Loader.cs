using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    public GameObject user_manager;
    public GameObject firebase_manager;
    public GameObject transaction_manager;

    public GameObject category_manager;
    public Image loading;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(this);
        Instantiate(firebase_manager,gameObject.transform);
        Instantiate(user_manager,gameObject.transform);
        Instantiate(category_manager,gameObject.transform);
        UserManager.Instance.LogIn();
        if(UserManager.Instance.GetCurrentUser() == null)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            
            Instantiate(transaction_manager,gameObject.transform);
        }

    }

    public void Update(){
        if (loading != null && loading.fillAmount < 1f)
        {
            loading.fillAmount += 0.025f;
        }
    }
}
