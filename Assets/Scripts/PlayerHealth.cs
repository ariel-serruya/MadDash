using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

    public float maxHealth = 100f;//The max health of the player.
    public float curHealth = 100f;// The current health of the player this number will be altered by the code to respond with damage and health packs.
    private float healthBarLength;//The length of the health bar.
    public Texture2D background; // Allows you to place a texture in the Inspector
    public Texture2D foreground;//Allows you to place a texture in the inspector
    public Texture2D bgPic;//Allows you to place a texture in, in the inspector

	public float getHealth() {
		return curHealth;
	}
	
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "PlaceHolderBullet(Clone)")
        {
            #if !UNITY_STANDALONE
			   Handheld.Vibrate(); //should probably make this a main menu choice
            #endif
            if (curHealth > 0)
            {
                //Debug.Log(" Health:" + curHealth);
                AdjustCurrentHealth(-2);
            }
			//curHealth <= 0 does not work since it would get called for all cars in game and health is not synced
            if (GameObject.Find("Managers").GetComponent<NetworkManager>().getPlayer().GetComponent<PlayerHealth>().getHealth() <= 0 ) 
            {
                //gameObject.SetActive(false);

                GetComponent<Rigidbody>().velocity = new Vector3(0, 50, 0);
                GetComponent<Rigidbody>().angularVelocity = new Vector3(20, 0, 20);
                StartCoroutine(GameOver(3));
            }
        }
        else if (col.gameObject.name == "Health(Clone)")
        {
            AdjustCurrentHealth(+45);
            if (curHealth > 100)
            {
                curHealth = 100f;
            }
        }
        else if (col.gameObject.name == "Car(Clone)")
        {
            #if !UNITY_STANDALONE
			    Handheld.Vibrate();
            #endif
        }
    }

    IEnumerator GameOver(float delay)
    {
        yield return new WaitForSeconds(delay);
		PhotonNetwork.Destroy(GameObject.Find("Managers").GetComponent<NetworkManager>().getPlayer());
    }


    void Start() // Use this for initialization, when the game stats what is displayed here will be loaded.
    {
        healthBarLength = Screen.width / 2; // The healthBarLength will be 1/2 the screens width.
    }


    void Update() // Update is called once per frame, to update information we are sending and receving.
    {
        AdjustCurrentHealth(0); // Calls appon AddjustCurHealth function to update the health.
    }

	private GameObject temp;
    void OnGUI()// set up for working with items in the GUI
    {
		temp = GameObject.Find("Managers").GetComponent<NetworkManager>().getPlayer();
		if (temp != null && temp.GetComponent<PlayerHealth>().getHealth() > 0 ) {
			GUI.DrawTexture(new Rect(30, 10, Screen.width / 5, 30), background);

			GUI.DrawTexture(new Rect(30, 10, healthBarLength, 30), foreground, ScaleMode.StretchToFill);

			GUI.DrawTexture(new Rect(30, 10, Screen.width / 5, 30), bgPic);
		}

    }

    public void AdjustCurrentHealth(int adj)//This function will allows us to alter our current health outside this script.
    {

        curHealth += adj;//This is to recieve heals or dammage to the CurHealth.  The number is passed in then assigned to curHealth.

        if (curHealth < 0)//Checks if the players health has gone below 0.
            curHealth = 0;// If players health has gone below 0 set it to 0.

        if (curHealth > maxHealth)//Checks if player health is higher then maxHealth.
            curHealth = maxHealth;//If players health is higher then maxHealth set it = to maxHeatlh

        if (maxHealth < 1)//Checks if maxHealth is set to less then 1.
            maxHealth = 1;//If maxHealth is set below 1, this sets it to 1.

        healthBarLength = (Screen.width / 5) * (curHealth / (float)maxHealth); // The full length of the bar * % of cur health.

    }

}

/*using UnityEngine;
using System.Collections;
 public class PlayerHealth : MonoBehaviour {
 
     public int maxHealth = 100;
     public int curHealth = 100;
 
     public Texture2D bgImage; 
     public Texture2D fgImage; 
    
     public float healthBarLength;
    
     // Use this for initialization
     void Start () {   
         healthBarLength = Screen.width /2;
     }
 
     // Update is called once per frame
     void Update () {
        AddjustCurrentHealth(0);
     }
 
     void OnGUI () {
         // Create one Group to contain both images
         // Adjust the first 2 coordinates to place it somewhere else on-screen
         GUI.BeginGroup (new Rect (0,0, healthBarLength,32));
 
         // Draw the background image
         GUI.Box (new Rect (0,0, healthBarLength,32), bgImage);
 
         // Create a second Group which will be clipped
         // We want to clip the image and not scale it, which is why we need the second Group
         GUI.BeginGroup (new Rect (0,0, curHealth / maxHealth * healthBarLength, 32));
 
         // Draw the foreground image
         GUI.Box (new Rect (0,0,healthBarLength,32), fgImage);
 
         // End both Groups
         GUI.EndGroup ();
 
         GUI.EndGroup ();
      }
 
     public void AddjustCurrentHealth(int adj){
 
		curHealth += adj;
 
        if(curHealth <0)
          curHealth = 0;
 
        if(curHealth > maxHealth)
          curHealth = maxHealth;
 
        if(maxHealth <1)
          maxHealth = 1;
 
        healthBarLength =(Screen.width /2) * (curHealth / (float)maxHealth);
     }
 }*/
