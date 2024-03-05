using Rm.Model.Models;


namespace Rm.BLL.Interfaces
{
    public interface ICarService
    {
        public bool IsCarExistById(int id);
        public bool IsCarExist(Car car);
    }
}
