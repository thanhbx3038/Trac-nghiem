using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TracNghiemOnline.Common;
namespace TracNghiemOnline.Models
{
    public class TeacherDA
    {
        User user = new User();
        trac_nghiem_onlineEntities db = new trac_nghiem_onlineEntities();

        public void UpdateLastLogin()
        {
            var update = (from x in db.teachers where x.id_teacher == user.ID select x).Single();
            update.last_login = DateTime.Now;
            db.SaveChanges();
        }
        public void UpdateLastSeen(string name, string url)
        {
            var update = (from x in db.teachers where x.id_teacher == user.ID select x).Single();
            update.last_seen = name;
            update.last_seen_url = url;
            db.SaveChanges();
        }
        public List<TestViewModel> GetListTest()
        {
            List<TestViewModel> tests = (from x in db.tests
                                         join s in db.subjects on x.id_subject equals s.id_subject
                                         join stt in db.statuses on x.id_status equals stt.id_status
                                         select new TestViewModel { test = x, subject = s, status = stt }).ToList();
            return tests;
        }
        public List<ScoreViewModel> GetListScore(int test_code)
        {
            List<ScoreViewModel> score = new List<ScoreViewModel>();
            try
            {
                score = (from x in db.scores
                         join s in db.students on x.id_student equals s.id_student
                         where x.test_code == test_code select new ScoreViewModel { score = x, student = s }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return score;
        }
        //Phần xem câu hỏi của giáo viên
        public List<QuestionViewModel> GetQuestions()
        {
            List<QuestionViewModel> questions = (from x in db.questions
                                                 join s in db.subjects on x.id_subject equals s.id_subject
                                                 select new QuestionViewModel { question = x, subject = s }).ToList();
            return questions;
        }
        public bool AddQuestion(int id_subject, string unit, string content, string img_content, string answer_a, string answer_b, string answer_c, string answer_d, string correct_answer)
        {
            var question = new question();
            question.id_subject = id_subject;
            question.unit = unit;
            question.content = content;
            question.img_content = img_content;
            question.answer_a = answer_a;
            question.answer_b = answer_b;
            question.answer_c = answer_c;
            question.answer_d = answer_d;
            question.correct_answer = correct_answer;
            try
            {
                db.questions.Add(question);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        public bool DeleteQuestion(int id)
        {
            try
            {
                var delete = (from x in db.questions where x.id_question == id select x).Single();
                db.questions.Remove(delete);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        public question GetQuestion(int id)
        {
            question question = new question();
            try
            {
                question = db.questions.SingleOrDefault(x => x.id_question == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return question;
        }
        public bool EditQuestion(int id_question, int id_subject, string unit, string content, string img_content, string answer_a, string answer_b, string answer_c, string answer_d, string correct_answer)
        {
            try
            {
                var update = (from x in db.questions where x.id_question == id_question select x).Single();
                update.id_subject = id_subject;
                update.unit = unit;
                update.content = content;
                update.img_content = img_content;
                update.answer_a = answer_a;
                update.answer_b = answer_b;
                update.answer_c = answer_c;
                update.answer_d = answer_d;
                update.correct_answer = correct_answer;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        public List<subject> GetSubjects()
        {
            return db.subjects.ToList();
        }
    }
}