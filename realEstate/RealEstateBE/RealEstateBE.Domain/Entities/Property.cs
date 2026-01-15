using RealEstateBE.Domain.Entities.Base;
using RealEstateBE.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateBE.Domain.Entities
{
    public  class Property :BaseEntity

    {

        public string Title { get; set; }    
        public string Description { get; set;  }
        public PropertyType Type { get; set; }  
        public PropertyStatus Status { get; set; }   
        public decimal Price { get; set;  } 
        public string Currency { get; set; }    
        public decimal Area { get; set;  }
        public int BedRooms { get; set; }   
        public int BathRooms { get; set;  }
        public int LivingRooms { get; set;  }
        public int? Floor { get; set; } 
        public int? TotalFloors { get; set; }   

        public int? BuildYear { get; set; }     

        public bool HasBalcony { get; set; }    

        public bool HasElavator { get; set;  }


        public bool HasParking { get; set; }        


        public bool IsFurnished { get; set; }   


        public string Country { get; set;  }    

public  string City { get; set;  }  

        public string District { get; set; }            

        public string Neighborhood { get; set; }        

        public string Address { get; set; }


        public string? PostalCode { get; set; }

        public decimal? Latitude { get; set;  } 

        public decimal? Longitute { get; set; }     

        public int ViewCount { get; set; }      

        public bool IsFeatured { get; set; }        

        public bool IsDeleted { get; set; }


        public bool IsPublished { get; set;  }  


        public string? VideoUrl { get; set; }       

        public string? VirtualTourUrl { get; set; }     

        public virtual User User { get; set; }      

        public int UserId { get; set;  }

        public virtual ICollection<Favorite> Favorites { get; set; }            

        public virtual ICollection<PropertyImage> Images { get; set; }      

        public virtual ICollection<Payment> Payments { get; set; }      
        public virtual ICollection<ContactMessage> ContactMessages { get; set;  }


        public string FullLocation => $"{Neighborhood},{District},{City}";

        public decimal PricePerSquareMeter => Area > 0 ? Price / Area : 0;




     
    }
}
