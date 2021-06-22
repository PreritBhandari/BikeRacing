using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class GearRatio
{
    public String gearName = "N";
    public int gearMinSpeed = 0;
    public int gearMaxSpeed = 0;
}

[Serializable]
public class MotoControlsGUI
{
    public enum controlerUI { GUIButton, CanvasPanel };
    public controlerUI ControllerKind = controlerUI.GUIButton;

    public GUISkin Skin = null;

    public Texture2D Accelerate = null;
    public Vector2 AccSize = Vector2.zero;

    public enum AccVAlign { Top, Bottom };
    public AccVAlign accVAlign = AccVAlign.Top;
    public enum AccHAlign { Right, Left };
    public AccHAlign accHAlign = AccHAlign.Left;

    public Vector2 AccPosition = Vector2.zero;

    public Texture2D Break = null;
    public Vector2 BreakSize = Vector2.zero;

    public enum BreakVAlign { Top, Bottom };
    public BreakVAlign breakVAlign = BreakVAlign.Top;

    public enum BreakHAlign { Right, Left };
    public BreakHAlign breakHAlign = BreakHAlign.Left;

    public Vector2 BreakPosition = Vector2.zero;

    [HideInInspector]
    public int score = 0;
}

[Serializable]
public class MotoSpeedoMeter
{
    public Texture2D speedoDial = null;
    public Texture2D speedoNeedle = null;
    public Texture2D speedoKnot = null;

    [Range (.1f, 4.0f)]
    public float speedoDialScaleRatio = 0.7f;
    public Vector2 speedoDialPositionOffset = new Vector2(0,0);

    [Range(.1f, 4.0f)]
    public float SpeedMeterKnotScaleRatio = 0.7f;
    public Vector2 SpeedMeterKnotPositionOffset = new Vector2(74, 80);

    [Range(.1f, 4.0f)]
    public float speedoNeedleScaleRatio = 0.5f;
    public Vector2 speedoNeedlePositionOffset = new Vector2(84.3f, 91.22f);

    public float speedoNeedleStartAngle = 131.51f;
    public float speedoNeedleEndAngle = 312.14f;

    public Vector2 speedoDigitalSpeedPosition = new Vector2(81.8f, 118.79f);
    public Vector2 speedoUnitMetricLabelPosition = new Vector2(71.45f, 140.79f);
    public Vector2 speedoGearLabelPosition = new Vector2(70.0f, 123.0f);

    public string UnitImperialText = "MPH";
    public string UnitMetricText = "Km/h";
}

[Serializable]
public class MotoBikeSettings
{
    public enum RimRotationAxis
    {
        Right,
        Forward,
        Up
    }

    //Rim rotation Axis Selector
    public RimRotationAxis wheelRotationAxis = RimRotationAxis.Forward;

    //Wheels array for 1 or more wheel vehicles compatibility 
    public Transform[] WheelAndRims = null;

    //Maximum Speed this player can reach
    [Range (1, 450)]
    public int MaxSpeed = 145;

    [Range(1, 60)]
    public int MinSpeed = 35;

    public GearRatio[] gearRatio = null;

    //The amount of speed automated addition mph or kmh
    public int speedIncreaseInTime = 3;

    //Break point Light
    public Light breakLight = null;

    //Sense of mobile movement acceleration
    public float MobileControlSense = 5.0f;
    
    //Sense of mouse movement
    public float MouseSense = 2.0f;

    //Acceleration Speedup when press the Acc
    public int AccelerationInMetric = 1;

    //Break Force
    public int BreakForce = 2;

    //Auto Break Force | higher value slower vehicle slow down
    public int AutoBreakForceOn = 5;

}

[Serializable]
public class MotoController
{
    //Game Main Camera
    [HideInInspector]
    public Camera PlayerCamera = null;

    public enum UnitMetric
    {
        Imperial,
        Metric
    }

    public UnitMetric unitMetric = UnitMetric.Imperial;

    public enum InputController
    {
        Mouse,
        Accelerometer
    }

    //Type of control input
    public InputController inputController = InputController.Mouse;

    public KeyCode pauseKeyCode = KeyCode.P;
    public Vector2 pausePositionHUD = Vector2.zero;

    //Current Speed of Player mph or kmh
    [HideInInspector]
    public int playerCurrentSpeed = 30;

    public bool autoSpeedCameraFOV = true;
    public int autoCamMinFOV = 100;
    public int autoCamMaxFOV = 115;

    //The Speed by unit for using in unity translate
    [HideInInspector]
    public float currentSpeedUnit = 1.0f;

    [HideInInspector]
    public float horizontalAxis = 0.0f;

    [HideInInspector]
    public int currentGearIndex = 0;

    //when gear is changing the vehicle stop accelerate
    [HideInInspector]
    public bool AccGearChange = false;

    //Player live
    [HideInInspector]
    public bool isAlive = true;

    [HideInInspector]
    public bool isGameOver = false;

    [HideInInspector]
    public bool isPaused = false;

    [HideInInspector]
    public bool isAccGUI = false;

    [HideInInspector]
    public bool isBreakGUI = false;

    [HideInInspector]
    public bool isIntroScene = false;

    //Camera Offset from the biker
    public Vector3 cameraOffset = new Vector3(0.0f, -1.4f, -0.26f);

    //Camera Rotation Offset
    public Vector3 cameraRotationOffset = Vector3.zero;

    [HideInInspector]
    public bool applyRotation = false;

    //Road Right Side / This is just like putting collider near the road, in other word
    //it is near line auto wall collider without collider mesh and with just limiting the player x axis movement
    public Transform roadRight, roadLeft = null;

    public void cameraFollow(Transform playerTransform)
    {
        //update camera with player position Z
        PlayerCamera.transform.position = new Vector3(cameraOffset.x, playerTransform.position.y, playerTransform.position.z) - cameraOffset;
    }
}

[Serializable]
public class MotoTools
{
    //each 60 miles equals to 100 kilometer
    private float mile = 60.0f;
    private float kilometer = 100.0f;

    //we just feel 7 is equal to 100 kmh so >>
    private float feelUnitMetric = 8.0f;
    private float feelUnitImperial = 5.2f;

    public int UnitToMetric(float unit)
    {
        return Mathf.RoundToInt( (kilometer * unit) / feelUnitMetric );
    }

    public int UnitToImperial(float unit)
    {
        return Mathf.RoundToInt((mile * unit) / feelUnitImperial);
    }

    public float ImperialToUnit(int value)
    {
        return (value * feelUnitImperial) / mile;
    }

    public float MetricToUnit(int value)
    {
        return (value * feelUnitMetric) / kilometer;
    }
}

[Serializable]
public class MotoSounds
{
    public AudioClip engineSound = null;
    
    [Range (0, 1)]
    public float engineVolume = 0.5f;
}

[AddComponentMenu("Noir Project/Simple Infinite MotoBiker/Player Controller")]

public class Player : MonoBehaviour 
{
    public MotoController motoController;
    public MotoBikeSettings motoSettings;
    public MotoSpeedoMeter motoSpeedoMeter;
    public MotoControlsGUI motoControlsGUI;
    public MotoSounds motoSounds;

    [HideInInspector]
    public MotoTools motoTools;

    //Temoprary value for store counter of the auto speed reduce
    private int temporaryBreakForceCounter = 0;

    void Start()
    {
        motoController.isAlive = true;

        //Add HUD on Player
        if (gameObject.GetComponent<MotoHUD>() == null)
            gameObject.AddComponent<MotoHUD>();

        //Add audio source on player for casting moto sound
        if (gameObject.GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();
            if (motoSounds.engineSound != null)
            {
                gameObject.GetComponent<AudioSource>().clip = motoSounds.engineSound;
                gameObject.GetComponent<AudioSource>().loop = true;
                gameObject.GetComponent<AudioSource>().volume = motoSounds.engineVolume;
                gameObject.GetComponent<AudioSource>().Play();
            }
        }

        //Define main camera for further access
        if (Camera.main.GetComponent<Camera>() != null)
        {
            motoController.PlayerCamera = Camera.main.GetComponent<Camera>();

            //update camera rotation with offset
            motoController.PlayerCamera.transform.eulerAngles = motoController.PlayerCamera.transform.eulerAngles +motoController.cameraRotationOffset;
        }


    }

    void Update()
    {
        if (motoController.isAlive)
        {
            //Pause player when press Pause key
            if (Input.GetKeyDown(motoController.pauseKeyCode))
                motoController.isPaused = !motoController.isPaused;

            //When controller is Accelerometer then the gui buttons are functioning 
            if (motoController.inputController == MotoController.InputController.Accelerometer)
            {
                if (motoController.isAccGUI)
                    acc();

                if (motoController.isBreakGUI)
                    mBreak();
            }
        }
        else
        {
            //When player is dead, this function is called
            DeadBiker();
        }
    }

    void FixedUpdate()
    {
        //When game is not paused and player is alive
        if (motoController.isAlive && !motoController.isPaused)
        {
            //Camera Follow
            motoController.cameraFollow(transform);
            //Movement Controller
            MotoMover();
            //GearManager | Handle current gear from the speed
            gearManager();
            //Rotate the rims by calculating speed and Axis
            RimRotator();
            //AutoCamera Field Of View
            autoCameraFov();
            //This will reduce speed when player not accelerate even break
            autoSpeedReduction();
            //Engine Sounds
            engineSound();
        }
    }

    //Handle Engine sound when sound is available
    public void engineSound()
    {
        if (GetComponent<AudioSource>() != null)
        {
            if (motoSounds.engineSound != null)
            {
                float normalizedSpeed = (float)motoController.playerCurrentSpeed / motoSettings.MaxSpeed;
                GetComponent<AudioSource>().pitch = Mathf.Lerp(0.4f, 1.0f, normalizedSpeed);
            }
        }
    }

    //This will handle current gear by calculating the speed
    public void gearManager()
    {
        float delay = 0.1f;

        //it seems to be gears are set
        if (motoSettings.gearRatio.Length > 0 && !motoController.AccGearChange)
        {
            int cnt = 0;
            foreach (GearRatio g in motoSettings.gearRatio)
            {
                if (motoController.playerCurrentSpeed <= g.gearMaxSpeed && motoController.playerCurrentSpeed >= g.gearMinSpeed)
                {
                    if (motoSettings.gearRatio[motoController.currentGearIndex].gearName != g.gearName)
                    {
                        StartCoroutine(changeGearDelaySimulator(delay, cnt));
                    }
                }
                cnt++;
            }
        }
    }

    //This will handle the waiting time of gear change
    IEnumerator changeGearDelaySimulator(float seconds, int gearIndex)
    {
        //now we change the gear
        motoController.AccGearChange = true;

        //you should have a gear named N on the first gear slot because when we change the gear, the game is in N position
        motoController.currentGearIndex = 0;

        yield return new WaitForSeconds(seconds);

        //set the new gear name
        motoController.currentGearIndex = gearIndex;

        //changing gear is finished
        motoController.AccGearChange = false;
    }

    void autoCameraFov()
    {
        if (motoController.autoSpeedCameraFOV)
        {
            float normalizedSpeed = (float)motoController.playerCurrentSpeed / motoSettings.MaxSpeed;
            float fov = Mathf.Lerp(motoController.autoCamMinFOV, motoController.autoCamMaxFOV, normalizedSpeed);

            Camera.main.fieldOfView = fov;
        }
    }

    //This will move the vehicle
    public void MotoMover()
    {
        //Get mouse Horizontal * Speed of mouse
        if (motoController.inputController == MotoController.InputController.Mouse)
            motoController.horizontalAxis = Input.GetAxis("Mouse X") * motoSettings.MouseSense;

        //Get horizontal from acceletometer
        if (motoController.inputController == MotoController.InputController.Accelerometer)
            motoController.horizontalAxis = Input.acceleration.x * motoSettings.MobileControlSense;

        //When unit is set to Imperial
        if (motoController.unitMetric == MotoController.UnitMetric.Imperial)
        {
            //convert speed from metric to unit for using in translate
            motoController.currentSpeedUnit = motoTools.ImperialToUnit(motoController.playerCurrentSpeed);
        }

        //When unit is set to Metric
        if (motoController.unitMetric == MotoController.UnitMetric.Metric)
        {
            //convert speed from metric to unit for using in translate
            motoController.currentSpeedUnit = motoTools.MetricToUnit(motoController.playerCurrentSpeed);
        }

        //Accelerate
        transform.Translate(Vector3.forward * motoController.currentSpeedUnit * Time.deltaTime);

        //Left / Right Movement of player with Mouse or Accelometer of Mobile Device
        transform.Translate(Vector3.right * motoController.horizontalAxis * Time.deltaTime);

        //set left side of the road
        if (transform.position.x < motoController.roadLeft.position.x)
            transform.position = new Vector3(motoController.roadLeft.position.x, transform.position.y, transform.position.z);

        //set the right side of the road
        if (transform.position.x > motoController.roadRight.position.x)
            transform.position = new Vector3(motoController.roadRight.position.x, transform.position.y, transform.position.z);

        mouseControl();
    }

    //acceleration system
    public void acc()
    {
        //check the speed to never be higher than the maximum Speed
        if (motoSettings.MaxSpeed >= motoController.playerCurrentSpeed && motoController.currentGearIndex != 0)
        {
            //when unit is imperial, accelerate from imperial unit conversion
            if (motoController.unitMetric == MotoController.UnitMetric.Imperial)
                motoController.playerCurrentSpeed += motoTools.UnitToImperial(motoTools.ImperialToUnit(motoSettings.AccelerationInMetric));

            if (motoController.unitMetric == MotoController.UnitMetric.Metric)
                motoController.playerCurrentSpeed += motoTools.UnitToImperial(motoTools.MetricToUnit(motoSettings.AccelerationInMetric));
        }

        //Turn the break light out | arg is light intensity | 0: off
        TurnTheBreakLight(0);
    }

    //break system
    public void mBreak()
    {
        if (motoSettings.MinSpeed < motoController.playerCurrentSpeed)
        {
            if (motoController.unitMetric == MotoController.UnitMetric.Imperial)
                motoController.playerCurrentSpeed -= motoTools.UnitToImperial(motoTools.ImperialToUnit(motoSettings.BreakForce));

            if (motoController.unitMetric == MotoController.UnitMetric.Metric)
                motoController.playerCurrentSpeed -= motoTools.UnitToImperial(motoTools.MetricToUnit(motoSettings.BreakForce));
        }

        //Break Light Should be appear | arg is light intensity
        TurnTheBreakLight(16);
    }

    void autoSpeedReduction()
    {
        //When mouse control is enabled and the buttons are not pressed
        if (!Input.GetMouseButton(1) && 
            !Input.GetMouseButton(0) && 
            !motoController.isAccGUI &&
            !motoController.isBreakGUI)
        {
            //add 1 to counter
            temporaryBreakForceCounter++;

            //check if not lower than min speed
            if (motoSettings.MinSpeed < motoController.playerCurrentSpeed && temporaryBreakForceCounter == motoSettings.AutoBreakForceOn)
            {
                motoController.playerCurrentSpeed -= motoTools.UnitToImperial(motoTools.MetricToUnit(1));
            }

            //Reset the counter
            if (temporaryBreakForceCounter >= motoSettings.AutoBreakForceOn)
                temporaryBreakForceCounter = 0;

            //Turn the break light out | arg is light intensity | 0: off
            TurnTheBreakLight(0);
        }

        //When reach over max speed
        if (motoController.playerCurrentSpeed > motoSettings.MaxSpeed)
        {
            temporaryBreakForceCounter++;

            if (temporaryBreakForceCounter == motoSettings.AutoBreakForceOn)
                motoController.playerCurrentSpeed -= motoTools.UnitToImperial(motoTools.MetricToUnit(1));
            
            TurnTheBreakLight(0);

            if (temporaryBreakForceCounter >= motoSettings.AutoBreakForceOn)
                temporaryBreakForceCounter = 0;
        }

    }

    //This will handle mouse controls
    public void mouseControl()
    {
        //Accelerate vehicle with mouse
        if (Input.GetMouseButton(0) && motoController.inputController == MotoController.InputController.Mouse)
        {
            acc();
        }

        //Break with mouse
        if (Input.GetMouseButton(1) && motoController.inputController == MotoController.InputController.Mouse) //Break
        {
            mBreak();
        }
    }

    //When the biker is dead, this function is called
    public void DeadBiker()
    {
        //set the player speed to 0
        motoController.playerCurrentSpeed = 0;

        //Set game over mode enable
        if (!motoController.isGameOver)
        {
            //Set game is over because we want this function run just once
            motoController.isGameOver = true;

            //turn of the break light when it's on
            TurnTheBreakLight(0);

            //Create savegame object for save game process
            MotoSavegame saveGame = new MotoSavegame();

            //save the score and compare highscore
            saveGame.saveScore(motoControlsGUI.score);
            
            //load GameOver Screen to show score and highscore
            //also the restart game button and mainmenu
            SceneManager.LoadScene("gameOver");
        }

        
    }

    //This function rotate the rims by player movement speed
    public void RimRotator()
    {
        int RotationMultiplier = 50;
        float speedInTime = motoController.playerCurrentSpeed * RotationMultiplier * Time.deltaTime;

        if (motoSettings.WheelAndRims.Length > 0)
        {
            foreach (Transform t in motoSettings.WheelAndRims)
            {
                if (motoSettings.wheelRotationAxis == MotoBikeSettings.RimRotationAxis.Right)
                    t.transform.Rotate(Vector3.right * speedInTime);

                if (motoSettings.wheelRotationAxis == MotoBikeSettings.RimRotationAxis.Forward)
                    t.transform.Rotate(Vector3.forward * speedInTime);

                if (motoSettings.wheelRotationAxis == MotoBikeSettings.RimRotationAxis.Up)
                    t.transform.Rotate(Vector3.up * speedInTime);
            }
        }
    }

    void TurnTheBreakLight(float intensity)
    {
        if (motoSettings.breakLight != null)
            motoSettings.breakLight.intensity = intensity;
    }
}