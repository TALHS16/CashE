using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// UnityWebRequest.Get example

// Access a website and use UnityWebRequest.Get to download a page.
// Also try to download a non-existing page. Display the error.

public class WebRequestManager : MonoBehaviour
{
    public string apikey = "fca_live_gKJAU9bat53PBgZTscpvKBHBVajQH5wRm9DgF6s2";
    private TransactionManager transactionManager;

    public void SetTransactionManager(TransactionManager manager)
    {
        transactionManager = manager;
    }

    void Start()
    {

    }

    public void ConvertCurrency(string currency, System.Action<string> callbackOnFinish)
    {
        // A correct website page.
        StartCoroutine(GetRequest("https://api.freecurrencyapi.com/v1/latest?apikey="+apikey+"&base_currency="+currency+"&currencies=ILS",callbackOnFinish));
    }

    IEnumerator GetRequest(string uri,System.Action<string> callbackOnFinish)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    callbackOnFinish("{\"data\":{\"ILS\":1}}");
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    callbackOnFinish("{\"data\":{\"ILS\":1}}");
                    break;
                case UnityWebRequest.Result.Success:
                    callbackOnFinish(webRequest.downloadHandler.text);
                    break;
            }
        }
        transactionManager.RebuildPieChart();
    }
}