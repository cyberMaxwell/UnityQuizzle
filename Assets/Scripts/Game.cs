using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject gamePanel;
    public GameObject mainPanel;
    public GameObject categoryPanel;
    public GameObject hintsPanel;

    public Text toastText;
    public Text livesText;
    public Text coinsText;
    public Text questionText;
    public Text timerLivesText;
    public Text inGameTimeText;

    public Text[] textCategory = new Text[3];
    public Text[] textAnswers = new Text[4];

    public Button[] hintsButtons = new Button[3];
    public Button[] answersButtons = new Button[4];

    public QuestionDatabase questionDatabase;

    List<string> categoryList;
    List<QuestionsData> allQuestionsForConcreteCategoryDataList;

    int lives;
    int coins;
    int randElement;
    float endTime;
    float inGameTime;
    bool canTiming;
    bool doubleChance;

    string currentCategory;

    float timeUntilLives = 20;
    bool afterPause = false;

    void Start()
    {

        coins = PlayerPrefs.GetInt("coinsCount", 10);
        coins = 50;//закомментить
        coinsText.text = coins.ToString();

        inGameTime = 15;
        canTiming = false;
        lives = PlayerPrefs.GetInt("livesCount", 3);
        //lives = 3;//закомментить
        livesText.text = lives.ToString();

        if (lives <= 0)// обновление таймера при старте приложения
        {
            timeUntilLives -= TimerMaster.instance.CheckDate();
            timerLivesText.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        LivesTimer();
        InGameTimer();
    }

    private void LivesTimer()
    {
        if (lives <= 0)//таймер для жизней
        {
            timeUntilLives -= Time.deltaTime;
            updateCountDownText();

            if (timeUntilLives <= 0)
            {
                timerLivesText.gameObject.SetActive(false);
                lives = 3;
                SetLives();
                timeUntilLives = 20;
            }
        }
    }

    private void updateCountDownText()
    {
        int minutes = (int)timeUntilLives / 60;
        int seconds = (int)timeUntilLives % 60;

        string timeLeftFormatted = string.Format("{0}:{1}", minutes, seconds);

        timerLivesText.text = timeLeftFormatted;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (afterPause && focus)
        {
            inGameTime = endTime - System.DateTime.Now.Second;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            endTime = inGameTime + System.DateTime.Now.Second;
            afterPause = true;
            TimerMaster.instance.SaveDate();
        }
        
    }


    private void OnApplicationQuit()
    {
        TimerMaster.instance.SaveDate();
    }

    private void GetCategories()
    {
        categoryList = questionDatabase.GetAllCategories();

        for (int i = 0; i < 3; i++)
        {
            int randomCategory = Random.Range(0, categoryList.Count);
            textCategory[i].text = categoryList[randomCategory];
            categoryList.RemoveAt(randomCategory);
        }
    }

    public void ResetClock()
    {
        TimerMaster.instance.SaveDate();
        timeUntilLives = 20;
        updateCountDownText();
        timeUntilLives -= TimerMaster.instance.CheckDate();
    }

    public void OnClickPlayButton()
    {
        if (lives > 0)
        {
            mainPanel.SetActive(false);
            GetCategories();
            categoryPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(showToast("Недостаточно жизней", 1));
        }

    }


    private void QuestionGenerate()
    {
        doubleChance = false;
        for (int i = 0; i < 4; i++)
        {
            answersButtons[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < 3; i++)
        {
            hintsButtons[i].gameObject.SetActive(true);
        }

        if (allQuestionsForConcreteCategoryDataList.Count > 0)
        {
            canTiming = true;
            inGameTime = 15;

            categoryPanel.SetActive(false);//скрываем все кнопки с категориями
            gamePanel.SetActive(true);//активируем игровую панель

            randElement = Random.Range(0, allQuestionsForConcreteCategoryDataList.Count);//случайный индекс вопроса

            questionText.text = allQuestionsForConcreteCategoryDataList[randElement].Question;//выводим вопрос на экран

            List<string> answers = new List<string>(allQuestionsForConcreteCategoryDataList[randElement].Answers);//лист с ответами для текущего вопроса

            for (int i = 0; i < 4; i++)
            {
                int rand = Random.Range(0, answers.Count);
                textAnswers[i].text = answers[rand];
                answers.RemoveAt(rand);
            }
        }
        else
        {
            gamePanel.SetActive(false);
            mainPanel.SetActive(true);
        }
    }

    public void OnClickCategoryButton(Text text)
    {
        currentCategory = text.text;//определяем, какая категория была выбрана

        allQuestionsForConcreteCategoryDataList = questionDatabase.GetAllElementsForConcreteCategory(currentCategory);// извлекаем вопросы для конкретной категории

        QuestionGenerate();
    }

    public void OnClickAnswersButtons(Text text)
    {
        if (doubleChance && text.text != allQuestionsForConcreteCategoryDataList[randElement].Answers[0])//если текст на кнопке не равен правильному ответу 
        {
            for (int i = 0; i < 4; i++)
            {
                if (answersButtons[i].GetComponentInChildren<Text>().text == text.text)//если текст на дочернем элементе равен тексту на кнопке
                {
                    answersButtons[i].gameObject.SetActive(false);//скрыть кнопку
                }
            }

            doubleChance = false;
        }
        else
        {
            if (text.text == allQuestionsForConcreteCategoryDataList[randElement].Answers[0])
            {
                StartCoroutine(TrueOrFalse(true));

            }
            else
            {
                StartCoroutine(TrueOrFalse(false));
            }
        }
    }



    private void ExitOnMainMenuOrQuestionGenerate()
    {
        if (lives <= 0)
        {
            canTiming = false;
            mainPanel.SetActive(true);
            gamePanel.SetActive(false);
            timerLivesText.gameObject.SetActive(true);
        }
        else
        {
            QuestionGenerate();
        }
    }

    void CheckOnZeroLives()
    {
        if (lives <= 0)
        {
            ResetClock();
        }
    }




    IEnumerator TrueOrFalse(bool check)
    {
        allQuestionsForConcreteCategoryDataList.RemoveAt(randElement);

        if (check)
        {
            inGameTime = 15;
            canTiming = false;
            //добавить анимацию для правильного ответа
            yield return new WaitForSeconds(1f);
            questionText.text = "верно";
            coins += 5;
            SetCoins();
            yield return new WaitForSeconds(0.5f);

            QuestionGenerate();
            yield break;
        }
        else
        {
            canTiming = false;
            inGameTime = 15;
            //добавить анимацию для неправильного ответа
            yield return new WaitForSeconds(1f);
            --lives;
            SetLives();

            questionText.text = "неверно";
            yield return new WaitForSeconds(0.5f);
            ExitOnMainMenuOrQuestionGenerate();
            yield break;
        }
    }
    //***************************************************************HINTS**********************************************************************

    public void OnClickFifthy()
    {
        if (coins - 10 < 0)
        {
            StartCoroutine(showToast("Недостаточно монет.", 1));
        }
        else
        {
            hintsButtons[0].gameObject.SetActive(false);
            int a = 0;
            coins -= 10;
            SetCoins();


            for (int i = 0; i < 4; i++)
            {
                if (textAnswers[i].text.ToString() != allQuestionsForConcreteCategoryDataList[randElement].Answers[0] && a < 2)
                {
                    answersButtons[i].gameObject.SetActive(false);
                    a++;
                }
            }
        }
    }

    public void OnClickDoubleX()
    {
        if (coins - 20 < 0)
        {
            StartCoroutine(showToast("Недостаточно монет.", 1));
        }
        else
        {
            hintsButtons[1].gameObject.SetActive(false);

            coins -= 20;
            SetCoins();
            doubleChance = true;
        }
    }

    public void OnClickChangeQ()
    {
        if (coins - 25 < 0)
        {
            StartCoroutine(showToast("Недостаточно монет.", 1));
        }
        else
        {
            //hintsButtons[2].gameObject.SetActive(false);
            allQuestionsForConcreteCategoryDataList.RemoveAt(randElement);

            coins -= 25;
            SetCoins();
            QuestionGenerate();
        }
    }

    //***************************************************************HINTS**********************************************************************




    //***************************************************************TOAST TEXT**********************************************************************
    private IEnumerator showToast(string text,
   int duration)
    {
        toastText.gameObject.SetActive(true);
        Color orginalColor = toastText.color;

        toastText.text = text;
        toastText.enabled = true;

        //Fade in
        yield return fadeInAndOut(toastText, true, 0.5f);

        //Wait for the duration
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        //Fade out
        yield return fadeInAndOut(toastText, false, 0.5f);

        toastText.enabled = false;
        toastText.color = orginalColor;
        toastText.gameObject.SetActive(false);
    }

    IEnumerator fadeInAndOut(Text targetText, bool fadeIn, float duration)
    {
        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0f;
            b = 1f;
        }
        else
        {
            a = 1f;
            b = 0f;
        }

        Color currentColor = Color.clear;
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            targetText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
    }
    //***************************************************************TOAST TEXT**********************************************************************

    private void InGameTimer()//таймер для уровней
    {

        if (inGameTime > 0 && canTiming)
        {
            inGameTime -= Time.deltaTime;
            inGameTimeText.text = System.Math.Round(inGameTime, 0).ToString();
        }
        else if (inGameTime <= 0)
        {
            inGameTime = 15;
            canTiming = false;
            StartCoroutine(TrueOrFalse(false));
        }
    }
    private void SetLives()
    {
        PlayerPrefs.SetInt("livesCount", lives);
        PlayerPrefs.Save();
        livesText.text = lives.ToString();
    }

    private void SetCoins()
    {
        PlayerPrefs.SetInt("coinsCount", coins);
        PlayerPrefs.Save();
        coinsText.text = coins.ToString();
    }
}
