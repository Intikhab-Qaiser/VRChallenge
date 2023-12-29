using VRChallenge.DB;
using VRChallenge.DB.Model;

namespace VRChallenge.Service
{
    public class DbService: IDbService
    {
        private readonly string tableName;

        public DbService(string tableName)
        {
            this.tableName = tableName;
        }

        async Task IDbService.Save(List<Domain.Box> data)
        {
            var dbData = ConvertDomainModelToDbModel(data);

            using (var context = new BoxDbContext(tableName))
            {
                context.Boxes.AddRange(dbData);
                await context.SaveChangesAsync();
            }
        }

        private List<Box> ConvertDomainModelToDbModel(List<Domain.Box> data)
        {
            var dbData = new List<Box>();
            foreach (var box in data)
            {
                if (box != null)
                {
                    var items = new List<Item>();
                    foreach (var content in box.Contents)
                    {
                        var item = new Item
                        {
                            Id = Guid.NewGuid(),
                            PoNumber = content.PoNumber,
                            Quantity = content.Quantity,
                            Isbn = content.Isbn
                        };
                        items.Add(item);
                    }

                    var newBox = new Box
                    {
                        Id = Guid.NewGuid(),
                        Identifier = box.Identifier,
                        SupplierIdentifier = box.SupplierIdentifier,
                        Items = items
                    };

                    dbData.Add(newBox);
                }
            }

            return dbData;
        }

    }
}
