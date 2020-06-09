﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Helpers
{
    public static class DateTimeOffsetExtensions
    {
        /// <summary>
        /// In base a current age obtain the age
        /// </summary>
        /// <param name="dateTimeOffset">value of DB</param>
        /// <returns>actual age</returns>
        public static int GetCurrentAge(this DateTimeOffset dateTimeOffset)
        {
            var currentDate = DateTime.UtcNow;
            int age = currentDate.Year - dateTimeOffset.Year;
            if (currentDate < dateTimeOffset.AddYears(age))
            {
                age--;
            }
            return age;
        }
    }
}
