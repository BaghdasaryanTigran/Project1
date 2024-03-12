using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rm.DAL.Context;
using Rm.DAL.Repositories.Interface;
using Rm.Model.Models;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.IO.Util.IntHashtable;

namespace Rm.DAL.Repositories
{
    public class DocumentRepository :IDocumentRepository
    {
        private readonly RmContext Context;
        private readonly MapperConfiguration Configuration = new MapperConfiguration(cfg =>
        cfg.CreateMap<Document, DocumentResponse>()
       );
        public DocumentRepository(RmContext context)
        {
            Context = context;
        }


        public DocumentResponse GetByCarNumber(string number)
        {
            var mapper = new Mapper(Configuration);
            var doc = Context.Set<Document>().Include(x => x.Car).Include(x => x.Worker).FirstOrDefault(x => x.Car.Number == number);
            var result = mapper.Map<DocumentResponse>(doc);
            return result;
        }
        public DocumentResponse GetByWorkerNumber(string number)
        {
            var mapper = new Mapper(Configuration);
            var doc = Context.Set<Document>().Include(x => x.Car).Include(x => x.Worker).FirstOrDefault(x => x.Worker.PhoneNumber == number);
            var result = mapper.Map<DocumentResponse>(doc);
            return result;
        }
        public DocumentResponse GetById(int id)
        {
            var mapper = new Mapper(Configuration);
            var doc = Context.Set<Document>().Include(x => x.Car).Include(x => x.Worker).FirstOrDefault(x => x.Id == id);
            var result = mapper.Map<DocumentResponse>(doc);
            return result;
        }

        public List<DocumentResponse> GetAll()
        {
            var mapper = new Mapper(Configuration);
           var docs = Context.Documents.ToList();
            List<DocumentResponse>response = mapper.Map<List<DocumentResponse>>(docs);
            return response;
        }

        public async Task Create(Document doc)
        {
            Context.Documents.Add(doc);
            await Context.SaveChangesAsync();
           
        }

        public async Task Update(int docId, int carId, int workerId)
        {
            var doc = Context.Documents.FirstOrDefault(x => x.Id == docId);
            doc.WorkerId = workerId;
            doc.CarId = carId;
            Context.Update(doc);
            await Context.SaveChangesAsync();
        }

        public async Task Delete(int docId)
        {
            var toDelete = Context.Documents.FirstOrDefault(x => x.Id == docId);
            Context.Documents.Remove(toDelete);
            await Context.SaveChangesAsync();
        }
    }
}
