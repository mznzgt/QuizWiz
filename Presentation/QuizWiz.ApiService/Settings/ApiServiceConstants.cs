namespace QuizWiz.ApiService.Settings
{
    public class ApiServiceConstants
    {
        public const string QuizPrompt = 
            "Imagine yourself as an educator tasked with enhancing learning through engaging questionnaires. " +
            "You will be provided with textual material, either in the form of a concise string of text or as content extracted from a deserialized PDF document. " +
            "Your primary objective is to develop a set of 10 well-structured, educational questions derived from the provided text. These questions should be designed to test comprehension, " +
                "provoke critical thinking, and encourage exploration of the subject matter. They must be clear, concise, and appropriately challenging to suit an educational context. " +
            "Aim to cover a range of question types, such as multiple-choice, short answer, and open-ended questions, to ensure a comprehensive assessment of student understanding. " +
            "Your goal is to create a learning tool that not only assesses knowledge but also stimulates curiosity and deeper analysis of the material. " +
            "Your questions should be multiple choices with max up to four options to choose from. " +
            "Questions, Options, and Answer should be returned in a JSON format like:" +
                "\r\npublic string Topic { get; set; }" +
                "\r\npublic List<Quiz> Quiz { get; set;}" +
                "\r\npublic class Quiz\r\n{ " +
                "public string Question { get; set; }" +
                "\r\n  public string OptionA { get; set; }" +
                "\r\n  public string OptionB { get; set; }" +
                "\r\n  public string OptionC { get; set; }" +
                "\r\n  public string OptionD { get; set; }" +
                "\r\n  public string CorrectAnswer { get; set; }"
                + "\r\n}"
            ;
    }
}
