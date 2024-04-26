namespace DACS_DAMH.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class Discount
    {
        [Key]

        public int IdDiscount { get; set; }

        public string Code { get; set; }

        public double Percentage { get; set; }

        public DateTime Expdate { get; set; }

        public int Remain { get; set; }
    }
}
