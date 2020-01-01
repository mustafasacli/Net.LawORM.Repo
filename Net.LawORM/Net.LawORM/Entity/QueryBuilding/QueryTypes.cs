namespace Net.LawORM.Entity.QueryBuilding
{
    public enum QueryTypes : byte
    {
        Select = 1,
        Insert = 2,
        Update = 3,
        Delete = 4,
        InsertAndGetId = 5,
        SelectWhereId = 6,
        SelectWhereChangeColumns = 7,
        SelectChangeColumns = 8
    };
}
