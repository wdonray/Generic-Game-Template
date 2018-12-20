using System.Collections;
using System.Collections.Generic;
using GenericManagers;
using UnityEngine;

public class S_UIManager : Singleton<S_UIManager>
{
    private Transform _mainCanvas;

    private void Awake()
    {
        _mainCanvas = GameObject.FindWithTag("MainCanvas").transform;
    }
    
    /// <summary>
    ///     
    /// </summary>
    /// <param name="uiName"></param>
    /// <param name="cursorOff"></param>
    /// <param name="playSound"></param>
    public void ShowUI(string uiName, bool cursorOff, bool playSound)
    {
        var ui = Resources.Load<GameObject>("UI/" + uiName);
        var theUi = GetUI(uiName);
        if (theUi == null)
        {
            var createdUi = Instantiate(ui, _mainCanvas);
        }
        else
        {
            theUi.SetActive(true);
        }
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="uiName"></param>
    /// <param name="cursorOff"></param>
    /// <param name="playSound"></param>
    public void CloseUI(string uiName, bool cursorOff, bool playSound)
    {

    }

    /// <summary>
    ///     
    /// </summary>
    public void CloseAllUI()
    {

    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="uiName"></param>
    public GameObject GetUI(string uiName)
    {
        foreach (var child in _mainCanvas.GetComponentsInChildren<GameObject>())
        {
            if (child.name == uiName)
            {
                return child;
            }
        }

        return null;
    }

    /// <summary>
    ///     
    /// </summary>
    public void OutputAllUI()
    {

    }

    /// <summary>
    ///     
    /// </summary>
    public void DestroyAll()
    {

    }

    /// <summary>
    ///     
    /// </summary>
    public void OptionsMenu()
    {

    }

    private void OnDestroy()
    {
        DestroyAll();
    }
}