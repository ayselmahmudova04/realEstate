using RealEstateBE.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateBE.Domain.Entities
{
    public class PropertyImage : BaseEntity
    {
        public string ImageUrl { get; set; }    

        public string? AllText { get; set;  }   

        public int DisplayOrder { get; set;  }  

        public bool IsCover { get; set;  }  

        public int PropertyId { get; set;  }                

        public virtual Property Property { get; set;  } 


}
    }
    