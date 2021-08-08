using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class SoundGun : MonoBehaviour
{
    public GameObject tempCritter;
    private Transform player;
    AudioSource audioData;
    public bool captured;//true when the player was recording for the beginning and end of an audio clip with no break
  
    private bool t1;
    private bool t2;
    private bool holder1;

    private bool canPlay = true;//coolDown so you can't spam sound

    public TextMeshProUGUI text;
    private int textValue = 0;

    public float dist1;




    void Start()
    {
        dist1 = 50;
        audioData = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        TextController();
        t1 = tempCritter.GetComponent<RandomAudio>().started;
        t2 = tempCritter.GetComponent<RandomAudio>().ended;

        MouseControls();


        if (captured)
        {
            Debug.Log("Captured!");
            audioData.clip = tempCritter.GetComponent<AudioSource>().clip;
            StartCoroutine("TextCaptured");
        }
       
            
        if (audioData.isPlaying)
        {
            textValue = 3;
        }else
        {
            if (textValue == 3)
                textValue = 0;
        }
            
    }

    void RangeDetector()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        
        if (other.tag == "RecordableAudio")// && other.GetComponent<AudioSource>().isPlaying)
        {
            
           // t1 = other.GetComponent<RandomAudio>().started;
           // t2 = other.GetComponent<RandomAudio>().ended;
          //  Debug.Log(t1);
          //  Debug.Log(t2);
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }

    public void MouseControls()
    {
        if (Input.GetMouseButton(1) && player.GetComponent<PlayerMovement>().inventoryOpen == false)//Holding Right Click will Record
        {
            StartCheck();
            textValue = 1;
        }
        else
        {
            if (textValue == 1)//if text says "Recording..." set it to ""
                textValue = 0;
            holder1 = false;
            captured = false;
        }

        if (Input.GetMouseButtonDown(0) && canPlay && audioData.clip != null && player.GetComponent<PlayerMovement>().inventoryOpen == false)//Left click + cooldown + there is an audio clip
        {
            StartCoroutine("PlaySound");

        }
    }

    public void StartCheck()
    {

        if (t1 == true)//detects if you captured the begininning of audio
        {
            holder1 = true;//temp holder to latch the true value
        }
        EndCheck();

    }
    public void EndCheck()
    {
        if (holder1 && t2)//detects if you captured the beginning of the clip and then the end
        {
            captured = true;
        }
        
            
    }

    IEnumerator PlaySound()
    {
        canPlay = false;//cooldown
        
        audioData.Play(0);//plays recorded clip
        yield return new WaitForSeconds(audioData.clip.length);
        
        canPlay = true;
    }



    private void TextController()
    {
        if(textValue == 1)
        {
            text.SetText("Recording...");
        }else if(textValue == 2)
        {
            text.SetText("Captured!");
        }
        else if(textValue == 3)
        {
            text.SetText("Playing");
        }
        else
        {
            text.SetText("");
        }
    }
    IEnumerator TextCaptured()
    {
        textValue = 2;
        yield return new WaitForSeconds(2);
        if(textValue == 2)
            textValue = 0;
        
    }
}
