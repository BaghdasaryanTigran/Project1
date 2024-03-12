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
        public async Task<Car> Get(int carId)
        {
            if (Service.IsCarExistById(carId))
            {
                return await Repository.GetById(carId);
            }
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Car car)
        {
            if (Service.IsCarExist(car))
            {
                return Conflict("Car already exist");
            }
            await Repository.Create(car);
            return Ok("Car Added");
        }

        [HttpPut]
        public async Task<IActionResult> Update(Car car)
        {
            if (Service.IsCarExistById(car.Id))
            {
                await Repository.Update(car);
                return Ok("Updated");
            }
            return Conflict("Car Not Found");
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(Car car)
        {
            if (Service.IsCarExist(car) && DocumentService.IsDocumentExistByCarId(car.Id) == false)
            {
              await  Repository.Delete(car);
                return Ok("Deleted");
            }
            return Conflict("Car not found or in progress");
        }
    }
}
