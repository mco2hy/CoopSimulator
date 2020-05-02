using System;

namespace CoopSimulator.Data
{
    public class PoultryDto
    {
        public DateTime BirthDate { get; set; }
        public Enums.PoultrySex Sex { get; set; }
        public bool Pregnant { get; set; }
        public DateTime? PregnantDate { get; set; }
    }
}
