using System;

namespace gym_project_business_logic.Model.Domains
{
    public class DTOWorkout
    {
        public string Title { get; set; } = null!; // Название тренировки

        public string NameCoach { get; set; } = null!; // Имя тренера

        public int GymId { get; set; }

        public int Places { get; set; }

        public string? ClientName {  get; set; }

        public string? Description { get; set; } // Описание тренировки

        public int DurationMinutes { get; set; } // Предполагаемая продолжительность

        public DateTime BookingTime { get; set; } // Время создания записи

        public int CoachId { get; set; } // Внешний ключ к тренеру, который создал эту тренировку
    }
}
