using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject boundingBoxes;

    [Header("Look Around Objects")]
    [SerializeField] int numOfLookCubes = 6;
    [SerializeField] SillyCube lookCube = null;

    [Header("Shoot Section")]
    [SerializeField] MoonSpawner moonSpawner = null;
    [SerializeField] List<AmmoTarget> ammoTargets = null;

    [Header("Warp portal")]
    [SerializeField] GameObject warpPortal = null;

    [SerializeField] AudioClip progressSound = null;
    AudioSource audioSource;

    InputControls inputControls = null;
    TMP_Text tutorialWords = null;
    InputCount inputCount = new InputCount();
    bool isWarpping = false;

    WaitForSeconds waitForChecking = new WaitForSeconds(.5f), 
        waitBeforeInitiating = new WaitForSeconds(3f);

    GameManager gameManager = null;

    // internal class used and only shown in tutorial
    class InputCount
    {
        AudioSource audioSource;
        AudioClip progressSound;

        public void SetAudio(AudioSource AS, AudioClip AC)
        {
            audioSource = AS;
            progressSound = AC;
        }

        void PlaySound(int pitch)
        {
            audioSource.PlayOneShot(progressSound);
            audioSource.pitch = pitch*0.2f + 1;
        }

        public InputCount()
        {
            forward = 0;
            back = 0;
            left = 0;
            right = 0;
            ascendend = 0;
            descend = 0;
        }

        public void CountMovementInput(Vector2 input)
        {
            if (input.x > 0)
            {
                if (left <= 2)
                    PlaySound(left);
                left++;
            }
            if (input.x < 0)
            {
                if (right <= 2)
                    PlaySound(right);
                right++;
            }
            if (input.y > 0)
            {
                if (forward <= 2)
                    PlaySound(forward);
                forward++;
            }
            if (input.y < 0)
            {
                if (back <= 2)
                    PlaySound(back);
                back++;
            }

        }

        public void CountFloatyInput(bool up, bool down)
        {
            if (up)
            {
                if (ascendend <= 2)
                    PlaySound(ascendend);
                ascendend++;
            }
            if (down)
            {
                if (descend <= 2)
                    PlaySound(descend);
                descend++;
            }
        }

        public bool IsAscendPressed()
        {
            int target = 2;
            return ascendend > target;
        }

        public bool IsDescendPressed()
        {
            int target = 2;
            return descend > target;
        }

        public bool IsMovementAllPressed()
        {
            int target = 2; // target amount you want to press the input

            return left > target && right > target && 
                forward > target && back > target;
        }

        int forward, back, left, right, ascendend, descend;
    }

    private void Awake()
    {
        inputControls = new InputControls();
        audioSource = GetComponent<AudioSource>();

#if UNITY_EDITOR
        if (lookCube == null)
        {
            Debug.LogWarning($"WRN : Look object prefab is missing.");
        }
#endif 
        foreach(var target in ammoTargets)
        {
            target.gameObject.SetActive(false);
        }

        inputCount.SetAudio(audioSource, progressSound);
    }

    private void OnEnable()
    {
        inputControls.Enable();
    }

    private void OnDisable()
    {
        inputControls.Player.Move.performed -= ctx => CountMovement(ctx.ReadValue<Vector2>());
        inputControls.Player.Jump.performed -= ctx => FloatingUp();
        inputControls.Player.Crouch.performed -= ctx => FloatingDown();

        inputControls.Disable();
    }

    IEnumerator Start()
    {
        gameManager = HelperFunctions.FindGameManagerInBaseScene();

        var waitBetweenInstructions = new WaitForSeconds(1f);

        tutorialWords = GetComponentInChildren<TMP_Text>();

        yield return LearnMovementCoroutine();
        audioSource.Play();
        yield return waitBetweenInstructions;

        yield return LearnFloatyUpCoroutine();
        audioSource.Play();
        yield return waitBetweenInstructions;

        yield return LearnFloatyDownCoroutine();
        audioSource.Play();
        yield return waitBetweenInstructions;

        yield return LearnLookCoroutine();
        audioSource.Play();
        yield return waitBetweenInstructions;

        yield return ShootMoonAtTargetCoroutine();
        audioSource.Play();
        yield return waitBetweenInstructions;

        yield return PlayerToWarpCoroutine();

    }

    IEnumerator LearnMovementCoroutine()
    {
        tutorialWords.text = "Press W,A,S,D or L.Stick on gamepad to move.";
        tutorialWords.gameObject.SetActive(true);
        yield return waitBeforeInitiating;
        
        inputControls.Player.Move.performed += ctx => CountMovement(ctx.ReadValue<Vector2>());

        while (!inputCount.IsMovementAllPressed())
        {
            yield return waitForChecking;
        }

        inputControls.Player.Move.performed -= ctx => CountMovement(ctx.ReadValue<Vector2>());

        tutorialWords.gameObject.SetActive(false);
    }

    IEnumerator LearnFloatyUpCoroutine()
    {
        // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/ActionBindings.html?_ga=2.150153078.1985191561.1604882948-1013150943.1559591277
        // string bindStr = inputControls.Player.Jump.GetBindingDisplayString(1);

        tutorialWords.text = "Press Space Bar or R.Bumper on Gamepad to float up.";
        tutorialWords.gameObject.SetActive(true);
        
        yield return waitBeforeInitiating;

        inputControls.Player.Jump.performed += ctx => FloatingUp();
        while (!inputCount.IsAscendPressed())
        {
            yield return waitForChecking;
        }

        inputControls.Player.Jump.performed -= ctx => FloatingUp();
        tutorialWords.gameObject.SetActive(false);
    }

    IEnumerator LearnFloatyDownCoroutine()
    {
        tutorialWords.text = "Press Shift or L.Bumper on Gamepad to float down.";
        tutorialWords.gameObject.SetActive(true);
        
        yield return waitBeforeInitiating;

        inputControls.Player.Crouch.performed += ctx => FloatingDown();
        while (!inputCount.IsDescendPressed())
        {
            yield return waitForChecking;
        }
        inputControls.Player.Crouch.performed -= ctx => FloatingDown();
        tutorialWords.gameObject.SetActive(false);
    }

    IEnumerator LearnLookCoroutine()
    {
        tutorialWords.text = "Look at the objects with the Mouse or R.Stick on Gamepad.";
        tutorialWords.gameObject.SetActive(true);
        
        List<SillyCube> sillyCubes = new List<SillyCube>();
        for(int i = 0; i < numOfLookCubes; i++)
        {
            var temp = Instantiate(lookCube, Vector3.zero + Random.onUnitSphere * 50f, Random.rotation);
            temp.gameObject.SetActive(false);
            sillyCubes.Add(temp);
        }
        
        foreach(var sillyCube in sillyCubes)
        {
            sillyCube.gameObject.SetActive(true);
            yield return new WaitForSeconds(Random.Range(.1f, .4f));
        }


        int focusedCount = 0, numOfSillyCubes = sillyCubes.Count;
        while (focusedCount != numOfSillyCubes)
        {
            int counter = 0;
            foreach(var sillyCube in sillyCubes)
            {
                if (sillyCube.HasBeenFocused) counter++;
            }
            focusedCount = counter;

            yield return waitForChecking;
        }

        tutorialWords.gameObject.SetActive(false);
        foreach(var go in sillyCubes)
        {
            go.gameObject.SetActive(false);
        }
    }

    IEnumerator ShootMoonAtTargetCoroutine()
    {
        tutorialWords.text = "Collect Moons. If you can't find them, hold Right Mouse Button or L.Trigger on Gamepad to scan.";
        tutorialWords.gameObject.SetActive(true);
        
        yield return waitBeforeInitiating;
        moonSpawner.InitiateSpawner();

        var player = gameManager.PlayerController;
        while(player.IsAmmoEmpty)
        {
            yield return waitForChecking;
        }

        tutorialWords.gameObject.SetActive(false);
        tutorialWords.text = "You can scan many other things in space using Right Mouse Button or L.Trigger on Gamepad.";
        tutorialWords.gameObject.SetActive(true);

        yield return new WaitForSeconds(9f);

        tutorialWords.gameObject.SetActive(false);
        tutorialWords.text = "Shoot Moon at the center of the Target with Left Mouse Button or R.Trigger on Gamepad.";
        tutorialWords.gameObject.SetActive(true);

        foreach (var ammoTarget in ammoTargets)
        {
            ammoTarget.gameObject.SetActive(true);
        }

        int targetGoals = 0, numOfTargets = ammoTargets.Count;
        while (targetGoals != numOfTargets)
        {
            int counter = 0;
            foreach(var ammoTarget in ammoTargets)
            {
                if (ammoTarget.HasAmmoPassedThrough) counter++;
            }
            targetGoals = counter;

            yield return waitForChecking;
        }

        tutorialWords.gameObject.SetActive(false);
    }

    IEnumerator PlayerToWarpCoroutine()
    {
        boundingBoxes.SetActive(false);

        tutorialWords.text = "Enter through the portal.";
        tutorialWords.gameObject.SetActive(true);

        yield return new WaitForSeconds(6f);

        tutorialWords.gameObject.SetActive(false);
        tutorialWords.text = "Good luck...";
        tutorialWords.gameObject.SetActive(true);

        var playerCamTransform = gameManager.PlayerController.PlayerCamera.transform;
        var spawnPosition = playerCamTransform.position + playerCamTransform.forward * 250f;
        var spawnRotation = playerCamTransform.rotation;
        warpPortal.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
        warpPortal.SetActive(true);
    }

    void CountMovement(Vector2 input)
    {
        inputCount.CountMovementInput(input);
    }

    void FloatingUp()
    {
        inputCount.CountFloatyInput(true, false);
    }

    void FloatingDown()
    {
        inputCount.CountFloatyInput(false, true);
    }
}
