using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class Depositor
    {
        [Key]
        public int Id { get; set; }
        public int DepositId { get; set; }
        public string DepositName { get; set; }
        public string DepositorName { get; set; }
        public decimal DepositAmount { get; set; }
        public DateTime DepositDate { get; set; }
        public decimal InterestRate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
