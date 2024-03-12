using AutoMapper;
using iText.Html2pdf;
using Microsoft.EntityFrameworkCore;
using Rm.BLL.Interfaces;
using Rm.DAL.Context;
using Rm.Model.Models;


namespace Rm.BLL
{
    public class DocumentService : IDocumentService
    {
        private readonly RmContext Context;
        
        public DocumentService(RmContext context)
        {
            Context = context;
        }
        
        

        
        public bool IsDocumentExist(int docId, bool withCarId = false, int carId = -1)
        {
            var count = 0;
            if (withCarId = true)
            {
                count = Context.Set<Document>().Count(x => x.Id == docId || x.CarId == carId);
            }
            else
            {
                count = Context.Set<Document>().Count(x => x.Id == docId);
            }


            if (count > 0)
            {
                return true;
            }
            return false;
        }
        public bool IsDocumentExistByCarId(int carId)
        {
            var count = Context.Set<Document>().Count(x => x.CarId == carId);

            if (count > 0)
            {
                return true;
            }
            return false;
        }
        public bool IsDocumentExistByWorkerId(int WorkerId)
        {
            var count = Context.Set<Document>().Count(x => x.CarId == WorkerId);

            if (count > 0)
            {
                return true;
            }
            return false;
        }
        public async Task DownloadDocs(int docId)
        {
            Document doc = Context.Documents.Include(x => x.Car).Include(x => x.Worker).FirstOrDefault(x => x.Id == docId);
            if (doc == null)
            {
                throw new ArgumentNullException();
            }
            string fileName = $"Doc{docId}.pdf";
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string path = Path.Combine(userPath, fileName);
            var html = $@"
              <html>
                <body>
                     <h1>Repair Document : {docId}</h1>
                     <p>Car Name: {doc.Car.Name}</p>
                     <p>Car Number: {doc.Car.Number}</p>
                     <p>Worker Name: {doc.Worker.Name}</p>
                     <p>Worker Phone Number: {doc.Worker.PhoneNumber}</p>
                    <p>Worker Position: {doc.Worker.Position}</p>
                </body>
              </html>";

            await Task.Run(() =>
            {
                using (FileStream pdfFile = new FileStream(path, FileMode.Create))
                {
                    HtmlConverter.ConvertToPdf(html, pdfFile);
                }
            });


        }
    }
}
