using DoctorAppointment.Dto;
using DoctorAppointment.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointment.Controllers
{
    public class SlotController : BaseController
    {
        private readonly IUnitOfWork uow;

        public SlotController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpPost("createSlot")]
        public async Task<IActionResult> CreateSlot(CreateTimeSlot createTimeSlot)
        {
            if (createTimeSlot == null)
            {
                return BadRequest();
            }
            else
            {
                var create = await uow.SlotRepository.CreateTimeSlot(createTimeSlot);
                if (create.Count == 0)
                {
                    return Ok("Not Create At This Time Sechdule");
                }
            }
            return Ok("Create Time Slot");
        }


        [HttpGet("getSolt/{metaId}")]
        public async Task<IActionResult> GetSlots(int metaId, int pageIndex=0, int pageSize=5)
        {
            if(metaId == 0)
            {
                return NotFound();
            }
            else
            {
                var data = await uow.SlotRepository.GetTimeMetaById(metaId, pageIndex, pageSize);                
                var response = new {
                    data = data.Item1,
                    totalData = data.Item2
                };
                return Ok(response);
            }
        }


        [HttpGet("getSlotById/{slotId}")]
        public async Task<IActionResult> GetSlotsById(int slotId)
        {
            if(slotId == 0)
            {
                return Ok("Not Found Id");
            }
            else
            {
                var data = await uow.SlotRepository.SlotGetById(slotId);
                if (data == null)
                {
                    return Ok("No Data Available");
                }
                return Ok(data);
            }
        }


        [HttpPut("updateSlotById/{slotId}")]
        public async Task<IActionResult> UpdateSlotById(int slotId, SlotUpdateDto slotUpdateDto)
        {
            if (slotId == 0)
            {
                return Ok("Id Not Found");
            }
            else
            {
                var data = await uow.SlotRepository.UpdateSlotById(slotId, slotUpdateDto);
                if(data == null)
                {
                    return Ok("No Data Avilable");
                }
                return Ok("Slot Updated");
            }
        }


        [HttpGet("getSlotDataOnDate/{metaId}")]
        public async Task<IActionResult> GetSlotDataOnDate(int metaId, DateTime? date)
        {
            try
            {
                var data = await uow.SlotRepository.GetSlotDataOnDate(metaId,date);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }            
        }

    }
}