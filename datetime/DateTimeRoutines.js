 function dateTimeRoutines()
 {
 		
 		this.getSecondsSinceUnixEpoch = getSecondsSinceUnixEpoch;
 		this.parsedDateTime; = parsedDateTime;
        var defaultDateIsNow = true;

 }

 function getSecondsSinceUnixEpoch(date_time){
 	  

 	  var t1 = new Date(date_time);
	  var t2 = new Date(1,1,1970);


	  var dif = t1.getTime() - t2.getTime();
      var ss = parseInt(dif);
      if (ss < 0)
                return 0;
      return ss;
 }

function parsedDateTime(){

    	var IndexOfDate = -1;
        /// <summary>
        /// Length a date substring found in the string
        /// </summary>
        var LengthOfDate = -1;
        /// <summary>
        /// Index of first char of a time substring found in the string
        /// </summary>
        var IndexOfTime = -1;
        /// <summary>
        /// Length of a time substring found in the string
        /// </summary>
        var LengthOfTime = -1;
        /// <summary>
        /// DateTime found in the string
        /// </summary>
       	var DateTime;
        /// <summary>
        /// True if a date was found within the string
        /// </summary>
        var IsDateFound;
        /// <summary>
        /// True if a time was found within the string
        /// </summary>
        var IsTimeFound;
        /// <summary>
        /// UTC offset if it was found within the string
        /// </summary>
        var utcOffset;
        /// <summary>
        /// True if UTC offset was found in the string
        /// </summary>

        this.isUtcOffsetFound;  = isUtcOffsetFound;
        this.utcDateTime = utcDateTime ;
        this.parsedDateTime = ParsedDateTime;

}

function isUtcOffsetFound(){

	get(){
		return Math.abs(this.utcOffset) ;
	}

}

function utcDateTime(){
    get(){

         if (!isUtcOffsetFound)
                        return new Date(1, 1, 1);
                    if (date == new Date(1, 1, 1))//to avoid negative date exception when date is undefined
                        return date + new Date(24, 0, 0) + utcOffset;
                    return date + utcOffset;
    }


}

function ParsedDateTime(){

     var indexOfDate = index_of_date;
     var lengthOfDate = length_of_date;
     var indexOfTime = index_of_time;
     var lengthOfTime = length_of_time;
     var dateTime = date_time;
     var isDateFound = index_of_date > -1;
     var isTimeFound = index_of_time > -1;
     var utcOffset = new Date(25, 0, 0);

}

function defaultDate(){
    set{
         _defaultDate = value;
         defaultDateIsNow = false;
    }

    get{
       if (defaultDateIsNow)
            return new Date();
            else
                return _defaultDate;
        }

}

var DateTimeFormat = {
  USA_DATE , 
  UK_DATE
};

function tryParseDateOrTime(str,  default_format,  date_time)
        {
            parsedDateTime parsed_date_time;
            if (!tryParseDateOrTime(str, default_format,  parsed_date_time))
            {
                date_time = new Date(1, 1, 1);
                return false;
            }
            date_time = parsed_date_time.dateTime;
            return true;
        }


function tryParseTime(str, default_format, time)
        {
            parsedDateTime parsed_time;
            if (!tryParseTime(str, default_format,  parsed_time))
            {
                time = new Date(1, 1, 1);
                return false;
            }
            time = parsed_time.dateTime;
            return true;
        }

function tryParseDate(str, default_format, date)
        {
            parsedDateTime parsed_date;
            if (!tryParseDate(str, default_format,  parsed_date))
            {
                date = new Date(1, 1, 1);
                return false;
            }
            date = parsed_date.dateTime;
            return true;
        }       


function tryParseTime(str, default_format,  parsed_time, parsed_date)
        {
            parsed_time = null;

            var time_zone_r;
            if(default_format == DateTimeFormat.USA_DATE)
                time_zone_r = "(?:\s*(?'time_zone'UTC|GMT|CST|EST))?";
            else
                time_zone_r = "(?:\s*(?'time_zone'UTC|GMT))?";

            var m;
            if (parsed_date != null && parsed_date.IndexOfDate > -1)
            {//look around the found date
                //look for <date> hh:mm:ss <UTC offset> 
                m = str.Substring(parsed_date.IndexOfDate + parsed_date.LengthOfDate).Match("(?<=^\s*,?\s+|^\s*at\s*|^\s*[T\-]\s*)(?'hour'\d{2})\s*:\s*(?'minute'\d{2})\s*:\s*(?'second'\d{2})\s+(?'offset_sign'[\+\-])(?'offset_hh'\d{2}):?(?'offset_mm'\d{2})(?=$|[^\d\w])" );
                if (!m)
                    //look for <date> [h]h:mm[:ss] [PM/AM] [UTC/GMT] 
                    m = str.Substring(parsed_date.IndexOfDate + parsed_date.LengthOfDate).Match("(?<=^\s*,?\s+|^\s*at\s*|^\s*[T\-]\s*)(?'hour'\d{1,2})\s*:\s*(?'minute'\d{2})\s*(?::\s*(?'second'\d{2}))?(?:\s*(?'ampm'AM|am|PM|pm))?"+time_zone_r+"(?=$|[^\d\w])");
                if (!m)
                    //look for [h]h:mm:ss [PM/AM] [UTC/GMT] <date>
                    m = str.Substring(0, parsed_date.IndexOfDate).Match("(?<=^|[^\d])(?'hour'\d{1,2})\s*:\s*(?'minute'\d{2})\s*(?::\s*(?'second'\d{2}))?(?:\s*(?'ampm'AM|am|PM|pm))?"+time_zone_r+"(?=$|[\s,]+)");
                if (!m)
                    //look for [h]h:mm:ss [PM/AM] [UTC/GMT] within <date>
                    m = str.Substring(parsed_date.IndexOfDate, parsed_date.LengthOfDate).Match("(?<=^|[^\d])(?'hour'\d{1,2})\s*:\s*(?'minute'\d{2})\s*(?::\s*(?'second'\d{2}))?(?:\s*(?'ampm'AM|am|PM|pm))?"+time_zone_r+"(?=$|[\s,]+)");
            }
            else//look anywhere within string
            {
                //look for hh:mm:ss <UTC offset> 
                m = str.Match("(?<=^|\s+|\s*T\s*)(?'hour'\d{2})\s*:\s*(?'minute'\d{2})\s*:\s*(?'second'\d{2})\s+(?'offset_sign'[\+\-])(?'offset_hh'\d{2}):?(?'offset_mm'\d{2})?(?=$|[^\d\w])");
                if (!m)
                    //look for [h]h:mm[:ss] [PM/AM] [UTC/GMT]
                    m = str.Match("(?<=^|\s+|\s*T\s*)(?'hour'\d{1,2})\s*:\s*(?'minute'\d{2})\s*(?::\s*(?'second'\d{2}))?(?:\s*(?'ampm'AM|am|PM|pm))?"+time_zone_r+"(?=$|[^\d\w])");
            }

            if (!m)
                return false;

            //try
            //{
            var hour = parseInt(m.Groups["hour"]);
            if (hour < 0 || hour > 23)
                return false;

            var minute = parseInt(m.Groups["minute"]);
            if (minute < 0 || minute > 59)
                return false;

            var second = 0;
            if (m.Groups["second"].Value != null)
            {
                second = parseInt(m.Groups["second"]);
                if (second < 0 || second > 59)
                    return false;
            }

            if (    m.Groups["ampm"] ==  "PM"  && hour < 12)
                hour += 12;
            else if ( m.Groups["ampm"] == "AM" && hour == 12)
                hour -= 12;

            var date_time = new Date(1, 1, 1, hour, minute, second);
            
            if (m.Groups["offset_hh"])
            {
                var offset_hh = parseInt(m.Groups["offset_hh"]);
                var offset_mm = 0;
                if (m.Groups["offset_mm"])
                    offset_mm = parseInt(m.Groups["offset_mm"]);
                var utc_offset = new Date(offset_hh, offset_mm, 0);
                if (m.Groups["offset_sign"] == "-")
                    utc_offset = -utc_offset;
                parsed_time = new parsedDateTime(-1, -1, m.Index, m.Length, date_time, utc_offset);
                return true;
            }

            if (m.Groups["time_zone"])
            {
                var utc_offset;
                switch (m.Groups["time_zone"])
                {
                    case "UTC":
                    case "GMT":
                    utc_offset = new Date(0, 0, 0);
                        break;
                    case "CST":
                         = new Date(-6, 0, 0);
                        break;
                    case "EST":
                        utc_offset = new Date(-5, 0, 0);
                        break;
             
                }
                parsed_time = new parsedDateTime(-1, -1, m.Index, m.Length, date_time, utc_offset);
                return true;
            }

            parsed_time = new parsedDateTime(-1, -1, m.Index, m.Length, date_time);
            //}
            //catch(Exception e)
            //{
            //    return false;
            //}
            return true;
        }


        function tryParseDate(str, default_format, parsed_date)
        {
            parsed_date = null;

            if (str != null )
                return false;

            //look for dd/mm/yy
            var m = str.Match("(?<=^|[^\d])(?'day'\d{1,2})\s*(?'separator'[\\/\.])+\s*(?'month'\d{1,2})\s*\'separator'+\s*(?'year'\d{2}|\d{4})(?=$|[^\d])");
            if (m)
            {
                Date date;
                if ((default_format ^ dateTimeFormat.USA_DATE) == dateTimeFormat.USA_DATE)
                {
                    if (!convert_to_date(parseInt(m.Groups["year"]), parseInt(m.Groups["day"]), parseInt(m.Groups["month"]), out date))
                        return false;
                }
                else
                {
                    if (!convert_to_date(parseInt(m.Groups["year"]), parseInt(m.Groups["month"]), parseInt(m.Groups["day"])))
                        return false;
                }
                parsed_date = new parsedDateTime(m.Index, m.Length, -1, -1, date);
                return true;
            }

            //look for [yy]yy-mm-dd
            m = str.Match("(?<=^|[^\d])(?'year'\d{2}|\d{4})\s*(?'separator'[\-])\s*(?'month'\d{1,2})\s*\'separator'+\s*(?'day'\d{1,2})(?=$|[^\d])");
            if (m.Success)
            {
                Date date;
                if (!convert_to_date(int.Parse(m.Groups["year"].Value), int.Parse(m.Groups["month"].Value), int.Parse(m.Groups["day"].Value), out date))
                    return false;
                parsed_date = new parsedDateTime(m.Index, m.Length, -1, -1, date);
                return true;
            }

            //look for month dd yyyy
            m = str.Match("(?:^|[^\d\w])(?'month'Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[uarychilestmbro]*\s+(?'day'\d{1,2})(?:-?st|-?th|-?rd|-?nd)?\s*,?\s*(?'year'\d{4})(?=$|[^\d\w])");
            if (!m)
                //look for dd month [yy]yy
                m = str.Match("(?:^|[^\d\w:])(?'day'\d{1,2})(?:-?st\s+|-?th\s+|-?rd\s+|-?nd\s+|-|\s+)(?'month'Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[uarychilestmbro]*(?:\s*,?\s*|-)'?(?'year'\d{2}|\d{4})(?=$|[^\d\w])");
            if (!m.Success)
                //look for yyyy month dd
                m = str.Match("(?:^|[^\d\w])(?'year'\d{4})\s+(?'month'Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[uarychilestmbro]*\s+(?'day'\d{1,2})(?:-?st|-?th|-?rd|-?nd)?(?=$|[^\d\w])");
            if (!m.Success)
                //look for month dd hh:mm:ss MDT|UTC yyyy
                m = str.Match("(?:^|[^\d\w])(?'month'Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[uarychilestmbro]*\s+(?'day'\d{1,2})\s+\d{2}\:\d{2}\:\d{2}\s+(?:MDT|UTC)\s+(?'year'\d{4})(?=$|[^\d\w])");
            if (!m.Success)
                //look for  month dd [yyyy]
                m = str.Match("(?:^|[^\d\w])(?'month'Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[uarychilestmbro]*\s+(?'day'\d{1,2})(?:-?st|-?th|-?rd|-?nd)?(?:\s*,?\s*(?'year'\d{4}))?(?=$|[^\d\w])");
            if (m)
            {
                var month = -1;
                var index_of_date = m.Index;
                var length_of_date = m.Length;

                switch (m.Groups["month"].Value)
                {
                    case "Jan":
                    case "JAN":
                        month = 1;
                        break;
                    case "Feb":
                    case "FEB":
                        month = 2;
                        break;
                    case "Mar":
                    case "MAR":
                        month = 3;
                        break;
                    case "Apr":
                    case "APR":
                        month = 4;
                        break;
                    case "May":
                    case "MAY":
                        month = 5;
                        break;
                    case "Jun":
                    case "JUN":
                        month = 6;
                        break;
                    case "Jul":
                        month = 7;
                        break;
                    case "Aug":
                    case "AUG":
                        month = 8;
                        break;
                    case "Sep":
                    case "SEP":
                        month = 9;
                        break;
                    case "Oct":
                    case "OCT":
                        month = 10;
                        break;
                    case "Nov":
                    case "NOV":
                        month = 11;
                        break;
                    case "Dec":
                    case "DEC":
                        month = 12;
                        break;
                }

                var year;
                if (m.Groups["year"] != null)
                    year = parseInt(m.Groups["year"]);
                else
                    year = defaultDate.Year;

                DateTime date;
                if (!convert_to_date(year, month, parseInt(m.Groups["day"].Value), out date))
                    return false;
                parsed_date = new parsedDateTime(index_of_date, length_of_date, -1, -1, date);
                return true;
            }

            return false;
        }

        function convert_to_date(year, month,  day,  date)
        {
            if (year >= 100)
            {
                if (year < 1000)
                {
                    date = new Date(1, 1, 1);
                    return false;
                }
            }
            else
                if (year > 30)
                    year += 1900;
                else
                    year += 2000;

            try
            {
                date = new Date(year, month, day);
            }
            catch
            {
                date = new Date(1, 1, 1);
                return false;
            }
            return true;
        }