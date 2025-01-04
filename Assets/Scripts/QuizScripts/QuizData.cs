using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Quiz", menuName = "Quiz/Quiz Data")]
public class QuizData : ScriptableObject
{
    public Question[] questions;
}


[System.Serializable]
public class Question
{
    public string questionText;
    public string[] answers;
    public int correctAnswerIndex;
    public string[] answerInfo;
}