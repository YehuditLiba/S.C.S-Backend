﻿using Dal.DataObject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = Dal.DataObject.Point;

namespace Dal.Interfaces
{
    public interface IStationRepository : IRepository<Station>
    {
        public Task<Station> GetNearestStation(bool fullStation, bool isMustCenteral,Point point, string street, string neighorhood, string city);
    }
}
