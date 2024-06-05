using SAPLink.Core.Models.System;

namespace SAPLink.API.Services
{
    // RecurrenceService.cs
    public class RecurrenceService
    {
        private static UnitOfWork _unitOfWork;

        public RecurrenceService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task AddAsync(RecurrenceDto dto)
        {
            var recurrence = new Recurrence
            {
                Interval = dto.Interval,
                Recurring = dto.Recurring,
                Document = dto.Document,
                DayOfWeek = dto.DayOfWeek
            };

            await _unitOfWork.Recurrences.AddAsync(recurrence);
        }

        public async Task<List<Recurrence>> GetAllAsync()
        {
            var recurrence = await _unitOfWork.Recurrences.GetAllAsync();
            return recurrence.ToList();
        }

        public async Task<Recurrence> GetByIdAsync(int id)
        {
            var recurrence = await _unitOfWork.Recurrences.GetByIdAsync((int)id);
            return recurrence;
        }

        public void Update(Recurrence recurrence)
        {
            var req = _unitOfWork.Recurrences.FindAsync(x => x.Id == recurrence.Id);

            if (req != null && req.Id == recurrence.Id)
            {
                _unitOfWork.Recurrences.Update(recurrence);
                _unitOfWork.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var recurrence = _unitOfWork.Recurrences.GetById(id.ToString());
            if (recurrence != null)
            {
                _unitOfWork.Recurrences.Delete(recurrence);
                _unitOfWork.SaveChanges();
            }
        }
    }

}
