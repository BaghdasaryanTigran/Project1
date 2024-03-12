
using Rm.Model.Models;


namespace Rm.BLL.Interfaces
{
    public interface IDocumentService
    {
        public bool IsDocumentExist(int docId,bool withCar = false,int carId = 0);
        public bool IsDocumentExistByCarId(int carId);
        public bool IsDocumentExistByWorkerId(int workerId);
        public Task DownloadDocs(int docId);

    }
}
