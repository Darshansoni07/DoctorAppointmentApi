using AutoMapper;
using DoctorAppointment.Dbcontext;
using DoctorAppointment.IRepository;


namespace DoctorAppointment.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;


        public UnitOfWork(AppDbContext dbContext, IConfiguration configuration, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IAccountRepository AccountRepository => new AccountRepsitory(dbContext, configuration);

        public IAdminRepository AdminRepository => new AdminRepository(dbContext, AccountRepository, AssistantRepository);

        public IDoctorRepository DoctorRepository => new DoctorRepository(dbContext);
        public ISlotRepository SlotRepository => new SlotRepository(dbContext,mapper);
        public IAppointmentRepository AppointmentRepository => new AppointmentRepository(dbContext,ReportRepository);
        public IAssistantRepository AssistantRepository => new AssistantRepository(dbContext);
        public IReportRepository ReportRepository => new ReportRepository(dbContext,webHostEnvironment);
        public async Task<bool> SaveAsync()
        {
            return await dbContext.SaveChangesAsync() > 0;
        }
    }
}
