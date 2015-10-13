using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendingMachine.BE
{
    public class MoneyCache
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public double Nominal { get; set; }
        public int Count { get; set; }
    }
}
