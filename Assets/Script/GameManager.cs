using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{

    public static PaperForm CurrentForm; // Variável para armazenar a forma atual
    public static PaperForm PreviousForm; // Variável para armazenar a forma atual
    public static bool isPicking;
    public static bool isPaused;
    private static bool[] formasDesbloqueadas = new bool[5] { true, true, true, true, true }; // Inicialmente, apenas Human e Ball estão desbloqueadas
    private static Dictionary<string, bool> doorStates = new Dictionary<string, bool>();
    public enum PaperForm
    {
        HumanForm = 0, // Modificado para começar de 0
        CircleForm = 1,
        AirPlaneForm = 2,
        BoatForm = 3,
        PopUpForm = 4,
    }
    public static bool IsFormUnlocked(PaperForm form)
    {
        return formasDesbloqueadas[(int)form];
    }

    public static void UnlockForm(PaperForm form)
    {
        formasDesbloqueadas[(int)form] = true;
    }
    public static void OpenDoorCutscene(string cutScene, string changeScene)
    {
        Debug.Log("Door is opened");
        SetDoorState(changeScene, true);
    }
    public static void SetDoorState(string doorName, bool isOpen)
    {
        doorStates[doorName] = isOpen;
    }
    public static bool GetDoorState(string doorName)
    {
        if (doorStates.ContainsKey(doorName))
        {
            return doorStates[doorName];
        }
        return false;
    }

}
