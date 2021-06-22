using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[AddComponentMenu ("Noir Project/Simple Infinite MotoBiker/Main Menu")]
public class Mainmenu : MonoBehaviour {

    public enum MenuType
    {
        Mainmenu,
        Loading,
        GameOver
    }

    public MenuType menuType = MenuType.Mainmenu;

    public TextMesh gameOverScoreTextMesh = null;
    public TextMesh gameOverHighScoreTextMesh = null;

    IEnumerator waitFakeLoad()
    {
        //Wait 3 seconds and then load level
        yield return new WaitForSeconds(3.0f);
        sceneLoader("level");
        yield return null;
    }

    //This will load level from string name
    void sceneLoader(string lvlName)
    {
        SceneManager.LoadScene(lvlName);
    }

    void Start()
    {
        //When it's loading screen start fake load waiting
        if (menuType == MenuType.Loading)
            StartCoroutine("waitFakeLoad");

        MotoSavegame savegame = new MotoSavegame();
        if (gameOverScoreTextMesh != null)
        {
            //show score + prefix text
            gameOverScoreTextMesh.text = savegame.prefixScore + savegame.loadScore().ToString();
        }
        if (gameOverHighScoreTextMesh != null)
        {
            //show score + prefix text
            gameOverHighScoreTextMesh.text = savegame.prefixHighScore + savegame.loadHighScore().ToString();
        }
    }

	// Update is called once per frame
	void Update () 
    {
        //When it's not loading screen and user press space or mouse click or tap screen
		if (menuType == MenuType.Mainmenu)
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                sceneLoader("loadScreen");
            }
        }

        if (menuType == MenuType.GameOver)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                sceneLoader("MainMenu");
            }
        }
	}

    void OnGUI()
    {
        if (menuType == MenuType.GameOver)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height - 200, 100, 50), "Restart Game"))
            {
                sceneLoader("MainMenu");
            }
        }
    }
}
