using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Database/Questions", fileName = "Questions")]
public class QuestionDatabase : ScriptableObject
{
    [SerializeField, HideInInspector] List<QuestionsData> questionsList;

    [SerializeField] private QuestionsData currentQuestion;

    private List<string> categories;

    private int currentIndex = 0;


    public void AddElement()
    {
        if (questionsList == null) questionsList = new List<QuestionsData>();

        currentQuestion = new QuestionsData();
        questionsList.Add(currentQuestion);
        currentIndex = questionsList.Count - 1;
    }

    public void RemoveCurrentElement()
    {
        if (currentIndex > 0)
        {
            currentQuestion = questionsList[--currentIndex];
            questionsList.RemoveAt(++currentIndex);
        }
        else
        {
            questionsList.Clear();
            currentQuestion = null;
        }

    }

    public QuestionsData GetNext()
    {
        if (currentIndex < questionsList.Count - 1)
            currentIndex++;
        currentQuestion = this[currentIndex];
        return currentQuestion;
    }

    public QuestionsData GetPrev()
    {
        if (currentIndex > 0)
            currentIndex--;
        currentQuestion = this[currentIndex];
        return currentQuestion;
    }

    public QuestionsData GetConcrete(int index)
    {
        if (index >= 0 && index <= questionsList.Count)
        {
            currentIndex = index;
            currentQuestion = this[currentIndex];
        }
        return currentQuestion;
    }

    public void ClearDatabase()
    {
        questionsList.Clear();
        questionsList.Add(new QuestionsData());
        currentQuestion = questionsList[0];
        currentIndex = 0;
    }

    public QuestionsData GetRandomElement()
    {
        int random = Random.Range(0, questionsList.Count);
        return questionsList[random];
    }

    public List<string> GetAllCategories()
    {
        string noRepeatCategory = "";
        categories = new List<string>();
        for(int i = 0; i < questionsList.Count; i++)
        {
            if (!noRepeatCategory.Contains(questionsList[i].Category))
            {
                categories.Add(questionsList[i].Category);
                noRepeatCategory += questionsList[i].Category;
            }
            else continue;
        }

        return categories;
    }

    public List<QuestionsData> GetAllElementsForConcreteCategory(string category)
    {
        List<QuestionsData> list = new List<QuestionsData>();
        for (int i = 0; i < questionsList.Count; i++)
        {
            if(questionsList[i].Category == category)
            {
                list.Add(questionsList[i]);
            }
        }

        return list;
    }

    public QuestionsData this[int index]
    {
        get
        {
            if (questionsList != null && index >= 0 && index < questionsList.Count)
                return questionsList[index];
            return null;
        }

        set
        {
            if (questionsList == null)
                questionsList = new List<QuestionsData>();

            if (index >= 0 && index < questionsList.Count && value != null)
                questionsList[index] = value;
            else Debug.LogError("Выход за границы массива, либо переданное значение = null!");
        }
    }

}

[System.Serializable]
public class QuestionsData
{
     [Tooltip("Категория")]
     [SerializeField] private string category;
     public string Category
     {
         get
         {
             return category;
         }
         protected set { }
     }

    [Tooltip("Вопрос")]
    [SerializeField] private string question;
    public string Question
    {
        get
        {
            return question;
        }
        protected set { }
    }

    [Tooltip("Ответы")]
    [SerializeField] private string[] answers = new string[4];
    public string[] Answers
    {
        get
        {
            return answers;
        }
        protected set { }
    }

}
