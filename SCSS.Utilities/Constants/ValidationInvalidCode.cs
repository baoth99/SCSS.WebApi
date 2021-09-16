namespace SCSS.Utilities.Constants
{
    public class InvalidFileCode
    {
        public const string Extension = "IFE0001";
        public const string Size = "IFS0002";
        public const string Name = "IFN0003";
        public const string Null = "IFNULL0003";
    }

    public class InvalidTextCode
    {
        public const string Empty = "IET0001";
        public const string MaxLength = "IET0001";
        public const string AccountStatus = "IACCS001";
        public const string PhoneRegex = "IPR0001";
        public const string DateTime = "DT0001";
        public const string TimeSpan = "TS0001";
        public const string DateTimeNow = "DTN0001";
        public const string TimeSpanNow = "TSN001";
    }

    public class InvalidScrapCategory
    {
        public const string Empty = "IESC0001";
        public const string SCDetail = "ISCD0001";
        public const string DuplicateSCDetail = "DISCD0001";
    }

    public class InvalidCollectingRequestCode
    {
        public const string MoreThan7Days = "MT7DS";
        public const string FromTimeGreaterThanToTime = "FTGTTT";
        public const string ToTimeGreaterThanToTime = "TTGTTT";
        public const string LessThan15Minutes = "LT15MS";
    }
}
