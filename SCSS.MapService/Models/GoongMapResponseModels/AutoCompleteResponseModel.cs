namespace SCSS.MapService.Models.GoongMapResponseModels
{
    public class AutoCompleteResponseModel
    {
        public string Description { get; set; }

        public string[] Matched_substrings { get; set; }

        public string Place_id { get; set; }

        public string Reference { get; set; }

        public StructuredFormatting Structured_formatting { get; set; }

        public string[] Terms { get; set; }

        public bool Has_children { get; set; }

        public string Display_type { get; set; }

        public float Score { get; set; }

        public PlusCode Plus_code { get; set; }

        public Compound Compound { get; set; }
    }

    public class Compound
    {
        public string District { get; set; }

        public string Commune { get; set; }

        public string Province { get; set; }
    }

    public class StructuredFormatting
    {
        public string Main_text { get; set; }

        public string Secondary_text { get; set; }
    }

    public class PlusCode
    {
        public string Compound_code { get; set; }

        public string Global_code { get; set; }
    }



}
