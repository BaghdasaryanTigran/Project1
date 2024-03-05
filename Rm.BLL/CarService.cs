using Rm.BLL.Interfaces;
using Rm.DAL.Context;
using Rm.Model.Models;


namespace Rm.BLL
{
    public class CarService : ICarService
    {
        private readonly RmContext Context;
        public CarService(RmContext context) 
        {
            Context = context;
        }

        public bool IsCarExist(Car car)
        {
            int count = Context.Set<Car>().Count(x =>  x.Number == car.Number && x.Name == car.Name|| x.Id == car.Id );
           

            if (count > 0)
            {
                return true; 
            }
            return false;
        }
        public bool IsCarExistById(int Id)
        {
            int count = Context.Set<Car>().Count(x => x.Id == Id);

            if (count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
