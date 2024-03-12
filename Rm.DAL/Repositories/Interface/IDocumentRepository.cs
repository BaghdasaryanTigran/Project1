using Rm.Model.Models;
using Rm.Models;


namespace Rm.DAL.Repositories.Interface
{
    public interface IDocumentRepository
    {
        public DocumentResponse GetById(int docId);
        public DocumentResponse GetByCarNumber(string number);
        public DocumentResponse GetByWorkerNumber(string number);
        public List<DocumentResponse> GetAll();
        public Task Create(Document doc);
        public Task Update(int docId, int carId, int workerId);
        public Task Delete(int docId);
    }
}
