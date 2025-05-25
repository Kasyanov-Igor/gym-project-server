using gym_project_business_logic.Model.Domains;
using System.Collections.Generic;

namespace gym_project_business_logic.Model
{
    public class Gym
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty!;

        public ICollection<Workout>? Workouts { get; set; }
    }
}
