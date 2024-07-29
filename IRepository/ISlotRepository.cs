using DoctorAppointment.Dto;
using DoctorAppointment.Models;

namespace DoctorAppointment.IRepository
{
    public interface ISlotRepository
    {
        Task<List<Slot>> CreateTimeSlot(CreateTimeSlot CreateTimeSlot);
        Task<(List<Slot>,int)> GetTimeMetaById(int metaId, int pageIndex, int pageSize);
        Task<SlotGetDto> SlotGetById(int slotId);
        Task<Slot> UpdateSlotById(int slotId,SlotUpdateDto Slot);
        Task<List<SlotGetDetailOnDate>> GetSlotDataOnDate(int metaId, DateTime? date);
    }
}
