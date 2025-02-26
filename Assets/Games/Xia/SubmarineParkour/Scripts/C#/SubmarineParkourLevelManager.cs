using UnityEngine;
using System.Collections;

public class SubmarineParkourLevelManager : MonoBehaviour 
{
	int coins 	= 0;							//Collected coins

    static SubmarineParkourLevelManager myInstance;
    static int instances = 0;

    //Retursn the instance
    public static SubmarineParkourLevelManager Instance
    {
        get
        {
            if (myInstance == null)
                myInstance = FindObjectOfType(typeof(SubmarineParkourLevelManager)) as SubmarineParkourLevelManager;

            return myInstance;
        }
    }

	//Called at the start of the game
	void Start()
	{
        //Calibrates the myInstance static variable
        instances++;

        if (instances > 1)
            Debug.Log("Warning: There are more than one Level Manager at the level");
        else
            myInstance = this;

		SubmarineParkourSaveManager.CreateAndLoadData();		        //Create or load the saved stats
		SubmarineParkourGUIManager.Instance.UpdateBestDistance();		//Update best distance at the hangar
		SubmarineParkourGUIManager.Instance.SetLevelResolution();		//Set the level for the current resolution
        SubmarineParkourMissionManager.Instance.LoadStatus();			//Load mission status
	}
	//Called when the level is started
	public void StartLevel()
	{
        StartCoroutine(SubmarineParkourLevelGenerator.Instance.StartToGenerate(1.25f, 3));	//Start the level generator
        SubmarineParkourPlayerManager.Instance.ResetStatus(true);							//Reset player status, and move the submarine to the starting position
        SubmarineParkourGUIManager.Instance.ShowStartPowerUps();								//Show the power up activation GUI
        SubmarineParkourGUIManager.Instance.ActivateMainGUI();								//Activate main GUI
	}
	//Called when the game is paused
	public void PauseGame()
	{
        SubmarineParkourPlayerManager.Instance.DisableControls();				//Disable sub controls
        SubmarineParkourLevelGenerator.Instance.Pause();							//Pause the level generator
	}
	//Called then the game is resumed
	public void ResumeGame()
	{
        SubmarineParkourPlayerManager.Instance.EnableControls();					//Enable the sub controls
        SubmarineParkourLevelGenerator.Instance.Resume();						//Resume level generation
	}
	//Called when the player is reviving
	public void Revive()
	{
        StartCoroutine(SubmarineParkourPlayerManager.Instance.Revive());			//Revive the player
	}
	//Called when a coin has been collected
	public void CoinGathered()
	{
		coins++;										//Increase coin number
        SubmarineParkourMissionManager.Instance.CoinEvent(coins);				//Notify the mission manager
	}
    //Returns the number of collected coins
    public int Coins()
    {
        return coins;
    }
	//Called when the level is restarting
	public void Restart()
	{
		coins = 0;										//Reset coin numbers

        SubmarineParkourLevelGenerator.Instance.Restart(true);					//Restart level generator
        SubmarineParkourPlayerManager.Instance.ResetStatus(true);				//Reset player status
        SubmarineParkourMissionManager.Instance.Save();							//Save mission status

        SubmarineParkourGUIManager.Instance.ShowStartPowerUps();					//Show the power up activation GUI
        SubmarineParkourGUIManager.Instance.ActivateMainGUI();					//Activate main GUI
        SubmarineParkourGUIManager.Instance.UpdateBestDistance();				//Update best distance at the hangar
	}
	//Called when quiting to the main menu from the level
	public void QuitToMain()
	{
        SubmarineParkourLevelGenerator.Instance.Restart(false);				//Disable level generator
        SubmarineParkourPlayerManager.Instance.ResetStatus(false);			//Reset player status
        SubmarineParkourMissionManager.Instance.Save();						//Save progress

        SubmarineParkourGUIManager.Instance.DeactivateMainGUI();				//Deactivate the main GUI
        SubmarineParkourGUIManager.Instance.ActivateMainMenu();				//Activate main menu
        SubmarineParkourGUIManager.Instance.UpdateBestDistance();			//Update best distance at the hangar
	}
}
