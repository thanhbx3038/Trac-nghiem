using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using TracNghiemOnline.Models;
using TracNghiemOnline.Common;
using Novacode;
using System.Security.Principal;
using System.Web.UI.WebControls;


namespace TracNghiemOnline.Common
{
    public class WordExportHelper:Controller
    {
        trac_nghiem_onlineEntities db = new trac_nghiem_onlineEntities();
        private string _serverPath;
        private IPrincipal _user;
        AdminDA Model = new AdminDA();

        public string ServerPath
        {
            get { return _serverPath; }
            set { _serverPath = value; }
        }
        public IPrincipal AppUser
        {
            get { return _user; }
            set { _user = value; }
        }
        public FileResult XuatBaiThi(int test_code)
        {
            MemoryStream stream = new MemoryStream();
            string server = ServerPath;
           // List<StudentQuestViewModel> list = Model.GetListQuest(test_code);
            //lấy danh sách câu hỏi
            
                List<quests_of_test> qs = (from x in db.quests_of_test
                                           where x.test_code == test_code
                                           select x).OrderBy(x => Guid.NewGuid()).ToList();
               
 
            test baithi = Model.GetTest(test_code);
            DocX doc = DocX.Create(stream);
            string fileTemplate = "";
            fileTemplate = server + "//mau_baithi.docx";
            doc = DocX.Load(fileTemplate);
            doc.ReplaceText("%tenbaithi%", baithi.test_name);
            int stt = 1;
           foreach (var item in qs)
                {
                question q = db.questions.SingleOrDefault(x => x.id_question == item.id_question);
                string[] answer = { q.answer_a, q.answer_b, q.answer_c, q.answer_d };
                answer = ShuffleArray.Randomize(answer);
              
                doc.ReplaceText("%cauhoi_"+ stt.ToString()+ "%", item.question.content);
                doc.ReplaceText("%dapan_"+ stt.ToString() +"_a%", answer[0]);
                doc.ReplaceText("%dapan_" + stt.ToString() + "_b%", answer[1]);
                doc.ReplaceText("%dapan_" + stt.ToString() + "_c%", answer[2]);
                doc.ReplaceText("%dapan_" + stt.ToString() + "_d%", answer[3]);
                stt++;
            }
            doc.SaveAs(stream);
            return File(stream.ToArray(), "application/octet-stream", "baithi.docx");
        }
        public FileResult XuatBienBan(string tengiaovien, int thisinh, int test_code)
        {

            MemoryStream stream = new MemoryStream();
            
            string server = ServerPath;
            // List<StudentQuestViewModel> list = Model.GetListQuest(test_code);
            //lấy điểm thí sinh theo bài thi
            var  score= Model.GetScore(test_code, thisinh);
            //List<quests_of_test> qs = (from x in db.quests_of_test
             //                          where x.test_code == test_code
            //                           select x).OrderBy(x => Guid.NewGuid()).ToList();


            //test baithi = Model.GetTest(test_code);
            DocX doc = DocX.Create(stream);
          
            string fileTemplate = "";
            fileTemplate = server + "//mau_bienban.docx";
            doc = DocX.Load(fileTemplate);
            DateTime currenttime = DateTime.Now;

            doc.ReplaceText("%ngay%", currenttime.Day.ToString());
            doc.ReplaceText("%thang%", currenttime.Month.ToString());
            doc.ReplaceText("%nam%", currenttime.Year.ToString());
            doc.ReplaceText("%tenthisinh%", score.student.name);
            doc.ReplaceText("%tendangnhap%", score.student.username);
            doc.ReplaceText("%socaudung%", score.score.detail.Trim());
            doc.ReplaceText("%diemthi%",Convert.ToString( score.score.score_number));
            doc.ReplaceText("%diemtacphong%", Convert.ToString(score.score.diem_tac_phong));
            double diemtong = Convert.ToDouble(score.score.score_number + score.score.diem_tac_phong);
            doc.ReplaceText("%tongdiem%", Convert.ToString(diemtong));
            doc.SaveAs(stream);
           
           
         
            return File(stream.ToArray(), "application/octet-stream", "bienban_"+ score.student.username.ToString()+".docx");
        }
    }
}