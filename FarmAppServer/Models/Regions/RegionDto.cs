namespace FarmAppServer.Models.Regions
{
    public class RegionDto
    {
        public int Id { get; set; }//id
        public int ParentId { get; set; }
        //public string ParentRegionName { get; set; }//parent region name
        //public int RegionTypeId { get; set; }//RegionTypeId
        public string Name { get; set; }//region name
        //public string RegionTypeName { get; set; }
        public int Population { get; set; }
        public bool IsDeleted { get; set; }
    }
}
