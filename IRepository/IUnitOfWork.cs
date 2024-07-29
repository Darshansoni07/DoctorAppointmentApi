namespace DoctorAppointment.IRepository
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
        IAdminRepository AdminRepository { get; }
        IDoctorRepository DoctorRepository { get; }
        ISlotRepository SlotRepository { get; }
        IAppointmentRepository AppointmentRepository { get; }
        IAssistantRepository AssistantRepository { get; }
        IReportRepository ReportRepository { get; }
        Task<bool> SaveAsync();
    }
}
