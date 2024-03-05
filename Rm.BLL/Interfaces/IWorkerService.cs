using Rm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rm.BLL.Interfaces
{
    public interface IWorkerService
    {
        public bool IsWorkerExistById(int Id);
        public bool IsWorkerExist(Worker worker);
    }

}
