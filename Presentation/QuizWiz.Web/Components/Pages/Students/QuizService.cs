using QuizWiz.Application.SharedModel;

namespace QuizWiz.Web.Components.Pages.Students
{
    public class QuizService
    {
        public int Score { get; private set; }
        public int TotalQuestions { get; private set; }
        public int QuestionsAnswered { get; private set; }

        public void InitializeQuiz(List<Quiz> questions)
        {
            Score = 0;
            TotalQuestions = questions.Count;
            QuestionsAnswered = 0;
        }

        public void AnswerQuestion(bool isCorrect)
        {
            if (isCorrect)
            {
                Score++;
            }
            QuestionsAnswered++;
        }
    }

    public class QuizStateService
    {
        public List<Quiz> Questions { get; private set; }

        public void SetQuestions(List<Quiz> questions)
        {
            Questions = questions;
        }
    }
}
