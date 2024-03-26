using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rm.BLL.Interfaces;
using Rm.DAL;
using Rm.DAL.Repositories.Interface;
using Rm.Model.Models;


namespace Rm.Api.Controllers
{
  
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentRepository Repository;
        private readonly IDocumentService Service;
        private readonly ICarService CarService;
        private readonly IWorkerService WorkerService;
        public DocumentController(IDocumentRepository repository, IDocumentService service, ICarService carService, IWorkerService workerService)
        {
            Repository = repository;
            Service = service;
            CarService = carService;
            WorkerService = workerService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Document document)
        {
            if (CarService.IsCarExistById(document.CarId) && WorkerService.IsWorkerExistById(document.WorkerId) && Service.IsDocumentExist(document.Id,true,document.CarId) == false)
            {
                await Repository.Create(document);
                return Ok("Document Created");
            }
            return Conflict("Invalid Car ,Worker or Car Already Exist");
           
        }

        [HttpGet]
        [Route("GetByDocId")]
        public DocumentResponse GetById(int id) 
        {
            var item = Repository.GetById(id);
           
            return item;
           
        }
        [HttpGet]
        [Route("GetByCarNumber")]
        public DocumentResponse GetByCarNumber(string number) 
        {
            var item = Repository.GetByCarNumber(number);
            return item;
        }
        [HttpGet]
        [Route("GetByWorkerNumber")]
        public DocumentResponse GetByWorkerNumber(string number) 
        {
            var item = Repository.GetByWorkerNumber(number);
            return item;
        }
        [HttpGet]
        [Route("DocsDownload")]
        public async Task<IActionResult> DownloadDocs(int docId)
        {
            if (Service.IsDocumentExist(docId))
            {
                try
                {
                    await Service.DownloadDocs(docId);
                    return Ok();
                }
                catch
                {
                    return BadRequest();
                }
            }
            return NotFound();
            
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int docId)
        {
            if (Service.IsDocumentExist(docId))
            {
                await Repository.Delete(docId);
                return Ok("Deleted");
            }
            return Conflict("Not Found");
        }
      
    }
}
