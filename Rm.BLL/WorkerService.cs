using Rm.BLL.Interfaces;
using Rm.DAL.Context;
using Rm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rm.BLL
{
    public class WorkerService : IWorkerService
    {
        private readonly RmContext Context;
        public WorkerService(RmContext context)
        {
            Context = context;
        }
        public bool IsWorkerExist(Worker worker)
        {
            var count = Context.Set<Worker>().Count(x => x.PhoneNumber == worker.PhoneNumber || x.Id == worker.Id);

            if(count > 0) 
            {
                return true;
            }
            return false;

        }
        public bool IsWorkerExistById(int Id)
        {
            var count = Context.Set<Worker>().Count(x => x.Id == Id);

            if (count > 0)
            {
                return true;
            }
            return false;

        }
    }
}
