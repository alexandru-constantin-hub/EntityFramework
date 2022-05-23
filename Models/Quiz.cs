using System;
using System.Collections.Generic;

namespace EntityFramework.Models
{
    public partial class Quiz
    {
        public Quiz()
        {
            Answer = new HashSet<Answer>();
            QuestionQuiz = new HashSet<QuestionQuiz>();
        }

        public int QuizId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Answer> Answer { get; set; }
        public virtual ICollection<QuestionQuiz> QuestionQuiz { get; set; }
    }
}
