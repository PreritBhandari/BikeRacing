using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("Noir Project/Simple Infinite MotoBiker/HUD")]
public class MotoHUD : MonoBehaviour
{
    private Player player = null;

    //Canvas and panel setup
    private Canvas canvas = null;
    private EventSystem eventSystem = null;
    private GameObject eventObject = null;
    private GameObject canvasObject = null;
    private GameObject panelObject = null;
    private GameObject accObject = null;
    private Button accButton = null;
    private GameObject breakObject = null;
    private Button breakButton = null;

    //change time to clock
	int sec,min,hour = 0;
	int timer = 0;

	//for change value to string
	string sSec, sMin, sHour;

    int screenAspectRatio = (Screen.width / Screen.height);

    // Use this for initialization
    void Start()
    {
        //get the player component
        if (GetComponent<Player>() != null)
            player = GetComponent<Player>();

        if (player != null)
        {
            //when the controller kind is Canvas Panel use this to create canvas and panel
            if (player.motoController.inputController == MotoController.InputController.Accelerometer &&
                player.motoControlsGUI.ControllerKind == MotoControlsGUI.controlerUI.CanvasPanel)
            {
                ControlsCanvasPanel();
            }
        }
    }


    // Update is called once per frame
    void OnGUI()
    {
        if (player != null)
        {
            //pause
            pauseUI();
        }

        if (player != null && player.motoController.isAlive && !player.motoController.isPaused)
        {
            if (player.motoControlsGUI.Skin != null)
                GUI.skin = player.motoControlsGUI.Skin;

            //analog odo meter
            speedoMeterAnalog();
            
            //Speed and Gear indicator
            speedoDigital();

            //Control Buttons, such as: acc, break when controller is accelerometer
            if (player.motoController.inputController == MotoController.InputController.Accelerometer &&
                player.motoControlsGUI.ControllerKind == MotoControlsGUI.controlerUI.GUIButton)
            {
                ControlsGUIButton();
            }

            //Display time on screen
            TimeClock();

            //Score
            displayScore();
        }
    }

    void displayScore()
    {
        Color oldcolor = GUI.color;
        GUI.color = Color.black;
        GUI.skin.label.fontSize = 25;

        float normalizedSpeed = (float)player.motoController.playerCurrentSpeed / player.motoSettings.MaxSpeed;
        player.motoControlsGUI.score += (int) Mathf.Lerp(1, 3, normalizedSpeed) ;

        //show the time on gui
        GUI.Label(new Rect(Screen.width - 200,
            player.motoSpeedoMeter.speedoDialPositionOffset.y + 40, 200, 30), player.motoControlsGUI.score.ToString());

        GUI.color = oldcolor;
    }

    void TimeClock()
    {
        Color oldcolor = GUI.color;
        GUI.color = Color.black;
        GUI.skin.label.fontSize = 25;

        if (player.motoController.isAlive)
			//get the time
			timer = Mathf.RoundToInt(Time.time);
		
		// get MOD time 60 for sec 
		sec = timer % 60;
		// get time /60 for min
		min = timer / 60;
		// get min / 60 for hour 
		hour = min / 60;
		
		
		//Add 0 for time format 00:00:00
		if (sec.ToString().Length == 1)
			sSec = "0" + sec.ToString();
		else
			sSec = sec.ToString();
		
		//minute format
		if (min.ToString().Length == 1)
			sMin = "0" + min.ToString();
		else
			sMin = min.ToString();

		//hour format
		if (hour.ToString().Length == 1)
			sHour = "0" + hour.ToString();
		else
			sHour = hour.ToString();
		
			
		//show the time on gui
        GUI.Label(new Rect(Screen.width - 200, 
            player.motoSpeedoMeter.speedoDialPositionOffset.y, 200, 30), sHour.ToString() + ":" + sMin.ToString() + ":" + sSec.ToString());

        GUI.color = oldcolor;
    }

    //Control with canvas panel button
    void ControlsCanvasPanel()
    {
        //create canvas
        if (canvasObject == null)
        {
            canvasObject = new GameObject("CanvasUI");
            if (canvas == null)
            {
                canvas = canvasObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObject.AddComponent<CanvasScaler>();
                canvasObject.AddComponent<GraphicRaycaster>();
            }
        }

        //Event system
        if (eventObject == null)
        {
            eventObject = new GameObject("EventSystemUI");
            eventObject.AddComponent<EventSystem>();
            eventObject.AddComponent<StandaloneInputModule>();
        }

        //create panel
        if (panelObject == null)
        {
            panelObject = new GameObject("PanelUI");
            panelObject.AddComponent<CanvasRenderer>();
            panelObject.transform.parent = canvasObject.transform;
        }


        //create acc button
        if (accObject == null)
        {
            accObject = new GameObject("AccelerateButton");
            accObject.AddComponent<Button>();
            accButton = accObject.GetComponent<Button>();
            accObject.transform.parent = canvasObject.transform;

            accObject.AddComponent<RectTransform>();
            Vector2 accPosition = Vector2.zero;

            //align left
            if (player.motoControlsGUI.accHAlign == MotoControlsGUI.AccHAlign.Left)
                accPosition.x = player.motoControlsGUI.AccPosition.x;
            else
                accPosition.x = Screen.width - player.motoControlsGUI.AccPosition.x;

            //align bottom
            if (player.motoControlsGUI.accVAlign == MotoControlsGUI.AccVAlign.Bottom)
                accPosition.y = player.motoControlsGUI.AccPosition.y;
            else
                accPosition.y = Screen.height - player.motoControlsGUI.AccPosition.y;

            accButton.GetComponent<RectTransform>().position = accPosition;
            accButton.GetComponent<RectTransform>().sizeDelta = player.motoControlsGUI.AccSize;
            
            accObject.AddComponent<CanvasRenderer>();

            //configure image
            accObject.AddComponent<Image>();
            Texture2D texture = player.motoControlsGUI.Accelerate as Texture2D;
            Image imgacc = accObject.GetComponent<Image>();
            imgacc.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            accObject.AddComponent<EventTrigger>();
            
            //Creating Event Trigger
            EventTrigger accTrigger = accObject.GetComponent<EventTrigger>();
            EventTrigger.Entry accEntryPressed = new EventTrigger.Entry();
            EventTrigger.Entry accEntryNotPressed = new EventTrigger.Entry();
            
            //Event: Accelerate Pressed
            accEntryPressed.eventID = EventTriggerType.PointerDown;
            accEntryPressed.callback.AddListener((data) => { player.motoController.isAccGUI = true; });
            accTrigger.triggers.Add(accEntryPressed);

            //Event Accelerate Not Pressed
            accEntryNotPressed.eventID = EventTriggerType.PointerUp;
            accEntryNotPressed.callback.AddListener((data => { player.motoController.isAccGUI = false; }));
            accTrigger.triggers.Add(accEntryNotPressed);
        }

        //Create Break button
        if (breakObject == null)
        {
            breakObject = new GameObject("BreakButton");
            breakObject.AddComponent<Button>();
            breakButton = breakObject.GetComponent<Button>();
            breakObject.transform.parent = canvasObject.transform;
            
            //Position
            breakObject.AddComponent<RectTransform>();
            Vector2 breakPosition = Vector2.zero;

            //align left / right
            if (player.motoControlsGUI.breakHAlign == MotoControlsGUI.BreakHAlign.Left)
            {
                breakPosition.x = player.motoControlsGUI.BreakPosition.x;
            }
            else
            {
                breakPosition.x = Screen.width - player.motoControlsGUI.BreakPosition.x;
            }

            //align top/bottom
            if (player.motoControlsGUI.breakVAlign == MotoControlsGUI.BreakVAlign.Bottom)
            {
                breakPosition.y = player.motoControlsGUI.BreakPosition.y;
            } else {
                breakPosition.y = Screen.height - player.motoControlsGUI.BreakPosition.y;
            }

            //set new position
            breakButton.GetComponent<RectTransform>().position = breakPosition;
            
            //set size
            breakButton.GetComponent<RectTransform>().sizeDelta = player.motoControlsGUI.BreakSize;

            breakObject.AddComponent<CanvasRenderer>();
            
            //configure image of button            
            breakObject.AddComponent<Image>();
            Texture2D texture = player.motoControlsGUI.Break as Texture2D;
            Image img = breakObject.GetComponent<Image>();
            img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);


            breakObject.AddComponent<EventTrigger>();

            //Creating Event Trigger
            EventTrigger breakTrigger = breakObject.GetComponent<EventTrigger>();
            EventTrigger.Entry breakEntryPressed = new EventTrigger.Entry();
            EventTrigger.Entry breakEntryNotPressed = new EventTrigger.Entry();

            //Event: Accelerate Pressed
            breakEntryPressed.eventID = EventTriggerType.PointerDown;
            breakEntryPressed.callback.AddListener((data) => { player.motoController.isBreakGUI = true; });
            breakTrigger.triggers.Add(breakEntryPressed);

            //Event Accelerate Not Pressed
            breakEntryNotPressed.eventID = EventTriggerType.PointerUp;
            breakEntryNotPressed.callback.AddListener((data => { player.motoController.isBreakGUI = false; }));
            breakTrigger.triggers.Add(breakEntryNotPressed);
        }
    }

    //Controls using GUI Button / may not good for mobile touch
    void ControlsGUIButton()
    {
       if (GUI.RepeatButton(new Rect((player.motoControlsGUI.accHAlign == MotoControlsGUI.AccHAlign.Right ? Screen.width - player.motoControlsGUI.AccPosition.x : player.motoControlsGUI.AccPosition.x),
                                (player.motoControlsGUI.accVAlign == MotoControlsGUI.AccVAlign.Bottom ? Screen.height - player.motoControlsGUI.AccPosition.y : player.motoControlsGUI.AccPosition.y),
                                player.motoControlsGUI.AccSize.x, 
                                player.motoControlsGUI.AccSize.y), 
                       player.motoControlsGUI.Accelerate))
        {
            player.motoController.isAccGUI = true;
        }
        else
        {
            player.motoController.isAccGUI = false;
        }

        if (GUI.RepeatButton(new Rect((player.motoControlsGUI.breakHAlign == MotoControlsGUI.BreakHAlign.Right ? Screen.width - player.motoControlsGUI.BreakPosition.x : player.motoControlsGUI.BreakPosition.x),
                                (player.motoControlsGUI.breakVAlign == MotoControlsGUI.BreakVAlign.Bottom ? Screen.height - player.motoControlsGUI.BreakPosition.y : player.motoControlsGUI.BreakPosition.y),
                                player.motoControlsGUI.BreakSize.x,
                                player.motoControlsGUI.BreakSize.y),
                       player.motoControlsGUI.Break))
        {
            player.motoController.isBreakGUI = true;
        }
        else
        {
            player.motoController.isBreakGUI = false;
        }
    }

    //Pause Menu
    void pauseUI()
    {
        //When game is pause show GUI 3D and if 3D is not available show in 2D
        if (player.motoController.isPaused && player.motoController.isAlive)
        {
            //set the skin and font size
            GUI.skin.label.fontSize = 35;
            GUI.Label(new Rect(player.motoController.pausePositionHUD.x, player.motoController.pausePositionHUD.y, 600, 200), "Paused!");

            GUI.skin.label.fontSize = 20;
            GUI.Label(new Rect(player.motoController.pausePositionHUD.x, player.motoController.pausePositionHUD.y + 40, 600, 200), "Press [P] or TAP pause button to unpause!");
        }
    }

    //Speedometer Digital part
    void speedoDigital()
    {
        Color oldcolor = GUI.color;
        GUI.color = Color.black;

        //Speed
        GUI.skin.label.fontSize = 14;
        GUI.Label(new Rect(player.motoSpeedoMeter.speedoDigitalSpeedPosition.x, 
                           player.motoSpeedoMeter.speedoDigitalSpeedPosition.y, 
                           200, 30), 
                           (player.motoController.playerCurrentSpeed).ToString());

        //Gear
        if (player.motoSettings.gearRatio.Length > 0)
        {
            GUI.skin.label.fontSize = 14;
            GUI.Label(new Rect(player.motoSpeedoMeter.speedoGearLabelPosition.x,
                               player.motoSpeedoMeter.speedoGearLabelPosition.y,
                               200, 30),
                               player.motoSettings.gearRatio[player.motoController.currentGearIndex].gearName);
        }
        

        GUI.color = oldcolor;
        GUI.skin.label.fontSize = 14;
        if (player.motoController.unitMetric == MotoController.UnitMetric.Imperial)
        {
            GUI.Label(new Rect(player.motoSpeedoMeter.speedoUnitMetricLabelPosition.x,
                           player.motoSpeedoMeter.speedoUnitMetricLabelPosition.y,
                           200, 30),
                           player.motoSpeedoMeter.UnitImperialText);
        }

        if (player.motoController.unitMetric == MotoController.UnitMetric.Metric)
        {
            GUI.Label(new Rect(player.motoSpeedoMeter.speedoUnitMetricLabelPosition.x,
                           player.motoSpeedoMeter.speedoUnitMetricLabelPosition.y,
                           200, 30),
                           player.motoSpeedoMeter.UnitMetricText);
        }
        

    }

    //Speedometer Analogue part
    void speedoMeterAnalog()
    {
        //This will draw speedoMeter Background
        if (player.motoSpeedoMeter.speedoDial != null)
        {
            //Draw speedometer bg
            GUI.DrawTexture(
                    new Rect(
                        player.motoSpeedoMeter.speedoDialPositionOffset.x,
                        player.motoSpeedoMeter.speedoDialPositionOffset.y,
                        player.motoSpeedoMeter.speedoDial.width * (player.motoSpeedoMeter.speedoDialScaleRatio),
                        player.motoSpeedoMeter.speedoDial.height * (player.motoSpeedoMeter.speedoDialScaleRatio)),
                    player.motoSpeedoMeter.speedoDial);
        }

        //Speed Needle
        if (player.motoSpeedoMeter.speedoNeedle != null)
        {
            Vector2 pivot = new Vector2(player.motoSpeedoMeter.speedoNeedlePositionOffset.x,
                                        player.motoSpeedoMeter.speedoNeedlePositionOffset.y);

            Matrix4x4 guiMatrix = GUI.matrix;

            float normalizedSpeed = (float)player.motoController.playerCurrentSpeed / player.motoSettings.MaxSpeed;
            float angle = Mathf.Lerp(player.motoSpeedoMeter.speedoNeedleStartAngle, player.motoSpeedoMeter.speedoNeedleEndAngle, normalizedSpeed);

            GUIUtility.RotateAroundPivot(angle, pivot);

            GUI.DrawTexture(new Rect(pivot.x,
                                     pivot.y - (player.motoSpeedoMeter.speedoNeedle.height * player.motoSpeedoMeter.speedoNeedleScaleRatio) / 2,
                                     player.motoSpeedoMeter.speedoNeedle.width * (player.motoSpeedoMeter.speedoNeedleScaleRatio),
                                     player.motoSpeedoMeter.speedoNeedle.height * (player.motoSpeedoMeter.speedoNeedleScaleRatio)),
                                     player.motoSpeedoMeter.speedoNeedle);

            GUI.matrix = guiMatrix;
        }

        //Draw the knot on the end corner of odo meter
        if (player.motoSpeedoMeter.speedoKnot != null)
        {
            //Speed knot
            GUI.DrawTexture(
                    new Rect(
                        player.motoSpeedoMeter.SpeedMeterKnotPositionOffset.x,
                        player.motoSpeedoMeter.SpeedMeterKnotPositionOffset.y,
                        player.motoSpeedoMeter.speedoKnot.width * (player.motoSpeedoMeter.SpeedMeterKnotScaleRatio),
                        player.motoSpeedoMeter.speedoKnot.height * (player.motoSpeedoMeter.SpeedMeterKnotScaleRatio)),
                    player.motoSpeedoMeter.speedoKnot);
        }
    }
}