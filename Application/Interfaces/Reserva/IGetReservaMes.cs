using Domain.Common.Classes.Reserva.GetAll;
using Domain.Common.Classes.Shared;
using Domain.Common.Interfaces;

namespace Application.Interfaces.Reserva;

public interface IGetReservaMes : IHandler<Unit,ReservaGetAllResponse>;
