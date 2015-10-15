using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendingMachine.BE
{
    [Table("user_wallet")]
    public class UserWallet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public decimal Nominal { get; set; }
        public int Count { get; set; }
    }
}
