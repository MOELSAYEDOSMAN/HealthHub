namespace HealthHup.API.Service.BackGroundJobs.Dates
{
    public interface IBDataPeients
    {
        //Day
        public Task DeleteOldDates();
        //Mount
        public Task DeleteOldDisease();
        
    }
}
