using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.iyzico
{
   public interface IIyzicoService
    {
        Task<IResult> PayWithIyzico(Iyzico iyzicoModel);
    }
}
