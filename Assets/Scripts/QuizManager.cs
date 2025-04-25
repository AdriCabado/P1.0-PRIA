using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class TriviaQuestion {
    public string category;
    public string type;
    public string difficulty;
    public string question;
    public string correct_answer;
    public List<string> incorrect_answers;
}

[Serializable]
public class TriviaResponse {
    public int response_code;
    public List<TriviaQuestion> results;
}

public class QuizManager : MonoBehaviour {
    [Header("UI References")]
    public TMP_Text questionText;
    public TMP_Text feedbackText;
    public Button trueButton;
    public Button falseButton;
    public Button nextButton;
    public Button restartButton;

    private List<TriviaQuestion> questions;
    private int currentQuestionIndex = 0;
    private int score = 0;

    void Start() {
        feedbackText.text = string.Empty;
        nextButton.interactable = false;
        restartButton.gameObject.SetActive(false);

        trueButton.onClick.AddListener(() => OnAnswer("True"));
        falseButton.onClick.AddListener(() => OnAnswer("False"));
        nextButton.onClick.AddListener(OnNextQuestion);
        restartButton.onClick.AddListener(OnRestart);

        StartCoroutine(FetchQuestions());
    }

    IEnumerator FetchQuestions() {
        string url = "https://opentdb.com/api.php?amount=10&category=15&type=boolean";
        using (UnityWebRequest request = UnityWebRequest.Get(url)) {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success) {
                Debug.LogError($"Error fetching questions: {request.error}");
            } else {
                TriviaResponse response = JsonUtility.FromJson<TriviaResponse>(request.downloadHandler.text);
                questions = response.results;
                DisplayQuestion();
            }
        }
    }

    void DisplayQuestion() {
        if (currentQuestionIndex < questions.Count) {
            string q = System.Net.WebUtility.HtmlDecode(questions[currentQuestionIndex].question);
            questionText.text = q;
            feedbackText.text = string.Empty;
            nextButton.interactable = false;
            trueButton.interactable = true;
            falseButton.interactable = true;
            Debug.Log(q); 
        } else {
            questionText.text = "Quiz ended!";
            feedbackText.text = $"Your score: {score}/{questions.Count}";
            Debug.Log($"Quiz ended with score: {score}/{questions.Count}");
            trueButton.gameObject.SetActive(false);
            falseButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(true);
        }
    }

    void OnAnswer(string playerAnswer) {
        TriviaQuestion current = questions[currentQuestionIndex];
        trueButton.interactable = false;
        falseButton.interactable = false;

        if (playerAnswer == current.correct_answer) {
             Debug.Log("Player Answer: " + playerAnswer);
            feedbackText.text = "Correct!";
            score++;
            Debug.Log("The Answer was Correct");
        } else {
            Debug.Log("Player Answer: " + playerAnswer);
            feedbackText.text = "Wrong!";
            Debug.Log("The Answer was Wrong");
        }

        nextButton.interactable = true;
    }

    void OnNextQuestion() {
        currentQuestionIndex++;
        DisplayQuestion();
    }

    void OnRestart() {
        currentQuestionIndex = 0;
        score = 0;
        trueButton.gameObject.SetActive(true);
        falseButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false); 
        StartCoroutine(FetchQuestions());
    }
}