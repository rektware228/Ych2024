using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalPracticeAutumn2024.DB
{
    public partial class ClientService
    {
        public string OstatokHours
        {
            get
            {
                int time = (int)(StartTime - DateTime.Now).TotalMinutes;
                string result = "";
                int hours = time / 60;
                int minutes = time % 60;

                if (hours > 0)
                {
                    result += $"{hours} ";
                    if (hours % 100 >= 11 && hours % 100 <= 14)
                    {
                        result += "часов";
                    }
                    else if (hours % 10 == 1)
                    {
                        result += "час";
                    }
                    else if (hours % 10 >= 2 && hours % 10 <= 4)
                    {
                        result += "часа";
                    }
                    else
                    {
                        result += "часов";
                    }
                    result += " ";
                }
                if (hours == 0 || minutes > 0)
                {
                    result += $"{minutes} ";
                    if (minutes >= 11 && minutes <= 14)
                    {
                        result += "минут";
                    }
                    else if (minutes % 10 == 1)
                    {
                        result += "минуту";
                    }
                    else if (minutes % 10 >= 2 && minutes % 10 <= 4)
                    {
                        result += "минуты";
                    }
                    else
                    {
                        result += "минут";
                    }
                }
                return result;
            }
        }

        public string LeftOurs
        {
            get
            {
                int time = (int)(StartTime - DateTime.Now).TotalMinutes;

                if (time < 60)
                {
                    return "Red";
                }
                else
                {
                    return "Black";
                }
            }
        }

    }
}

