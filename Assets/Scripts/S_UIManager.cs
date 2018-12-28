using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenericManagers;
using UnityEngine;

public class S_UIManager : Singleton<S_UIManager>
{
    /// <summary>
    ///     Pass in the ui name and turn that object on
    /// TODO: if UI already exist active it
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="uiName"></param>
    /// <param name="cursorOff"></param>
    /// <param name="playSound"></param>
    public void ShowUI(Canvas canvas ,string uiName, bool cursorOff, bool playSound)
    {
        var ui = Resources.Load<GameObject>("UI/" + uiName);

        if (ui == null)
        {
            PrintNotFound(uiName);
            return;
        }

        var theUi = GetUI(canvas, uiName);

        if (cursorOff)
        {
            StartCoroutine(TurnCursorOff(canvas, uiName));
        }

        if (playSound)
        {
            S_AudioManager.Instance.PlayClip("Test");
        }

        if (theUi == null)
        {
            var createdUi = Instantiate(ui, canvas.transform);
        }
        else
        {
            theUi.gameObject.SetActive(true);
        }
    }

    /// <summary>
    ///     Pass in a ui name and turn that object off
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="uiName"></param>
    /// <param name="playSound"></param>
    public void CloseUI(Canvas canvas, string uiName, bool playSound)
    {
        var theUi = GetUI(canvas, uiName);
        if (theUi == null)
        {
            PrintNotFound(uiName);
            return;
        }

        if (playSound)
        {
            S_AudioManager.Instance.PlayClip("Test");
        }

        theUi.gameObject.SetActive(false);
    }

    /// <summary>
    ///     Turns off all UI
    /// </summary>
    public void CloseAllUI(Canvas canvas)
    {
        foreach (Transform ui in canvas.transform)
        {
            CloseUI(canvas, ui.name, false);
        }
    }

    /// <summary>
    ///     Returns the UI yo uare looking for from the main canvas
    /// TODO: Fix error 
    /// </summary>
    /// <param name="uiName"></param>
    public Transform GetUI(Canvas canvas, string uiName)
    {
        foreach (var child in canvas.GetComponentsInChildren<Transform>())
        {
            if (child.name == uiName)
            {
                return child;
            }
        }

        return null;
    }

    /// <summary>
    ///    Returns all UI under a canvas
    /// </summary>
    public List<Transform> OutputAllUI(Canvas canvas)
    {
        return canvas.transform.GetComponentsInChildren<Transform>().ToList();
    }

    /// <summary>
    ///     
    /// </summary>
    public void DestroyAll()
    {
        foreach (var canvase in FindObjectsOfType<Canvas>())
        {
            foreach (var child in canvase.GetComponentsInChildren<Transform>())
            {
                Destroy(child);
            }
        }
    }

    /// <summary>
    ///     
    /// </summary>
    public void OptionsMenu()
    {

    }

    /// <summary>
    ///     While the ui is showing turn the cursor off
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    private IEnumerator TurnCursorOff(Canvas canvas, string uiName)
    {
        while (GetUI(canvas, uiName).gameObject.activeInHierarchy)
        {
            Cursor.visible = false;
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    ///     If ui not found print
    /// </summary>
    /// <param name="uiName"></param>
    private void PrintNotFound(string uiName)
    {
        Debug.Log("<color=yellow>[" + typeof(S_AudioManager) + "] " + uiName +
                  " was not found </color>");
    }

    private void OnDestroy()
    {
        DestroyAll();
    }
}