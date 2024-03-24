using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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
    public GameObject headerTextObject;
    private TextMeshProUGUI _headerTextComponent;

    public GameObject instructionTextObject;
    private TextMeshProUGUI _instructionTextComponent;

    private int _currentTaskIndex = 0;
    private int _currentTaskStep = 0;

    private List<Task> _taskList;
    
    private void Start()
    {
        _taskList = InitTaskList();

        _headerTextComponent = headerTextObject.GetComponent<TextMeshProUGUI>();
        _instructionTextComponent = instructionTextObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        SetKeyBoardListener();
        SetTaskText();
    }
    
    private static List<Task> InitTaskList()
    {
        var list = new List<Task>
        {
            new("Task 1", "Instruction for task 1"),
            new("Task 2", "Instruction for task 2"),
            new("Task 3", "Instruction for task 3"),
            new("Task 4", "Instruction for task 4")
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

    private void SetCurrentTaskIndex(int index, bool resetStep = true)
    {
        if (resetStep) _currentTaskStep = 0;

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
}
