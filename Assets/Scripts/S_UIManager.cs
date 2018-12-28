using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenericManagers;
using UnityEngine;

public class S_UIManager : Singleton<S_UIManager>
{
    /// <summary>
    ///     Pass in the ui name and turn that object on
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="uiName"></param>
    /// <param name="cursorOff"></param>
    /// <param name="playSound"></param>
    public void ShowUI(Canvas canvas, string uiName, bool cursorOff, bool playSound)
    {
        var ui = Resources.Load<GameObject>("UI/" + uiName);

        if (ui == null)
        {
            PrintUINotFound(uiName);
            return;
        }

        if (canvas == null)
        {
            PrintCanvasNotFound();
            return;
        }

        var theUi = GetUI(canvas, uiName);

        if (theUi == null)
        {
            var createdUi = Instantiate(ui, canvas.transform);
            var replace = createdUi.name.Replace("(Clone)", "");
            createdUi.name = replace;
            theUi = createdUi.transform;
        }
        else if (!theUi.gameObject.activeInHierarchy)
        {
            theUi.gameObject.SetActive(true);
        }

        if (playSound)
            S_AudioManager.Instance.PlayClip("Test");

        if (cursorOff)
            StartCoroutine(TurnCursorOff(canvas, theUi));
    }

    /// <summary>
    ///     Pass in a ui name and turn that object off
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="uiName"></param>
    /// <param name="playSound"></param>
    public void CloseUI(Canvas canvas, string uiName, bool playSound)
    {
        if (canvas == null)
        {
            PrintCanvasNotFound();
            return;
        }
        var theUi = GetUI(canvas, uiName);
        if (theUi == null)
        {
            PrintUINotFound(uiName);
            return;
        }

        if (playSound)
            S_AudioManager.Instance.PlayClip("Test");

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
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="uiName"></param>
    public Transform GetUI(Canvas canvas, string uiName)
    {
        Transform[] trs = canvas.GetComponentsInChildren<Transform>(true);
        foreach (var child in trs)
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
    ///     Destroys all UI Objects 
    /// </summary>
    public void DestroyAll()
    {
        foreach (var canvase in FindObjectsOfType<Canvas>())
        {
            foreach (var child in canvase.GetComponentsInChildren<Transform>())
            {
                Destroy(child.gameObject);
            }
        }
    }

    /// <summary>
    ///     While the ui is showing turn the cursor off
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="theUI"></param>
    /// <returns></returns>
    private IEnumerator TurnCursorOff(Canvas canvas, Transform theUI)
    {
        yield return new WaitUntil(() => theUI != null);
        do
        {
            Cursor.visible = false;
            yield return new WaitForEndOfFrame();
        } while (theUI != null && theUI.gameObject.activeInHierarchy);
        Cursor.visible = true;
    }

    /// <summary>
    ///     If ui not found print
    /// </summary>
    /// <param name="uiName"></param>
    private void PrintUINotFound(string uiName)
    {
        Debug.Log("<color=blue>[" + typeof(S_AudioManager) + "] " + uiName +
                  " was not found </color>");
    }

    /// <summary>
    ///     If canvas was null print
    /// </summary>
    private void PrintCanvasNotFound()
    {
        Debug.Log("<color=blue>[" + typeof(S_AudioManager) + "] Canvas was not found </color>");
    }

    private void OnDestroy()
    {
        DestroyAll();
    }
}