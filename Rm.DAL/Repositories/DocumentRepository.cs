
using Rm.DAL.Context;
using Rm.DAL.Repositories.Interface;
using Rm.Model.Models;
using Rm.Models;

namespace Rm.DAL.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly RmContext Context;
     
        public DocumentRepository(RmContext context)
        {
            Context = context;
        }

        public void Create(Document document)
        {
           
            Context.Set<Document>().Add(document);
            Context.SaveChanges();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

      

        public Tuple<Car, Worker> Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
