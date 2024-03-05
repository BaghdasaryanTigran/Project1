using Rm.Model.Models;
using Rm.Models;


namespace Rm.DAL.Repositories.Interface
{
    public interface IDocumentRepository
    {
        
      
        public void Create(Document doc);
        public Tuple<Car, Worker> Update(User entity);
        public void Delete();
    }
}
