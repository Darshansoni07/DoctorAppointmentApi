using AutoMapper;
using DoctorAppointment.Dbcontext;
using DoctorAppointment.Dto;
using DoctorAppointment.IRepository;
using DoctorAppointment.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DoctorAppointment.Repository
{
    public class SlotRepository : ISlotRepository
    {
        private readonly TimeZoneInfo IndianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;
        public SlotRepository(AppDbContext dbContext, IMapper mapper)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public async Task<List<Slot>> CreateTimeSlot(CreateTimeSlot CreateTimeSlot)
        {

            List<Slot> slots = new List<Slot>();
            try
            {
                if (CreateTimeSlot != null)
                {
                    bool timeSlotsExist = dbContext.DS_slots
                        .Any(ts => ts.MetadataDoc_meta_Id == CreateTimeSlot.MetadataDoc_meta_Id &&
                          ts.StartTimeslot >= CreateTimeSlot.StartTimeslot &&
                           ts.EndTimeslot <= CreateTimeSlot.EndTimeslot);

                    if (!timeSlotsExist)
                    {
                        DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianTimeZone);

                        for (DateTime time = CreateTimeSlot.StartTimeslot; time < CreateTimeSlot.EndTimeslot; time += CreateTimeSlot.interval)
                        {
                            if (time > currentTime)
                            {
                                slots.Add(new Slot
                                {
                                    StartTimeslot = time,
                                    EndTimeslot = time + CreateTimeSlot.interval,
                                    Status = "Available",
                                    CreatedOn = currentTime,
                                    MetadataDoc_meta_Id = CreateTimeSlot.MetadataDoc_meta_Id
                                });
                            }
                        }                        
                    }
                    dbContext.DS_slots.AddRange(slots);
                    dbContext.SaveChanges();
                    return slots;
                }
                else
                {
                    throw new Exception("Data is not comming");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get Slot On selected Date Dateails 
        /// </summary>
        /// <param name="metaId"></param>
        /// <param name="date"></param>
        /// <returns> SlotGetDetailDto value </returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<SlotGetDetailOnDate>> GetSlotDataOnDate(int metaId, DateTime? date)
        {
            DateTime dateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianTimeZone);
            try
            {
                if (dateTime.Date == date.Value.Date)
                {
                    var slotOnDate = await dbContext.DS_slots
                            .Where(ts => ts.MetadataDoc_meta_Id == metaId
                            && (ts.StartTimeslot.Date == date.Value.Date && ts.StartTimeslot.TimeOfDay > dateTime.TimeOfDay))
                    .OrderBy(ts => ts.StartTimeslot)
                    .ToListAsync();
                    var slotGetDetailsOnDate = mapper.Map<List<SlotGetDetailOnDate>>(slotOnDate);

                    return slotGetDetailsOnDate;
                }
                else
                {
                    var slotOnDate = await dbContext.DS_slots
                            .Where(ts => ts.MetadataDoc_meta_Id == metaId
                            && (ts.StartTimeslot.Date == date.Value.Date ))
                    .OrderBy(ts => ts.StartTimeslot)
                    .ToListAsync();
                    var slotGetDetailsOnDate = mapper.Map<List<SlotGetDetailOnDate>>(slotOnDate);

                    return slotGetDetailsOnDate;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(List<Slot>,int)> GetTimeMetaById(int metaId, int pageIndex = 0, int pageSize = 5)
        {
            DateTime dateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianTimeZone);
            try
            {
                if(metaId != 0)
                {
                    var data= await dbContext.DS_slots
                        .Where(ts => ts.MetadataDoc_meta_Id == metaId
                        && (ts.StartTimeslot.Date > dateTime.Date
                        || (ts.StartTimeslot.Date == dateTime.Date && ts.StartTimeslot.TimeOfDay > dateTime.TimeOfDay)))
                        .OrderBy(ts => ts.StartTimeslot)
                        .Skip(pageIndex * pageSize)
                         .Take(pageSize)
                        .ToListAsync();
                    var totalData = await (from slot in dbContext.DS_slots
                                           where slot.MetadataDoc_meta_Id == metaId
                                           && (slot.StartTimeslot.Date > dateTime.Date
                                           || slot.StartTimeslot.Date == dateTime.Date && slot.StartTimeslot.TimeOfDay > dateTime.TimeOfDay)
                                           orderby slot.StartTimeslot
                                           select (slot)
                                           ).CountAsync();
                    return (data, totalData);
                                           
                }
                else
                {
                    return (null,0);
                }
            }
            catch {
                throw new Exception("Not Implement");
            }

            
        }

        public async Task<SlotGetDto> SlotGetById(int slotId)
        {
            if (slotId != 0)
            {
                return await dbContext.DS_slots
                    .Where(doc => doc.SlotId == slotId)
                    .Select(doc => new SlotGetDto
                    {
                        SlotId = doc.SlotId,
                        StartTimeslot = doc.StartTimeslot,
                        EndTimeslot = doc.EndTimeslot,
                        Status = doc.Status,
                        MetadataDoc_meta_Id = doc.MetadataDoc_meta_Id
                    }).FirstOrDefaultAsync();
            }
            else
            {
                return null;
            }
        }

        public async Task<Slot> UpdateSlotById(int slotId,SlotUpdateDto slot)
        {
            if(slotId == 0)
            {
                return null;
            }
            else
            {
                var slotDetail = await dbContext.DS_slots.FindAsync(slotId);
                if(slotDetail != null)
                {
                    slotDetail.StartTimeslot = slot.StartTimeslot;
                    slotDetail.EndTimeslot = slot.EndTimeslot;
                    slotDetail.Status = slot.Status;
                    slotDetail.UpdatedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianTimeZone);
                    dbContext.DS_slots.Update(slotDetail);
                    await dbContext.SaveChangesAsync();
                    
                }
                return slotDetail;
            } 

        }
    }
}
