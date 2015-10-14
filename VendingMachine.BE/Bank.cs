using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendingMachine.BE
{
    [Table("bank")]
    public class Bank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public decimal Nominal { get; set; }
        public int Count { get; set; }
    }
}
