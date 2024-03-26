using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Oculus.Interaction;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

internal class Task
{
    public readonly string Header;
    public readonly string Instruction;

    public Task(string header, string instruction)
    {
        Header = header;
        Instruction = instruction;
    }
}

public class TaskController : MonoBehaviour
{
    // Text display
    public GameObject headerTextObject;
    private TextMeshProUGUI _headerTextComponent;

    public GameObject instructionTextObject;
    private TextMeshProUGUI _instructionTextComponent;
    
    // Task Prefabs
    public GameObject taskOnePrefab;
    private GameObject _taskOneObject;

    public GameObject taskTwoPrefab;
    private GameObject _taskTwoObject;

    public GameObject taskThreePrefab;
    private GameObject _taskThreeObject;

    public GameObject taskFourPrefab;
    private GameObject _taskFourObject;

    public GameObject cylinder;
    public GameObject canvas;
    
    // parent node for mounting task prefabs
    private Transform _panelObject;
    
    private int _currentTaskIndex = 0;
    private int _previousTaskIndex = -1;
    private int _currentTaskStep = 0;
    private int _previousTaskStep = -1;

    private List<Task> _taskList;
    
    private void Start()
    {
        _taskList = InitTaskList();
        _headerTextComponent = headerTextObject.GetComponent<TextMeshProUGUI>();
        _instructionTextComponent = instructionTextObject.GetComponent<TextMeshProUGUI>();
        _panelObject = headerTextObject.transform.parent;
    }

    private void Update()
    {
        SetKeyBoardListener();

        if (_previousTaskIndex == _currentTaskIndex)
        {
            if (_previousTaskStep == _currentTaskStep) return;
            HandleTasks();
            _previousTaskStep = _currentTaskStep;
        }
        else
        {
            _currentTaskStep = 0;
            SetTaskText();
            HandleTasks();
            _previousTaskIndex = _currentTaskIndex;
        }
    }
    
    private static List<Task> InitTaskList()
    {
        var list = new List<Task>
        {
            new("Task 1: Large Blocks", "Please activate the yellow block."),
            new("Task 2: Small Blocks", "Please activate the yellow block."),
            new("Task 3: Slider", "Please adjust the slider to about 75%"),
            new("Task 4: Browsing" , "This is a document. Please position the yellow paragraph to the top area of the screen"),
            new("Task 5: Enlarge the screen", "Enlarge the screen twice as large as now and then reduce it to original size"),
            new("Task 6: Move the screen", "Move the screen 50% closer and then move it back to its original position")
        };
        return list;
    }

    private void SetKeyBoardListener()
    {
        // next task
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            SetCurrentTaskIndex(_currentTaskIndex + 1);

        // previous task
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            SetCurrentTaskIndex(_currentTaskIndex - 1);

        // restart task
        else if (Input.GetKeyDown(KeyCode.R))
            _currentTaskStep = 0;

        // next step
        // note: the step number have no limit, the logic of how to deal with overflow steps should be decided on each task.
        // mouseDown 0 -> mouse left button
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            _currentTaskStep += 1;

        // this should always at the end
        else if (Input.anyKey)
        {
            var isNumber = int.TryParse(Input.inputString, out var index);

            // only 1-9, enough for our experiment, input start from 1 not 0
            if (isNumber && index is > 0 and < 10)
                SetCurrentTaskIndex(index - 1);
        }
    }

    private void SetCurrentTaskIndex(int index)
    {
        if (index > _taskList.Count - 1)
            _currentTaskIndex = _taskList.Count - 1;
        else if (index < 0)
            _currentTaskIndex = 0;
        else _currentTaskIndex = index;
    }
    
    private void SetTaskText()
    {
        _headerTextComponent.text = _taskList[_currentTaskIndex].Header;
        _instructionTextComponent.text = _taskList[_currentTaskIndex].Instruction;
    }

    private void HandleTasks()
    {
        ResetTasks();
        switch (_currentTaskIndex)
        {
            case 0:
                HandleTaskOne();
                break;
            case 1:
                HandleTaskTwo();
                break;
            case 2:
                HandleTaskThree();
                break;
            case 3:
                HandleTaskFour();
                break;
            case 4:
                HandleTaskFive();
                break;
            case 5:
                HandleTaskSix();
                break;
        }
    }

    private void HandleTaskOne()
    {
        if (_taskOneObject is not null) Destroy(_taskOneObject);
        _taskOneObject = Instantiate(taskOnePrefab, _panelObject);

        var setColor = new Action<int, Color>((i, color) =>
        {
            _taskOneObject.transform.GetChild(i).GetComponent<Image>().color = color;
        });

        switch (_currentTaskStep % 3)
        {
            // step 1 - initial state
            case 0:
                setColor(0, Color.white);
                setColor(1, Color.white);
                break;
            // step 2 - to activate left button
            case 1:
                setColor(0, new Color(1, 0.2f, 0.2f));
                setColor(1, Color.white);
                break;
            // step 3 - to activate right button
            case 2:
                setColor(1, new Color(1, 0.2f, 0.2f));
                setColor(0, Color.white);
                break;
        }
    }
    
    private void HandleTaskTwo()
    {
        if (_taskTwoObject is not null) Destroy(_taskTwoObject);
        _taskTwoObject = Instantiate(taskTwoPrefab, _panelObject);
        
        var setColor = new Action<int, Color>((i, color) =>
        {
            _taskTwoObject.transform.GetChild(i).GetComponent<Image>().color = color;
        });
        
        switch (_currentTaskStep % 3)
        {
            // step 1 - initial state
            case 0:
                setColor(0, Color.white);
                setColor(1, Color.white);
                break;
            // step 2 - to activate left button
            case 1:
                setColor(0, new Color(1, 0.2f, 0.2f));
                setColor(1, Color.white);
                break;
            // step 3 - to activate right button
            case 2:
                setColor(0, Color.white);
                setColor(1, new Color(1, 0.2f, 0.2f));
                break;
        }
    }
    private void HandleTaskThree()
    {
        if (_taskThreeObject is not null) Destroy(_taskThreeObject);
        _taskThreeObject = Instantiate(taskThreePrefab, _panelObject);
        
        var setValue = new Action<float>(value =>
        {
            if (_taskThreeObject is not null)
                _taskThreeObject.transform.GetChild(0).GetComponent<Slider>().value = value;
        });

        switch (_currentTaskStep % 2)
        {
            case 0:
                setValue(0);
                break;
            case 1:
                setValue(0.75f);
                break;
        }
    }
    
    private void HandleTaskFour()
    {
        if (_taskFourObject is not null) Destroy(_taskFourObject);
        _taskFourObject = Instantiate(taskFourPrefab, _panelObject);
        
        // not working, delete the feedback for now
        //
        // var originalText = _taskFourObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        // var lastParagraph = originalText.Split("\r\n").Last();
        //
        // var sizeDelta = _taskFourObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
        // var originalX = sizeDelta.x;
        // var originalY = sizeDelta.y;
        //
        // switch (_currentTaskStep % 2)
        // {
        //     // step 1 - initial state
        //     case 0:
        //         _taskFourObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = originalText;
        //         _taskFourObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(originalX, originalY);
        //         break;
        //     case 1:
        //         _taskFourObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = lastParagraph;
        //         _taskFourObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(originalX, 50);
        //         break;
        // }
    }

    private void HandleTaskFive()
    {
        // todo hardcode here
        const int originalWidth = 1600;
        const int originalHeight = 1000;
        switch (_currentTaskStep % 2)
        {
            case 0:
                canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(originalWidth, originalHeight);
                break;
            case 1:
                canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(originalWidth * 1.41f, originalHeight * 1.41f);
                break;
        }

    }

    private void HandleTaskSix()
    {
        // todo hardcode
        var originalCanvasPosition = new Vector3(0, 1.2f, 5);
        
        switch (_currentTaskStep % 2)
        {
            case 0:
                cylinder.transform.GetChild(0).GetComponent<Transform>().position = originalCanvasPosition;
                break;
            case 1:
                cylinder.transform.GetChild(0).GetComponent<Transform>().position = new Vector3(originalCanvasPosition.x, originalCanvasPosition.y, 3.5f);
                break;
        }
    }
    
    private void ResetTasks()
    {
        if (_taskOneObject is not null) Destroy(_taskOneObject);
        if (_taskTwoObject is not null) Destroy(_taskTwoObject);
        if (_taskThreeObject is not null) Destroy(_taskThreeObject);
        if (_taskFourObject is not null) Destroy(_taskFourObject);
    }
}
