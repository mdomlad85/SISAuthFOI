using System.IO;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Test()
        {
            return View();
        }
        public FileContentResult Document()
        {
            string filePath = Server.MapPath("~/App_Data/SIS Auth.docx");
            FileInfo file = new FileInfo(filePath);

            if (!file.Exists)
            {
                using (StreamWriter writer = file.CreateText())
                {
                    writer.WriteLine("Hello, I am a new text file");

                }
            }

            return File(
                System.IO.File.ReadAllBytes(file.FullName), 
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
                file.Name);
        }
    }
}
