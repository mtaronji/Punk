
using Punk.FREDDataModels;
using Punk.SP500StockModels;

namespace Punk.DBContextTests
{
    
    public class DBContexts_Should_Work
    {
        private FredContext fred;
        private SP500Context sp500;
        public DBContexts_Should_Work()
        {
            this.fred = new FredContext();
            this.sp500 = new SP500Context();
        }

        [Fact]
        public async Task SP500Context_Should_Work()
        {
            var smas = await sp500.SMA(20, "SPY", "2020-01-04", "2021-01-05"); 
            Assert.True(smas.Count() > 0);
            var emas = await sp500.EMA(8, "SPY", "2022-01-01"); 
            Assert.True(emas.Count() > 0);

          
        }

        [Fact]
        public async Task FREDContext_Should_Work()
        {
          
            var observations = await fred.GetObservations("T10Y2Y", "2020-01-04", "2021-01-05");
            Assert.True(observations.Count() > 0);
            var smas = await fred.SMA(20, "T10Y2Y", "2020-01-04", "2021-01-05");
            Assert.True(smas.Count() > 0);
            var emas = await fred.EMA(8, "T10Y2Y", "2022-01-01");
            Assert.True(emas.Count() > 0);


        }
        //[Fact]
        //public async Task SP500oContext_Should_Work()
        //{
        //    var smas = await sp500o.SMA(20, "QQQ220218C00340000");
        //    Assert.True(smas.Count() > 0);
        //    var emas = await sp500o.EMA(20, "QQQ220218C00340000");
        //    Assert.True(emas.Count() > 0);
        //}


    }
    
}
