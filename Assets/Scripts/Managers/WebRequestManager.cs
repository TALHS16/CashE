using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// UnityWebRequest.Get example

// Access a website and use UnityWebRequest.Get to download a page.
// Also try to download a non-existing page. Display the error.

public class WebRequestManager
{
    public static string apikey = "fca_live_gKJAU9bat53PBgZTscpvKBHBVajQH5wRm9DgF6s2";

    public static void ConvertCurrency(MonoBehaviour mono, string currency, System.Action<string,TransactionType> callbackOnFinish,bool buildPie,TransactionType type)
    {
        // A correct website page.
        mono.StartCoroutine(GetRequest("https://api.freecurrencyapi.com/v1/latest?apikey="+apikey+"&base_currency="+currency+"&currencies=ILS",callbackOnFinish,buildPie,type));
    }

    static IEnumerator GetRequest(string uri,System.Action<string,TransactionType> callbackOnFinish,bool buildPie,TransactionType type)
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
                    callbackOnFinish("{\"data\":{\"ILS\":1}}",type);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    callbackOnFinish("{\"data\":{\"ILS\":1}}",type);
                    break;
                case UnityWebRequest.Result.Success:
                    callbackOnFinish(webRequest.downloadHandler.text,type);
                    break;
            }
        }
        if(buildPie)
        {
            TransactionManager.Instance.RebuildPieChart(type);
        }
    }
}