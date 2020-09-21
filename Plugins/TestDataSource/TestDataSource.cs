

namespace TestDataSource
{
    public class TestDataSource : ZigZag.DataSource
    {
        
        public override ZigZag.Color GetColor()
        {
            return new ZigZag.Color(100, 100, 100);
        }

    }
}
