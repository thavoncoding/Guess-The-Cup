using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class CupSystemManager : MonoBehaviour
{
    [SerializeField] private Cup[] cups;
    [SerializeField] private Transform[] visualOfCups;
    [SerializeField] private Transform pingPongBall;
    [SerializeField] private Transform[] cupPositions;
    [SerializeField] private TMP_InputField speed;
    [SerializeField] private TMP_InputField round;
    [SerializeField] private GameObject allButtons;
    [SerializeField] private GameObject guessButtons;
    [SerializeField] private GameObject tryAgainButton;
    [SerializeField] private GameObject correctGuess;
    [SerializeField] private GameObject wrongGuess;
    [SerializeField] private Vector3[] cupsStartPositions;

    float revealOrHidePingPongBallDuration = 1f;
    private Vector3 correctCupPosition;

    private void Start()
    {
        // Record position of cups;
        for (int i = 0; i < cups.Length; i++)
        {
            cupsStartPositions[i] = cups[i].transform.position;
        }
    }

    public void OnStartGame()
    {
        allButtons.gameObject.SetActive(false);

        int integerRound = int.Parse(round.text);
        float moveSpeed = float.Parse(speed.text);

        float moveDuration = 1f / moveSpeed;

        StartCoroutine(RandomMoveCups(integerRound, moveDuration));
    }

    public void OnButtonTryAgain()
    {
        //RecordPositionOfCups();
        tryAgainButton.SetActive(false);
        correctGuess.SetActive(false);
        wrongGuess.SetActive(false);
        allButtons.SetActive(true);
    }

    public void OnButtonCup1()
    {
        StartCoroutine(CheckGuess(cupsStartPositions[0]));
    }

    public void OnButtonCup2()
    {
        StartCoroutine(CheckGuess(cupsStartPositions[1]));
    }

    public void OnButtonCup3()
    {
        StartCoroutine(CheckGuess(cupsStartPositions[2]));
    }

    private IEnumerator CheckGuess(Vector3 guessedPosition)
    {
        guessButtons.SetActive(false);
        // Call to Reveal Ping Pong Ball;
        RevealPingPongBall();
        yield return new WaitForSeconds(revealOrHidePingPongBallDuration);
        //Debug.Log("Guess Position: " + guessedPosition);
        if (guessedPosition == correctCupPosition)
        {
            Debug.Log("Correct guess!");
            correctGuess.SetActive(true);
        }
        else
        {
            Debug.Log("Wrong guess. Try again!");
            wrongGuess.SetActive(true);
        }
        tryAgainButton.SetActive(true);
    }

    private IEnumerator RandomMoveCups(int rounds, float moveSpeed)
    {
        // Call to Reveal Ping Pong Ball;
        RevealPingPongBall();
        yield return new WaitForSeconds(revealOrHidePingPongBallDuration);

        // Call to Reveal Ping Pong Ball;
        HidePingPongBall();
        yield return new WaitForSeconds(revealOrHidePingPongBallDuration);


        for (int i = 0; i < rounds; i++)
        {
            ShufflePositions();

            for (int j = 0; j < cups.Length; j++)
            {
                cups[j].transform.DOMove(cupPositions[j].position, moveSpeed);
            }

            // Wait for the move to complete before starting the next round
            yield return new WaitForSeconds(moveSpeed);
        }

        // Update the correct cup index based on the final position of the cups
        for (int i = 0; i < cups.Length; i++)
        {
            if (cups[i].isTheCupThatHavePingPongBall)
            {
                correctCupPosition = cups[i].transform.position;
                break;
            }
        }

        //Debug.Log("Correct position: " + correctCupPosition);

        // All rounds completed
        guessButtons.SetActive(true);
    }

    private void RevealPingPongBall()
    {
        // Move Y of all cups to 4.44 to reveal the ping pong ball
        foreach (var visualOfCup in visualOfCups)
        {
            visualOfCup.transform.DOMoveY(4.44f, revealOrHidePingPongBallDuration);
        }
    }

    private void HidePingPongBall()
    {
        // Move Y of all cups to 2.3 to hide the ping pong ball
        foreach (var visualOfCup in visualOfCups)
        {
            visualOfCup.transform.DOMoveY(2.3f, revealOrHidePingPongBallDuration);
        }
    }

    // Fisher-Yates shuffle algorithm to shuffle the positions
    private void ShufflePositions()
    {
        for (int i = cupPositions.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Vector3 temp = cupPositions[i].position;
            cupPositions[i].position = cupPositions[randomIndex].position;
            cupPositions[randomIndex].position = temp;
        }
    }
}
