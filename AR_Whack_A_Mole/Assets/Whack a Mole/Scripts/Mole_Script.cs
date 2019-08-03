using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_Script : MonoBehaviour {

    public Transform MolePrefab;

    private float speed; // mole up/down speed
    private float timeLimit; // whack wait time
    private bool whacked;
    private float upPos, downPos;

	// Use this for initialization
    void Start () {
        MolePrefab.GetChild(0).gameObject.SetActive(false);
        timeLimit = 1.0f;
        speed = 30.0f;

        // set up and down yPos values
        float height = MolePrefab.GetComponent<Collider>().bounds.size.y; // get bounding box size for mole
        upPos = height / 2;
        downPos = -(height / 2);

        // move mole below hole
        Vector3 temp = MolePrefab.localPosition;
        temp.y = downPos;
        MolePrefab.localPosition = temp;


        MolePrefab.gameObject.SetActive(false);
        GameManager_Script.Instance.AddMole(gameObject); // allows for flexible number of moles
    }

    public GameObject ReturnMolePrefab ()
    {
        return MolePrefab.gameObject;
    }

    // Update is called once per frame
    void Update () {

	}

    public void ActivateMole (float newTime)
    {
        whacked = false;
        MolePrefab.GetChild(0).gameObject.SetActive(false);
        timeLimit = newTime;
        MolePrefab.gameObject.SetActive(true);
        StartCoroutine(MainMoleLoop());
    }

    private IEnumerator MainMoleLoop() // core mole actions
    {
        yield return StartCoroutine(MoveMoleUp());
        yield return StartCoroutine(WaitForHit());
        yield return StartCoroutine(MoveMoleDown());
    }

    private IEnumerator MoveMoleUp()
    {
        while (MolePrefab.localPosition.y < upPos) // while mole is below hole
        {
            if (!PauseMenu_Script.IsPaused)
            {
                Vector3 tempPos = MolePrefab.localPosition;
                float newYPos = tempPos.y + (speed * Time.deltaTime); // update y pos based on speed
                tempPos.y = newYPos > upPos ? upPos : newYPos; // if newYPos > height, set tempPos.y to height
                MolePrefab.localPosition = tempPos;
            }

            yield return null;
        }
    }

    private IEnumerator WaitForHit() // mole remains static until time is up or hit
    {
        float time = 0.0f;
        while (!whacked && time < timeLimit) // loop while mole not hit and time not up
        {
            if (!PauseMenu_Script.IsPaused)
            {
                time += Time.deltaTime;
            }
            yield return null;
        }
    }

    private IEnumerator MoveMoleDown()
    {
        while (MolePrefab.localPosition.y > downPos) // while mole is above
        {
            if (!PauseMenu_Script.IsPaused)
            {
                Vector3 tempPos = MolePrefab.localPosition;
                float newYPos = tempPos.y - (speed * 2 * Time.deltaTime); // update y pos based on speed
                tempPos.y = newYPos < downPos ? downPos : newYPos; // if newYPos < 0, set tempPos.y to 0
                MolePrefab.localPosition = tempPos;
            }

            yield return null;
        }

        MolePrefab.gameObject.SetActive(false); // deactivate once back in hole
    }

    public void Whack()
    {
        whacked = true;
        MolePrefab.GetChild(0).gameObject.SetActive(true);
        GetComponent<SFX_Script>().PlayMoleHit();
    }

    public bool Whacked()
    {
        return whacked;
    }
}
