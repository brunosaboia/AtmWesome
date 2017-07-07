using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coinify.Web.Models
{
    public class CoinSize
    {
        public int CoinSizeId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Size (in mm)")]
        public int Size { get; set; }

        public override string ToString()
        {
            return $"{Size} mm";
        }
    }
}
