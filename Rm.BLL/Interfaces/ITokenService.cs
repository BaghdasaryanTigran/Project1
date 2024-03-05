using Rm.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rm.BLL.Interfaces
{
    public interface ITokenService
    {
        public string TokenGenerator(string userLogin);
        public List<string> TokenDecode(string token);
    }
}
