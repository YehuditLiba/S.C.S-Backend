﻿using Dal.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTO
{
    public class StationDTO
    {
        public int Number { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Neighborhood { get; set; }
        public bool IsFull { get; set; } 

        //להוסיף בDAL את NUMBER
        public StationDTO(int number, string street, string neighborhood, string city)
        {
            this.Number = number;
            this.Neighborhood = neighborhood;
            this.Street = street;
            this.City = city;
        }
        //public StationDTO(string street, string neighborhood, string city)
        //{
        //    this.Neighborhood = neighborhood;
        //    this.Street = street;
        //    this.City = city;
        //}
    }
}
