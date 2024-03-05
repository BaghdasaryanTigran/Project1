using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rm.BLL.Interfaces;
using Rm.DAL;
using Rm.Model.Models;

namespace Rm.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly IBaseRepository<Car> Repository;
        private readonly ICarService Service;
        private readonly IDocumentService DocumentService;
        public CarController(IBaseRepository<Car> repository, ICarService service, IDocumentService documentService)
        {
            Repository = repository;
            Service = service;
            DocumentService = documentService;
        }

        [HttpGet]
        public Car Get(int carId)
        {
            if (Service.IsCarExistById(carId))
            {
                return Repository.GetById(carId);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Post(Car car)
        {
            if (Service.IsCarExist(car))
            {
                return Conflict("Car already exist");
            }
            Repository.Create(car);
            return Ok("Car Added");
        }

        [HttpPut]
        public IActionResult Put(Car car)
        {
            if (Service.IsCarExistById(car.Id))
            {
                Repository.Update(car);
                return Ok("Updated");
            }
            return Conflict("Car Not Found");
        }

        [Authorize]
        [HttpDelete]
        public IActionResult Delete(Car car)
        {
            if (Service.IsCarExist(car) && DocumentService.IsDocumentExistByCarId(car.Id) == false)
            {
                Repository.Delete(car);
                return Ok("Deleted");
            }
            return Conflict("Car not found or in progress");
        }
    }
}
