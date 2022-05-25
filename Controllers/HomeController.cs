using EntityFramework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Controllers
{
    
    public class HomeController : Controller
    {

        public QuizExamenContext db = new QuizExamenContext();
        public static List<int> howManyQuestions;
        public static List<string> questionsInCategory;
        public static List<int> counter;
        public static int personId;
        


        
        public IActionResult Index()
        {
            ViewBag.howmanyQuestionsEasy = db.Question.Where(c => c.CategoryId == 1).Count();
            ViewBag.howmanyQuestionsMedium = db.Question.Where(c => c.CategoryId == 2).Count();
            ViewBag.howmanyQuestionsHard = db.Question.Where(c => c.CategoryId == 3).Count();
            

            return View(db.Quiz);
        }
        

        public IActionResult CreateQuiz(int easy, int medium, int hard, int quizId)
        {

            List<int> questions = new List<int>();
            
            
            

            Random easyrandom = new Random();
            Random mediumrandom = new Random();
            Random hardrandom = new Random();
            List<int> easyCatList= db.Question.Where(c => c.CategoryId == 1).Select(c=>c.QuestionId).ToList();
            List<int> mediumCatList = db.Question.Where(c => c.CategoryId == 2).Select(c => c.QuestionId).ToList();
            List<int> hardCatList = db.Question.Where(c => c.CategoryId == 3).Select(c => c.QuestionId).ToList();
            
            if(easy !=0)
            {
                for (int i = 0; i < easy; i++)
                {
                    //int number = easyrandom.Next(easy-1);
                    questions.Add(easyCatList[i]);
                }
            }

            if (medium != 0)
            {
                for (int i = 0; i < medium; i++)
                {
                    //int number = mediumrandom.Next(medium);
                    questions.Add(mediumCatList[i]);
                }
            }

            if (hard != 0)
            {
                for (int i = 0; i < hard; i++)
                {
                    //int number = hardrandom.Next(hard);
                    questions.Add(hardCatList[i]);
                }
            }


            if (db.QuestionQuiz.Where(a => a.QuizId == quizId).Count() != 0)
            {
               
                return View(db.Question.Where(c => questions.Contains(1000000000)).ToList());
                
            } else
            {

                QuestionQuiz q = new QuestionQuiz();
                foreach (var item in questions)
                {
                    
                    q.QuestionId = item;
                    q.QuizId = quizId;
                    db.QuestionQuiz.Add(q);
                    db.SaveChanges();
                    Debug.WriteLine("Question number:" + item);
                }

                Debug.WriteLine("Id:" + quizId);
                ViewData["answers"] = db.ItemOption.ToList();
                personId = quizId;


                return View(db.Question.Where(c => questions.Contains(c.QuestionId)).ToList());
            }

           
        }
        


        public IActionResult Score(IFormCollection form)
        {
            Debug.WriteLine("Answer: " + form["question"]);
            string value = form["question"];
            string[] result = value.Split(',');
            
            foreach (string res in result)
            {
                //Debug.WriteLine("Raspuns: " + res);
                Answer a = new Answer();
                a.OptionId = Int32.Parse(res);
                a.QuizId = personId;
                db.Answer.Add(a);
                db.SaveChanges();
            }



            return View(db.ItemOption.Include(q => q.Question).Where(c => result.Contains(c.OptionId.ToString())).ToList());
        }
        






        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
