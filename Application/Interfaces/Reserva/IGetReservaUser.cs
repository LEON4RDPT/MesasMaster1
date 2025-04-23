using Domain.Common.Classes.Reserva.Get;
using Domain.Common.Classes.Reserva.GetAll;
using Domain.Common.Interfaces;

namespace Application.Interfaces.Reserva;

public interface IGetReservaUser : IHandler<ReservaGetRequest, ReservaGetAllResponse>;