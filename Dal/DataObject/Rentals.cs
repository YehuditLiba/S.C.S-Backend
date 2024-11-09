using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.DataObject
{
    public partial class Rentals
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CarId {  get; set; }
        public int UserId{ get; set; }
        public double Price { get; set; }
        public DateTime? StartDate { get; set; } 
        public DateTime? EndDate { get; set; }

        public  Car Car { get; set; }
        public  User User { get; set; }
    }
}
