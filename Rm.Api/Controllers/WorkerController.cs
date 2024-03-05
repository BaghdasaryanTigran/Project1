using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rm.BLL.Interfaces;
using Rm.DAL;
using Rm.Models;

namespace Rm.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkerController : ControllerBase
    {

        IBaseRepository<Worker> Repository;
        IWorkerService Service;
        IDocumentService DocumentService;
        public WorkerController(IBaseRepository<Worker> repository, IWorkerService service , IDocumentService documentService)
        {
            Repository = repository;
            Service = service;
            DocumentService = documentService;
        }

        [HttpGet]
        public Worker Get(int id)
        {
            if (Service.IsWorkerExistById(id))
            {
                return Repository.GetById(id);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Post(Worker worker)
        {
            if (Service.IsWorkerExist(worker))
            {
                return Conflict("Worker already exist");
            }
            Repository.Create(worker);
            return Ok("Worker Added");
        }

        [HttpPut]
        public IActionResult Put(Worker worker)
        {
            if (Service.IsWorkerExist(worker))
            {
                Repository.Update(worker);
                return Ok("Updated");
            }
            return Conflict("Worker Not Found");
        }
        [Authorize]
        [HttpDelete]
        public IActionResult Delete(Worker worker)
        {
            if (Service.IsWorkerExist(worker) && DocumentService.IsDocumentExistByWorkerId(worker.Id) == false)
            {
                Repository.Delete(worker);
                return Ok("Deleted");
            }
            return Conflict("Worker not found or in progress");
        }
    }
}
