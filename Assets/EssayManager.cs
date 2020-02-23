using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EssayManager : MonoBehaviour
{

    public class IM
    {
        public string sender;
        public string content;
        public float typingTime;
        public float waitTime;
        public bool required;

        public IM(string originator, string message, int WPM, float waitingTime, bool inputRequired)
        {
            sender = originator;
            content = message;
            float characterSpeed = 1f / (((float)WPM * 5f) / 60f);
            typingTime = message.ToCharArray().Length * characterSpeed;
            waitTime = waitingTime;
            required = inputRequired;
        }

    }

    Queue<IM> chatLog;
    public Text chatText;
    public string userName;
    public string readerName;
    public InputChecker typingGame;
    IM curMessage;
    public Text inputtedText;
    public Text leadText;
    public float chatDelay;
    public Text typingStatus;
    public AudioSource messageReceived;
    public AudioSource messageSent;
    // Start is called before the first frame update
    void Start()
    {
        LoadEssay();
        StartCoroutine(PlayEssay());
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator PlayEssay()
    {
        typingStatus.gameObject.SetActive(false);
        yield return new WaitForSeconds(2.5f);
        while(chatLog.Count > 0)
        {
            curMessage = chatLog.Dequeue();
            if(curMessage.sender == readerName)
            {
                leadText.text = curMessage.content;
                while (!Input.GetKeyDown(KeyCode.Return)) {
                    if(Input.anyKeyDown) { leadText.text = ""; }
                    yield return null;
                }
                chatText.text += "\n<color=red>" + curMessage.sender + "</color>: " + inputtedText.text;
                inputtedText.text = "";
                messageSent.Play();
            } else {
                yield return TypingStatus(curMessage.typingTime);
                chatText.text += "\n<color=blue>" + curMessage.sender + "</color>: " + curMessage.content;
                messageReceived.Play();
            }
            yield return new WaitForSeconds(curMessage.waitTime);
        }
        typingGame.StartGame();
    }

    IEnumerator TypingStatus(float typingTime)
    {
        typingStatus.gameObject.SetActive(true);
        yield return new WaitForSeconds(typingTime);
        typingStatus.gameObject.SetActive(false);
    }

    void LoadEssay()
    {
        chatLog = new Queue<IM>();
        chatLog.Enqueue(new IM(userName, "When I was a sophomore in high school, everyone used AOL Instant Messenger",
                    90, 2f, false));
        chatLog.Enqueue(new IM(userName, "The key to its popularity sat in the middle of its name: ",
                    90, 1.8f, false));
        chatLog.Enqueue(new IM(userName, "<i>Instant</i> messaging", 85, 3f, false));
        chatLog.Enqueue(new IM(userName, "Unlike email, which has a cadence of hours or days AIM allowed users to type a message into a computer",
                    80, 1f, false));
        chatLog.Enqueue(new IM(userName, "send it to a friend via a server", 60, 0.1f, false));
        chatLog.Enqueue(new IM(userName, "and receive one back within seconds.", 60, 3f, false));
        chatLog.Enqueue(new IM(userName, "Communicating in real time gives messages the gravitas of proximity", 60, 0.3f, false));
        chatLog.Enqueue(new IM(userName, "it collapses physical distance", 60, 0.5f, false));
        chatLog.Enqueue(new IM(readerName, "Type Anything", 0, 0, false));
    }
}
