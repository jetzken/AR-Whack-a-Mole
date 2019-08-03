using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GameManager_Script : MonoBehaviour {

    public static bool GameOver;

    public int maxSpawnDelaySec = 3; // max time before activating new mole
    public int maxActiveMoles = 3; // maximum moles active at a time
    public Camera arGameCam; // game camera

    private List<Mole_Script> moleList = new List<Mole_Script>(); // store all moles in game

    private static GameManager_Script instance; // holds instance of class
    
    public static GameManager_Script Instance
    {
        get
        {
            if (instance == null) // should never happen
                Debug.Log("Error: Instance does not exist");
            return instance;
        }
    }

    private void Awake() // called before start
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        GameOver = false;
    }

    // Update is called once per frame
    void Update()
    {

        // controls
        // mouse
        #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = arGameCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                foreach (Mole_Script m in moleList)
                {
                    if (m.ReturnMolePrefab().activeSelf && m.ReturnMolePrefab() == hit.transform.gameObject)
                    {
                        MoleHit(m);
                    }
                }
            }
        }
        #endif

        // touch
        if (Input.touchCount > 0)
        {
            RaycastHit hit = new RaycastHit();
            foreach (Touch t in Input.touches)
            {
                Ray ray = arGameCam.ScreenPointToRay(t.position);
                if (Physics.Raycast(ray, out hit))
                {
                    foreach (Mole_Script m in moleList)
                    {
                        if (m.ReturnMolePrefab().activeSelf && m.ReturnMolePrefab() == hit.transform.gameObject)
                        {
                            MoleHit(m);
                        }
                    }
                }
            }
        }

        if (!GameOver && !PauseMenu_Script.IsPaused)
        {
            RunGame();

            // check if time up
            if (Timer_Script.CurrentTime <= 0)
                GameOver = true;
        }
    }

    private void RunGame() // main game loop
    {
        // wait a random time before activating new mole
        float delayTimer = 0f;
        float currentSpawnDelay = Random.Range(1, (maxSpawnDelaySec * 1000)) / 1000;
        while (delayTimer < currentSpawnDelay)
        {
            delayTimer += Time.deltaTime;
        }

        // check if there are moles to activate
        if (CanActivateBool())
        {
            // activate random inactive mole
            if (MolesInactive() > 0)
            {
                int random = (int)Random.Range(0, moleList.Count);
                while (moleList[random].ReturnMolePrefab().activeSelf) // generate new random until selected mole is inactive
                    random = (int)Random.Range(0, moleList.Count);

                // randomise mole hit wait time
                float newHitTime = Random.Range(0.5f, 2);
                moleList[random].ActivateMole(newHitTime);
            }
        }
    }

    // count number of active moles
    private bool CanActivateBool()
    {
        int currentActiveMoles = 0;
        for (int i = 0; i < moleList.Count; i++)
        {
            if (moleList[i].ReturnMolePrefab().activeSelf)
                currentActiveMoles++;
        }

        if (currentActiveMoles < maxActiveMoles)
            return true;
        else
            return false;
    }

    private int MolesInactive()
    {
        // determine if there are any inactive moles
        // useful if total moles in game is the same
        // as maxActiveMoles
        int molesInactive = 0;
        int i = 0;
        while (molesInactive == 0 && i < moleList.Count)
        {
            if (!moleList[i].ReturnMolePrefab().activeSelf)
                    molesInactive++;
            i++;
        }

        return molesInactive;
    }

    private void MoleHit(Mole_Script m)
    {
        Score_Script.Score += m.Whacked() ? 0 : 10; // only add score if mole isnt "whacked"
        m.Whack();
    }

    public void AddMole(GameObject moleUnit)
    {
        moleList.Add(moleUnit.GetComponent<Mole_Script>());
    }

}
