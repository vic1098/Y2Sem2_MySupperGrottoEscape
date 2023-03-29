using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class timerScript : MonoBehaviour
{
    // for acessing the high scores
    const string privateCode = "fYR0qF1TlkyxeVN8jk2LhgmxvzTGipjEmhbi_bVLI0cg";
    const string publicCode = "6407066b778d3c8a30d47ae1";
    const string webURL = "http://dreamlo.com/lb/";

    [SerializeField] string myName;

    [SerializeField] private float myScore = 0f;
    public TextMeshProUGUI myGUIScore;
    private IEnumerator myCoroutine;
    

    public GameObject player;

    void Start()
    {
        //player = GameObject.Find("CapnGigi");

        myGUIScore.text = myScore.ToString();

        myName = "player1";

        if (PlayerPrefs.HasKey("PlayerName"))
        {
            myName = PlayerPrefs.GetString("PlayerName");
        }

        
        //myName = "rodger";

        //myCoroutine = myCountdown();

        StartCoroutine(myCountdown());

        //used to clear the leaderboard
        //Application.OpenURL(webURL + privateCode + "/clear");
    }


    private IEnumerator myCountdown()
    {

        //I changed this so the score ticks up faster, activating those dopamine receptors, hope that's okay
        while (true)
        {
            yield return new WaitForSeconds(0.005f); //wait 1 second

            myScore++;

            myGUIScore.text = "" + myScore;

            bool mIA = player.gameObject.GetComponent<Damageable>().IsAlive;

            if (mIA == false)
            {
                StartCoroutine(UploadNewHighScore(myName, myScore));
                break;
                //StopCoroutine(myCoroutine);
            }
        }
    }

    IEnumerator UploadNewHighScore(string myName, float myScore)
    {
        //string url = webURL + privateCode + "/add/" + myName + "/" + myScore;
        Debug.Log("Uploading the name and score");

        string loaded = webURL + privateCode + "/add/" + myName + "/" + myScore;
        UnityWebRequest uwr = UnityWebRequest.Get(loaded);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("error while sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Great sucess");
        }
    }

    //IEnumerator UploadNewHighScore(string myName, float myScore)
    //{
    //    WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(myName) + "/" + myScore);
    //    yield return www;
    //}

    // update version of the above
    //IEnumerator UploadNewHighScore(string myName, float myScore)
    //{
    //    var output = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(myName) + "/" + myScore);
    //    yield return output;
    //}

    //IEnumerator UploadNewHighScore(string myName, float myScore)
    //{
    //    WWWForm form = new WWWForm();

    //    UnityWebRequest www = UnityWebRequest.Post(webURL + privateCode + "/add/" + myName + "/" + myScore, form);
    //    yield return www.SendWebRequest();

    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.Log("Error: " + www.error);
    //    }
    //    else
    //    {
    //        Debug.Log("Form upload complete!");
    //    }



}
